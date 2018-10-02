using UnityEngine;
using System;//Uri
using System.IO;//Stream
using System.Net;//WebExceptionStatus

public class WaitForDownload : CustomYieldInstruction
{
    private bool _keepWaiting = true;
    public override bool keepWaiting
    {
        get
        {
            return _keepWaiting;
        }
    }

    private WebExceptionStatus _status;
    public WebExceptionStatus status
    {
        get
        {
            return _status;
        }
    }

    public bool isDone
    {
        get
        {
            return _status == WebExceptionStatus.Success;
        }
    }
    public void Success()
    {
        _keepWaiting = false;
        _status = WebExceptionStatus.Success;
    }
    public void Failed(WebExceptionStatus status)
    {
        _keepWaiting = false;
        _status = status;
    }
}


public class DownloadFileState : HttpWebRequestState
{
    public Stream streamWrite;
    public int bytesStart = 0;
    public bool isCancel = false;

    public DownloadFileState(int buffSize) : base(buffSize) { }
}
public static class HttpRequestDownloader
{
    private const int BUFFER_SIZE = 1024 * 1024;

    private static HttpWebRequest request;
    private static DownloadFileState downloadState;
    private static WaitForDownload coroutine;


    public static WaitForDownload DownloadFile(string url, Stream stream, ProgressDelegate progress = null)
    {
        coroutine = new WaitForDownload();

        int length = Mathf.Max(0, (int)stream.Length - BUFFER_SIZE);
        stream.Seek(length, SeekOrigin.Begin);

        Uri uri = new Uri(url);

        request = (HttpWebRequest)HttpWebRequest.Create(uri);
        request.AddRange(length);

        downloadState = new DownloadFileState(BUFFER_SIZE);
        downloadState.request = request;
        downloadState.fileURI = uri;
        downloadState.respInfoCB = null;// new ResponseInfoDelegate(SetResponseInfo);
        downloadState.progCB = progress;
        downloadState.doneCB = new DoneDelegate(coroutine.Success);
        downloadState.transferStart = DateTime.Now;
        downloadState.streamWrite = stream;
        downloadState.bytesStart = length;
        downloadState.bytesRead = length;
        downloadState.isCancel = false;

        // Start the asynchronous request.
        request.BeginGetResponse(new AsyncCallback(RespCallback), downloadState);

        return coroutine;
    }

    public static void StopDownload()
    {
        if( downloadState != null )
        {
            downloadState.isCancel = true;
        }
    }

