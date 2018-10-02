using System;
using System.IO;
using System.Net;

public delegate void ResponseInfoDelegate(string statusDescr, string contentLength);
public delegate void ProgressDelegate(int bytesRead, int totalBytes);
public delegate void DoneDelegate();

/// <summary>
/// Base class for state object that gets passed around amongst async methods
/// when doing async web request/response for data transfer.  We store basic
/// things that track current state of a download, including # bytes transfered,
/// as well as some async callbacks that will get invoked at various points.
/// </summary>
abstract public class WebRequestState
{
    public int bytesRead;           // # bytes read during current transfer
    public long totalBytes;            // Total bytes to read
    public double progIncrement;    // delta % for each buffer read
    public Stream streamResponse;    // Stream to read from
    public byte[] bufferRead;        // Buffer to read data into
    public Uri fileURI;                // Uri of object being downloaded
    public string FTPMethod;        // What was the previous FTP command?  (e.g. get file size vs. download)
    public DateTime transferStart;  // Used for tracking xfr rate, 다운로드 시작 시간을 기록하는 듯

    // Callbacks for response packet info & progress
    public ResponseInfoDelegate respInfoCB;
    public ProgressDelegate progCB;
    public DoneDelegate doneCB;

    private WebRequest _request;
    public virtual WebRequest request
    {
        get { return null; }
        set { _request = value; }
    }

    private WebResponse _response;
    public virtual WebResponse response
    {
        get { return null; }
        set { _response = value; }
    }

    public WebRequestState(int buffSize)
    {
        bytesRead = 0;
        bufferRead = new byte[buffSize];
        streamResponse = null;
    }
}

/// <summary>
/// State object for HTTP transfers
/// </summary>
public class HttpWebRequestState : WebRequestState
{
    private HttpWebRequest _request;
    public override WebRequest request
    {
        get
        {
            return _request;
        }
        set
        {
            _request = (HttpWebRequest)value;
        }
    }

    private HttpWebResponse _response;
    public override WebResponse response
    {
        get
        {
            return _response;
        }
        set
        {
            _response = (HttpWebResponse)value;
        }
    }

    public HttpWebRequestState(int buffSize) : base(buffSize) { }
}

/// <summary>
/// State object for FTP transfers
/// </summary>
public class FtpWebRequestState : WebRequestState
{
    private FtpWebRequest _request;
    public override WebRequest request
    {
        get
        {
            return _request;
        }
        set
        {
            _request = (FtpWebRequest)value;
        }
    }

    private FtpWebResponse _response;
    public override WebResponse response
    {
        get
        {
            return _response;
        }
        set
        {
            _response = (FtpWebResponse)value;
        }
    }

    public FtpWebRequestState(int buffSize) : base(buffSize) { }
}