    private static void RespCallback(IAsyncResult asyncResult)
    {
        //WebRequestState state2 = ((WebRequestState)(asyncResult.AsyncState));
        //WebRequest req3 = state2.request;

        //try
        //{
        //    WebResponse wr = req3.EndGetResponse(asyncResult);

        //    HttpWebResponse resp3 = (HttpWebResponse)wr;
        //}
        //catch (Exception e)
        //{
        //    Debug.Log(e.Message);
        //}


        try
        {
            // Will be either HttpWebRequestState or FtpWebRequestState
            WebRequestState state = ((WebRequestState)(asyncResult.AsyncState));
            WebRequest req = state.request;
            string statusDescr = "";
            string contentLength = "";

            // HTTP
            if (state.fileURI.Scheme == Uri.UriSchemeHttp)
            {
                HttpWebResponse resp = ((HttpWebResponse)(req.EndGetResponse(asyncResult)));
                state.response = resp;
                statusDescr = resp.StatusDescription;
                state.totalBytes = state.response.ContentLength;
                contentLength = state.response.ContentLength.ToString();// # bytes
            }
            // FTP part 1 - response to GetFileSize command
            else if ((state.fileURI.Scheme == Uri.UriSchemeFtp) && (state.FTPMethod == WebRequestMethods.Ftp.GetFileSize))
            {
                // First FTP command was GetFileSize, so this 1st response is the size of
                // the file.
                FtpWebResponse resp = ((FtpWebResponse)(req.EndGetResponse(asyncResult)));
                statusDescr = resp.StatusDescription;
                state.totalBytes = resp.ContentLength;
                contentLength = resp.ContentLength.ToString();   // # bytes
            }
            // FTP part 2 - response to DownloadFile command
            else if ((state.fileURI.Scheme == Uri.UriSchemeFtp) && (state.FTPMethod == WebRequestMethods.Ftp.DownloadFile))
            {
                FtpWebResponse resp = ((FtpWebResponse)(req.EndGetResponse(asyncResult)));
                state.response = resp;
            }
            else
            {
                throw new ApplicationException("Unexpected URI");
            }

            // Get this info back to the GUI -- max # bytes, so we can do progress bar
            if (statusDescr != "" && state.respInfoCB != null)
            {
                state.respInfoCB(statusDescr, contentLength);
            }

            // FTP part 1 done, need to kick off 2nd FTP request to get the actual file
            if ((state.fileURI.Scheme == Uri.UriSchemeFtp) && (state.FTPMethod == WebRequestMethods.Ftp.GetFileSize))
            {
                // Note: Need to create a new FtpWebRequest, because we're not allowed to change .Method after
                // we've already submitted the earlier request.  I.e. FtpWebRequest not recyclable.
                // So create a new request, moving everything we need over to it.
                FtpWebRequest req2 = (FtpWebRequest)FtpWebRequest.Create(state.fileURI);
                req2.Credentials = req.Credentials;
                req2.UseBinary = true;
                req2.KeepAlive = true;
                req2.Method = WebRequestMethods.Ftp.DownloadFile;

                state.request = req2;
                state.FTPMethod = WebRequestMethods.Ftp.DownloadFile;

                // Start the asynchronous request, which will call back into this same method
                req2.BeginGetResponse(new AsyncCallback(RespCallback), state);
            }
            // HTTP or FTP part 2 -- we're ready for the actual file download
            else
            {
                // Set up a stream, for reading response data into it
                Stream responseStream = state.response.GetResponseStream();
                state.streamResponse = responseStream;

                // Begin reading contents of the response data
                responseStream.BeginRead(state.bufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallback), state);
            }

            return;
        }
        catch (WebException e)
        {
            Debug.Log(string.Format("WebException in ReadCallback(): {0}", e.Message));
            Failed(e.Status);
        }
        catch (Exception e)
        {
            Debug.Log(string.Format("exception in RespCallback(): {0}", e.Message));
            Failed(WebExceptionStatus.UnknownError);
        }
    }

    static void ReadCallback(IAsyncResult asyncResult)
    {
        try
        {
            // Will be either HttpWebRequestState or FtpWebRequestState
            DownloadFileState state = ((DownloadFileState)(asyncResult.AsyncState));
            if (state.isCancel)
            {
                SafeClose();
                return;
            }

            // Get results of read operation
            int bytesRead = state.streamResponse.EndRead(asyncResult);

            // Got some data, need to read more
            if (bytesRead > 0)
            {
                // Report some progress, including total # bytes read, % complete, and transfer rate
                state.bytesRead += bytesRead;
                //double pctComplete = ((double)state.bytesRead / (double)(state.bytesStart + state.totalBytes)) * 100.0f;

                // Note: bytesRead/totalMS is in bytes/ms.  Convert to kb/sec.
                //TimeSpan totalTime = DateTime.Now - state.transferStart;
                //double kbPerSec = (state.bytesRead * 1000.0f) / (totalTime.TotalMilliseconds * 1024.0f);

                state.streamWrite.Write(state.bufferRead, 0, bytesRead);
                if (state.progCB != null)
                {
                    state.progCB(state.bytesRead, state.bytesStart + (int)state.totalBytes);
                }

                // Kick off another read
                state.streamResponse.BeginRead(state.bufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallback), state);
                return;
            }
            // EndRead returned 0, so no more data to be read
            else
            {
                SafeClose();

                state.doneCB();
            }
        }
        catch (WebException e)
        {
            Debug.Log(string.Format("exception in ReadCallback(): {0}", e.Message));

            Failed(e.Status);
        }
        catch (Exception e)
        {
            Debug.Log(string.Format("exception in ReadCallback(): {0}", e.Message));

            Failed(WebExceptionStatus.UnknownError);
        }
    }

    static void SafeClose()
    {
        if (downloadState.streamWrite != null)
        {
            downloadState.streamWrite.Close();
        }

        if (downloadState.streamResponse != null)
        {
            downloadState.streamResponse.Close();
        }

        if (downloadState.response != null)
        {
            downloadState.response.Close();
        }
    }

    static void Failed(WebExceptionStatus status)
    {
        SafeClose();

        coroutine.Failed(status);
    }

    public static bool Exists(string url)
    {
        bool result = true;

        WebRequest webRequest = WebRequest.Create(url);
        webRequest.Timeout = 1200;
        webRequest.Method = "HEAD";

        try
        {
            webRequest.GetResponse();
        }
        catch
        {
            result = false;
        }

        return result;
    }



}


public class FTPDownloadFileState : FtpWebRequestState
{
    public Stream streamWrite;
    public int bytesStart = 0;
    public bool isCancel = false;

    public FTPDownloadFileState(int buffSize) : base(buffSize) { }
}
public static class FTPRequestDownloader
{
    private const int BUFFER_SIZE = 1024 * 1024;

    private static FtpWebRequest request;
    private static FTPDownloadFileState downloadState;
    private static WaitForDownload coroutine;


    public static WaitForDownload DownloadFile(string url, Stream stream, ProgressDelegate progress = null)
    {
        coroutine = new WaitForDownload();

        int length = Mathf.Max(0, (int)stream.Length - BUFFER_SIZE);
        stream.Seek(length, SeekOrigin.Begin);

        Uri uri = new Uri(url);
        request = (FtpWebRequest)WebRequest.Create(uri);
        //request.AddRange(length);

        request.Method = WebRequestMethods.Ftp.DownloadFile;
        request.Credentials = new NetworkCredential("anonymous", "");

        downloadState = new FTPDownloadFileState(BUFFER_SIZE);
        downloadState.request = request;
        downloadState.fileURI = uri;
        downloadState.respInfoCB = null;// new ResponseInfoDelegate(SetResponseInfo);
        downloadState.progCB = progress;
        downloadState.doneCB = new DoneDelegate(coroutine.Success);
        downloadState.transferStart = DateTime.Now;
        downloadState.streamWrite = stream;
        downloadState.bytesStart = length;
        downloadState.bytesRead = length;
        downloadState.isCancel = false;

        downloadState.FTPMethod = WebRequestMethods.Ftp.DownloadFile;

        // Start the asynchronous request.
        request.BeginGetResponse(new AsyncCallback(RespCallback), downloadState);

        return coroutine;
    }


    public static void StopDownload()
    {
        if (downloadState != null)
        {
            downloadState.isCancel = true;
        }
    }


    static void RespCallback(IAsyncResult asyncResult)
    {
        //using (FtpWebResponse resp = (FtpWebResponse)req.GetResponse())
        //{
        //    // FTP 결과 스트림
        //    Stream stream = resp.GetResponseStream();

        //    // 결과를 문자열로 읽기 (바이너리로 읽을 수도 있다)
        //    string data;
        //    using (StreamReader reader = new StreamReader(stream))
        //    {
        //        data = reader.ReadToEnd();
        //    }

        //}


        try
        {
            // Will be either HttpWebRequestState or FtpWebRequestState
            WebRequestState state = ((WebRequestState)(asyncResult.AsyncState));
            WebRequest req = state.request;
            string statusDescr = "";
            string contentLength = "";

            // HTTP
            if (state.fileURI.Scheme == Uri.UriSchemeHttp)
            {
                HttpWebResponse resp = ((HttpWebResponse)(req.EndGetResponse(asyncResult)));
                state.response = resp;
                statusDescr = resp.StatusDescription;
                state.totalBytes = state.response.ContentLength;
                contentLength = state.response.ContentLength.ToString();   // # bytes
            }
            // FTP part 1 - response to GetFileSize command
            else if ((state.fileURI.Scheme == Uri.UriSchemeFtp) && (state.FTPMethod == WebRequestMethods.Ftp.GetFileSize))
            {
                // First FTP command was GetFileSize, so this 1st response is the size of
                // the file.
                FtpWebResponse resp = ((FtpWebResponse)(req.EndGetResponse(asyncResult)));
                statusDescr = resp.StatusDescription;
                state.totalBytes = resp.ContentLength;
                contentLength = resp.ContentLength.ToString();   // # bytes
            }
            // FTP part 2 - response to DownloadFile command
            else if ((state.fileURI.Scheme == Uri.UriSchemeFtp) && (state.FTPMethod == WebRequestMethods.Ftp.DownloadFile))
            {
                FtpWebResponse resp = ((FtpWebResponse)(req.EndGetResponse(asyncResult)));
                state.response = resp;
            }
            else
            {
                throw new ApplicationException("Unexpected URI");
            }

            // Get this info back to the GUI -- max # bytes, so we can do progress bar
            if (statusDescr != "" && state.respInfoCB != null)
            {
                state.respInfoCB(statusDescr, contentLength);
            }

            // FTP part 1 done, need to kick off 2nd FTP request to get the actual file
            if ((state.fileURI.Scheme == Uri.UriSchemeFtp) && (state.FTPMethod == WebRequestMethods.Ftp.GetFileSize))
            {
                // Note: Need to create a new FtpWebRequest, because we're not allowed to change .Method after
                // we've already submitted the earlier request.  I.e. FtpWebRequest not recyclable.
                // So create a new request, moving everything we need over to it.
                FtpWebRequest req2 = (FtpWebRequest)FtpWebRequest.Create(state.fileURI);
                req2.Credentials = req.Credentials;
                req2.UseBinary = true;
                req2.KeepAlive = true;
                req2.Method = WebRequestMethods.Ftp.DownloadFile;

                state.request = req2;
                state.FTPMethod = WebRequestMethods.Ftp.DownloadFile;

                // Start the asynchronous request, which will call back into this same method
                req2.BeginGetResponse(new AsyncCallback(RespCallback), state);
            }
            // HTTP or FTP part 2 -- we're ready for the actual file download
            else
            {
                // Set up a stream, for reading response data into it
                Stream responseStream = state.response.GetResponseStream();
                state.streamResponse = responseStream;

                // Begin reading contents of the response data
                responseStream.BeginRead(state.bufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallback), state);
            }

            return;
        }
        catch (WebException e)
        {
            Debug.Log(string.Format("exception in ReadCallback(): {0}", e.Message));

            Failed(e.Status);
        }
        catch (Exception e)
        {
            Debug.Log(string.Format("exception in RespCallback(): {0}", e.Message));

            Failed(WebExceptionStatus.UnknownError);
        }
    }


    static void ReadCallback(IAsyncResult asyncResult)
    {
        try
        {
            // Will be either HttpWebRequestState or FtpWebRequestState
            FTPDownloadFileState state = ((FTPDownloadFileState)(asyncResult.AsyncState));
            if (state.isCancel)
            {
                SafeClose();
                return;
            }

            // Get results of read operation
            int bytesRead = state.streamResponse.EndRead(asyncResult);

            // Got some data, need to read more
            if (bytesRead > 0)
            {
                // Report some progress, including total # bytes read, % complete, and transfer rate
                state.bytesRead += bytesRead;
                //double pctComplete = ((double)state.bytesRead / (double)(state.bytesStart + state.totalBytes)) * 100.0f;

                // Note: bytesRead/totalMS is in bytes/ms.  Convert to kb/sec.
                //TimeSpan totalTime = DateTime.Now - state.transferStart;
                //double kbPerSec = (state.bytesRead * 1000.0f) / (totalTime.TotalMilliseconds * 1024.0f);

                state.streamWrite.Write(state.bufferRead, 0, bytesRead);
                if (state.progCB != null)
                {
                    state.progCB(state.bytesRead, state.bytesStart + (int)state.totalBytes);
                }

                // Kick off another read
                state.streamResponse.BeginRead(state.bufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallback), state);
                return;
            }
            // EndRead returned 0, so no more data to be read
            else
            {
                SafeClose();

                state.doneCB();
            }
        }
        catch (WebException e)
        {
            Debug.Log(string.Format("exception in ReadCallback(): {0}", e.Message));

            Failed(e.Status);
        }
        catch (Exception e)
        {
            Debug.Log(string.Format("exception in ReadCallback(): {0}", e.Message));

            Failed(WebExceptionStatus.UnknownError);
        }
    }


    static void SafeClose()
    {
        if (downloadState.streamWrite != null)
        {
            downloadState.streamWrite.Close();
        }

        if (downloadState.streamResponse != null)
        {
            downloadState.streamResponse.Close();
        }

        if (downloadState.response != null)
        {
            downloadState.response.Close();
        }
    }


    static void Failed(WebExceptionStatus status)
    {
        SafeClose();

        coroutine.Failed(status);
    }


    public static bool Exists(string url)
    {
        bool result = true;

        WebRequest webRequest = WebRequest.Create(url);
        webRequest.Timeout = 1200;
        webRequest.Method = "HEAD";

        try
        {
            webRequest.GetResponse();
        }
        catch
        {
            result = false;
        }

        return result;
    }
}
