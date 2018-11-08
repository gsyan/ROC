﻿using UnityEngine;
using UnityEngine.SceneManagement;//SceneManager
using UnityEngine.Networking;
using System;//Exception
using System.IO;//MemoryStream
using System.Text;//Encoding
using System.Net;//WebExceptionStatus
using System.Collections;
using System.Collections.Generic;

//https://cdn.jsdelivr.net/gh/gsyan/ROCPatch@v0.0.6/patch/android_test/server_condition.json

public class Patch : MonoBehaviour
{
    private const string _applyPatchKey = "apply_patch_count";

    public enum URLType
    {
        Local,
        Dev,
        QA,
        Live,
        Custom,
    }
    public URLType urlType;
    public string address;//패치 서버 주소, "https://cdn.jsdelivr.net/gh/gsyan/ROCPatch"
    public bool bVersion;
    public string verion;//test용"@v0.0"
    public string patchVersion;//test용 "1"
    public string filePath;//"/patch/android_test/"
    public bool useAssetbundle;

    private UIRootPatch _patchUI;
    private string _patchURL = "";//패치 서버의 URL

    //패치 버전 숫자를 저장할 파일이 있는 경로
    private string _versionFilePath
    {
        get
        {
            return Application.persistentDataPath + "/version.data";
        }
    }
    
    //무엇에 쓰는 변수인지 정리
    private bool _updateDownloadProgress = false;
    private bool _updateExtractProgress = false;
    private int _currentValue = 0;
    private float _currentProgress = 0.0f;
    private int _totalValue = 0;

    
    IEnumerator Start()
    {
#if UNITY_EDITOR
        ResourceSystem.useAssetBundle = useAssetbundle;
#endif
        SetFilePath();

        _patchURL = address;
        
        //런처 텍스트를 로컬라이제이션에 로드
        Localization.LoadCSV(Resources.Load("Localization/launcher") as TextAsset, true);

        GameObject go = Instantiate(Resources.Load("UI/UI Root Patch")) as GameObject;
        if (go != null)
        {
            _patchUI = go.GetComponent<UIRootPatch>();
        }

        _patchUI.SetStateText("");

        yield return ScreenBlinder.Instance.BlinderFadeOut();

        StartCoroutine(DownloadServerConditionThisVersion());

        yield return 0;
    }
    private void SetFilePath()
    {
#if UNITY_ANDROID
#if GAMESERVER_ALL
        filePath = "/patch/android_test/";
#elif GAMESERVER_QA
        filePath = "/patch/android_qa/";
#elif GAMESERVER_LIVE
        filePath = "/patch/android_live/";
#endif
#elif UNITY_IOS
#if GAMESERVER_ALL
        filePath = "/patch/ios_test/";
#elif GAMESERVER_QA
        filePath = "/patch/ios_qa/";
#elif GAMESERVER_LIVE
        filePath = "/patch/ios_live/";
#endif
#endif
    }

    private void OnDestroy()
    {
        
    }

    private void Update()
    {
        if(_updateDownloadProgress)
        {
            _patchUI.UpdateDownloadProgress(_currentProgress, _totalValue);
            _updateDownloadProgress = false;
        }

        if(_updateExtractProgress)
        {
            _patchUI.UpdateExtractProgress(_currentValue, _totalValue);
            _updateExtractProgress = false;
        }
    }


    //ServerCondition 받고자, 버전과 패치 번호 등이 주요 이슈인듯
    IEnumerator DownloadServerConditionThisVersion()
    {
        _patchUI.SetStateText(Localization.Get("checking_version"));

        string path = "";
        if(bVersion)
        {
            _patchURL = _patchURL + verion + "." + patchVersion;
            //path = string.Format("{0}{1}.{2}{3}server_condition.json", _patchURL, verion, patchVersion, filePath);
        }
        path = string.Format("{0}{1}server_condition.json", _patchURL, filePath);

        DLog.LogMSG("path: " + path);

        UnityWebRequest www = UnityWebRequest.Get(path);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
            StartCoroutine(DownloadServerCondition());
            yield break;
        }
        
        //Debug.Log(www.downloadHandler.text);
        byte[] results = www.downloadHandler.data;
        OnDoneServerCondition(JsonUtility.FromJson<ServerCondition>(Encoding.UTF8.GetString(results)));
    }

    //최초 ServerCondition 정보 받아오기가 안되면 다시 시도 할지 UI보여주고 승락 시 다시 수행
    IEnumerator DownloadServerCondition()
    {
        string path = string.Format("{0}{1}server_condition.json", _patchURL, filePath);
        
        UnityWebRequest www = UnityWebRequest.Get(path);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Callback callback = delegate ()
            {
                StartCoroutine(DownloadServerCondition());
            };
            //ShowFailMessageBox(state.status, callback);
            Debug.Log("www.error");
            yield break;
        }

        byte[] results = www.downloadHandler.data;
        OnDoneServerCondition(JsonUtility.FromJson<ServerCondition>(Encoding.UTF8.GetString(results)));
    }

    //bk, ServerCondition 정보 받아왔으면, 이를 토대로 패치(ExtractPatchFile)를 받을지 다음 씬으로 넘길지 결정
    void OnDoneServerCondition(ServerCondition serverCondition)
    {
        try
        {
            // check application version
            int[] clientAppVersions = null;
            CheckClientAppVersion(ref clientAppVersions);

            //minAppVersions(서버에서받은 server_condition.json)
            int[] serverAppVersions = null;
            CheckServerAppVersion(serverCondition, ref serverAppVersions);

            // bk, clientAppVersions(unity project settings) 과 minAppVersions(서버에서받은 server_condition.json) 비교
            // 서버에서받은 데이터의 min_application_version 버전보다 낮으면 
            // 앱 스토어에 새 버전이 있음을 UI로 알리고, 유저가 확인하면 해당 마켓으로 리다이랙션 및 앱 종료,
            CheckUpdateApp(clientAppVersions, serverAppVersions, serverCondition);

            GInfo.serverType = serverCondition.server_type;
            GInfo.serverGroup = serverCondition.server_group;

            // check test app version || tester GAID  ==========================================================================
            bool isTester = CheckIsTestAppVersion(clientAppVersions, serverCondition);
            isTester = CheckIsTesterGAID(isTester, serverCondition);

            //check server inspection
            CheckServerInspection(isTester, serverCondition);

            //check patch version
            CheckPatchVersion(serverCondition);
        }
        catch (Exception e)
        {
            Debug.Log(e);

            Callback callback = delegate ()
            {
                Application.Quit();
            };

            _patchUI.ShowMessageBoxOK(Localization.Get(e.Message), callback, callback);
        }
    }
    private void CheckClientAppVersion(ref int[] clientAppVersions)
    {
        clientAppVersions = ConvertVersion(Application.version);
        if (clientAppVersions == null)
        {
            throw new Exception("invalid_client_app_version");
        }
    }
    private void CheckServerAppVersion(ServerCondition serverCondition, ref int[] serverAppVersions)
    {
        serverAppVersions = ConvertVersion(serverCondition.app_version);
        //DLog.LogMSG("PatchServer AppVersion: " + serverCondition.app_version);
        if (serverAppVersions == null)
        {
            throw new Exception("invalid_server_app_version");
        }
    }
    private void CheckUpdateApp(int[] clientAppVersions, int[] serverAppVersions, ServerCondition serverCondition)
    {
        //버전 체크
        for (int i = 0; i < 2; i++)
        {
            if (clientAppVersions[i] > serverAppVersions[i])//클라 버전이 패치서버 버전보다 높을 경우
            {
                break;//빌드 버전이 낮지 않음을 확인했으니 오케이 다음으로.
            }
            else if (clientAppVersions[i] < serverAppVersions[i])//클라 버전이 패치서버 버전 낮을 경우
            {
                Callback callback = delegate ()
                {
#if UNITY_ANDROID
#if BILLING_UNITY
                    Application.OpenURL(serverCondition.playstore_download_url);
#elif BILLING_NSTORE
                        Application.OpenURL(serverCondition.nstore_download_url);
#endif
#endif
                    Application.Quit();
                };

                // 앱 스토어에 새 버전이 있음을 UI로 알리고, 유저가 확인하면 해당 마켓으로 리다이랙션 및 앱 종료,
                _patchUI.ShowMessageBoxOK(Localization.Get("notice_new_application"), callback, callback);
                return;
            }
        }
    }
    private bool CheckIsTestAppVersion(int[] clientAppVersions, ServerCondition serverCondition)
    {
        if (!string.IsNullOrEmpty(serverCondition.tester_app_version))
        {
            int[] testerAppVersions = ConvertVersion(serverCondition.tester_app_version);
            //NativeBK.LogMSG("PatchServer testerAppVersions: " + serverCondition.tester_app_version);
            for (int i = 0; i < 2; i++)
            {
                if (clientAppVersions[i] < testerAppVersions[i])//주의 testerAppVersions 과 같은 버전의 클라이언트는 테스터가 됨
                {
                    return false;
                }
            }
            return true;
        }

        return false;
    }
    private bool CheckIsTesterGAID(bool isTester, ServerCondition serverCondition)
    {
        if (!isTester)
        {
            //NativeBK.SetGAID("e099125a-4097-4af9-af5b-0154cb92e4ad");//인위적으로 등록된 테스터의 광고아이디 삽입
            for (int i = 0; i < serverCondition.tester.Length; ++i)
            {
                if (string.Compare(serverCondition.tester[i], NativeBK.GAID) == 0)
                {
                    return true;
                }
            }
            return false;
        }
        return true;
    }
    private void CheckServerInspection(bool isTester, ServerCondition serverCondition)
    {
        if (!isTester && !serverCondition.is_opened)
        {
            if (serverCondition.is_regular)//정기점검
            {
                if (serverCondition.close_hour == -1)//close_hour 가 -1 이면 시간을 표기하지 않는다.
                {
                    throw new Exception("notice_inspect");
                }
                else
                {
                    throw new Exception("notice_regular_inspect");
                }

            }
            else//임시정검
            {
                if (serverCondition.close_hour == -1)
                {
                    throw new Exception("notice_inspect");
                }
                else
                {
                    throw new Exception("notice_temporary_inspect");
                }
            }
        }
    }
    int _serverPatchNumber = 0;
    private void CheckPatchVersion(ServerCondition serverCondition)
    {
#if !UNITY_EDITOR && !USE_ASSET_BUNDLE
        return;
#endif
        _serverPatchNumber = serverCondition.patch_number;
        int serverPatchNumberAll = RemakePatchNumber(serverCondition.app_version, _serverPatchNumber);

        int minServerPatchNumber = serverCondition.min_patch_number;
        int minServerPatchNumberAll = RemakePatchNumber(serverCondition.min_app_version, minServerPatchNumber);

        int clientVersionNumber = ReadFileValue(_versionFilePath, 0);
        
        // clientPatchVersion 이 0이 아니라면 패치를 받은 상태인것, 0이면 설치후 한번도 패치받지 않은것
        if (clientVersionNumber > 0)//한번이라도 패치받은 경우
        {
            if (clientVersionNumber < minServerPatchNumberAll)
            {
                // 최소 패치 버전보다 낮다면 1을 받는다.
                clientVersionNumber = RemakePatchNumber(Application.version, 1); ;
            }

            if (clientVersionNumber > serverPatchNumberAll)
            {
                // 클라이언트 패치 버전이 이상함으로 모든 패치 파일을 다시 받고 버전을 갱신한다.
                clientVersionNumber = 0;
            }
        }

        // 다운로드 해야 하는 상황
        if (clientVersionNumber < serverPatchNumberAll)
        {
            StartCoroutine(CheckFileSize(clientVersionNumber, serverPatchNumberAll));
        }
        else
        {
#if UNITY_EDITOR || USE_ASSET_BUNDLE
            GInfo.patchNumber = _serverPatchNumber;
#endif
            _patchUI.SetStateText(Localization.Get("lastest_version"));
            _patchUI.ClearProgress();

            // 로그인 씬으로 이동
            StartCoroutine(LoadingLevel());
        }
    }
    private int RemakePatchNumber(string appVersion, int patchNumber)
    {
        string[] s = appVersion.Split('.');
        int f1 = int.Parse(s[0]) * 1000000;
        int f2 = int.Parse(s[1]) * 1000;
        return f1 + f2 + patchNumber;
    }

    IEnumerator CheckFileSize(int clientPatchNumberAll, int serverPatchNumberAll)
    {
        Callback callbackExtractPatchFile = delegate ()
        { //패치가 있으니 받겠냐는 버튼에 오케이 하면 호출
            _patchUI.StartLoopImage();
            StartCoroutine(ExtractPatchFile(clientPatchNumberAll, serverPatchNumberAll));
        };

        Callback callbackQuit = delegate ()
        {
            Application.Quit();
        };

        string downloadUrl = string.Format("{0}{1}{2}/{3}.zip", _patchURL, filePath, serverPatchNumberAll, clientPatchNumberAll);
        Uri uri = new Uri(downloadUrl);
        using (UnityWebRequest uwr = UnityWebRequest.Get(uri))
        {
            uwr.method = UnityWebRequest.kHttpVerbHEAD;
            yield return uwr.SendWebRequest();
            
            //Debug.Log(uwr.GetResponseHeader("content-length"));//파일 크기
            //Debug.Log(uwr.downloadedBytes);//실제로 다운로드된 건 없다는 증거
            _totalValue = int.Parse(uwr.GetResponseHeader("Content-Length"));
            if(_totalValue > 0)
            {
                //패치 확인 창 x 버튼 클릭시 패치가 받아지는 문제로 인해 callback 에 null을 넣는 것으로 수정, bk
                _patchUI.ShowMessageBoxOK(string.Format(Localization.Get("notice_new_patch"), string.Format("({0:0.0} MB)", _totalValue / 1048576.0f)), callbackExtractPatchFile, null);
            }
            else
            {
                callbackQuit();
            }

        }

    }
    IEnumerator ExtractPatchFile(int clientPatchNumberAll, int serverPatchNumberAll)
    {
        _patchUI.SetStateText(Localization.Get("state_check_availability"));

        // 다운로드 완료된 파일이 있는지 체크
        string completeFilePath = string.Format("{0}/{1}_{2}", Application.persistentDataPath, clientPatchNumberAll, serverPatchNumberAll);
        if (File.Exists(completeFilePath))
        {
            // CRC 정보 읽기
            string crcUrl = string.Format("{0}{1}{2}/{3}.crc", _patchURL, filePath, serverPatchNumberAll, clientPatchNumberAll);
            
            using (UnityWebRequest uwr = UnityWebRequest.Get(crcUrl))
            {
                yield return uwr.SendWebRequest();

                if (uwr.isNetworkError || uwr.isHttpError)
                {
                    //Debug.Log("CRC server download error: " + uwr.error);
                    Callback callback = delegate ()
                    {
                        StartCoroutine(ExtractPatchFile(clientPatchNumberAll, serverPatchNumberAll));
                    };

                    ShowFailMessageBox(WebExceptionStatus.ConnectFailure, callback);
                    yield break;
                }
                else
                {
                    byte[] serverCrc = uwr.downloadHandler.data;
                    //using (var stream = new MemoryStream(serverCrc))
                    //using (var binaryStream = new BinaryReader(stream))
                    //{
                    //    Debug.Log(binaryStream);
                    //}
                    byte[] clientCrc = AssetBundleUtility.ComputeHash(completeFilePath);

                    if (Utility.ByteArrayCompare(serverCrc, clientCrc))
                    {
                        _patchUI.SetStateText(Localization.Get("state_apply_patch"));

#if UNITY_DEBUG
                        float start = Time.realtimeSinceStartup;
#endif
                        string assetBundleDir = Application.persistentDataPath + "/assetbundle";
                        yield return ZipUtility.Decompression(completeFilePath, assetBundleDir, PlayerPrefs.GetInt(_applyPatchKey, 0), ExtractProgress);
#if UNITY_DEBUG
                        DLog.LogMSG("Extract elapsedTime=" + (Time.realtimeSinceStartup - start));
#endif
                        PlayerPrefs.SetInt(_applyPatchKey, 0);

                        // 버전 업데이트
                        _patchUI.SetStateText(Localization.Get("state_remove_file"));
                        WriteFileValue(_versionFilePath, serverPatchNumberAll);

                        // 패치 파일 삭제
                        TryRemoveFile(completeFilePath);

                        // 필요 없어진 파일제거
                        string removeFilePath = string.Format("{0}/remove_{1}_{2}.txt", assetBundleDir, clientPatchNumberAll, serverPatchNumberAll);
                        if (File.Exists(removeFilePath))
                        {
                            try
                            {
                                string[] removeFiles = File.ReadAllLines(removeFilePath);
                                for (int i = 0; i < removeFiles.Length; ++i)
                                {
                                    TryRemoveFile(assetBundleDir + "/" + removeFiles[i]);
                                }
                            }
                            catch (Exception e)
                            {
                                DLog.LogMSG(e.ToString());
                            }

                            TryRemoveFile(removeFilePath);
                        }

                        GInfo.patchNumber = _serverPatchNumber;
                        _patchUI.SetStateText(Localization.Get("lastest_version"));


                        // 로그인 씬으로 이동
                        StartCoroutine(LoadingLevel());
                    }
                    else
                    {
                        TryRemoveFile(completeFilePath);

                        StartCoroutine(DownloadPatchFile(clientPatchNumberAll, serverPatchNumberAll));
                    }
                }
            }
        }
        else
        {
            StartCoroutine(DownloadPatchFile(clientPatchNumberAll, serverPatchNumberAll));
        }
    }
    IEnumerator DownloadPatchFile(int clientPatchNumberAll, int serverPatchNumberAll)
    {
        PlayerPrefs.SetInt(_applyPatchKey, 0);
        _patchUI.SetStateText(Localization.Get("state_download_patch"));
        //받을 파일의 이름
        string downloadFilePath = string.Format("{0}/{1}_{2}_", Application.persistentDataPath, clientPatchNumberAll, serverPatchNumberAll);
        //받을 곳 URL
        string downloadUrl = string.Format("{0}{1}{2}/{3}.zip", _patchURL, filePath, serverPatchNumberAll, clientPatchNumberAll);

#if UNITY_DEBUG
        float start = Time.realtimeSinceStartup;
#endif

        using (UnityWebRequest uwr = UnityWebRequest.Get(downloadUrl))
        {
            uwr.method = UnityWebRequest.kHttpVerbGET;
            var dh = new DownloadHandlerFile(downloadFilePath);
            dh.removeFileOnAbort = true;
            uwr.downloadHandler = dh;
            //yield return uwr.SendWebRequest();
            uwr.SendWebRequest();

            while (!uwr.isDone)
            {
                _updateDownloadProgress = true;
                _currentProgress = uwr.downloadProgress;
                Debug.Log("_currentProgress: " + _currentProgress);
                yield return null;
            }

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                //Debug.Log("Download saved to: " + downloadFilePath.Replace("/", "\\") + "\r\n" + uwr.error);
                Callback callback = delegate ()
                {
                    StartCoroutine(DownloadPatchFile(clientPatchNumberAll, serverPatchNumberAll));
                };

                ShowFailMessageBox(WebExceptionStatus.ConnectFailure, callback);
                yield break;
            }
        }

#if UNITY_DEBUG
        DLog.LogMSG("Download elapsedTime=" + (Time.realtimeSinceStartup - start));
#endif

        string completeFilePath = downloadFilePath.Remove(downloadFilePath.Length - 1);

        TryMoveFile(downloadFilePath, completeFilePath);
        StartCoroutine(ExtractPatchFile(clientPatchNumberAll, serverPatchNumberAll));
    }

    private void CheckFileSizeHTTP(int clientPatchNumberAll, int serverPatchNumberAll)
    {
        Callback callback = delegate ()
        {
            _patchUI.StartLoopImage();

            StartCoroutine(ExtractPatchFileHTTP(clientPatchNumberAll, serverPatchNumberAll));
        };


        HttpWebResponse response = null;
        string downloadUrl = string.Format("{0}{1}{2}/{3}.zip", _patchURL, filePath, serverPatchNumberAll, clientPatchNumberAll);
        try
        {
            Uri uri = new Uri(downloadUrl);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = "HEAD";
            response = (HttpWebResponse)(request.GetResponse());
        }
        catch (WebException e)
        {
            Debug.Log(string.Format("{0}{1}{2}/{3}.zip", downloadUrl, filePath, serverPatchNumberAll, clientPatchNumberAll) + " doesn't exist: " + e.Message);
        }

        if (response != null)
        {
            //패치 확인 창 x 버튼 클릭시 패치가 받아지는 문제로 인해 callback 에 null을 넣는 것으로 수정, bk
            //patchUI.ShowMessageBoxOK(string.Format(Localization.Get("notice_new_patch"), string.Format("({0:0.0} MB)", response.ContentLength / 1048576.0f)), callback, callback);
            _patchUI.ShowMessageBoxOK(string.Format(Localization.Get("notice_new_patch"), string.Format("({0:0.0} MB)", response.ContentLength / 1048576.0f)), callback, null);
        }
        else
        {
            _patchUI.ShowMessageBoxOK(string.Format(Localization.Get("notice_new_patch"), "", callback, callback));
        }
    }
    IEnumerator DownloadPatchFileHTTP(int clientPatchNumberAll, int serverPatchNumberAll)
    {
        PlayerPrefs.SetInt(_applyPatchKey, 0);
        _patchUI.SetStateText(Localization.Get("state_download_patch"));
        //받을 파일의 이름
        string downloadFilePath = string.Format("{0}/{1}_{2}_", Application.persistentDataPath, clientPatchNumberAll, serverPatchNumberAll);
        //받을 곳 URL
        string downloadUrl = string.Format("{0}{1}{2}/{3}.zip", _patchURL, filePath, serverPatchNumberAll, clientPatchNumberAll);

#if UNITY_DEBUG
        float start = Time.realtimeSinceStartup;
#endif
        using (FileStream stream = new FileStream(downloadFilePath, FileMode.OpenOrCreate, FileAccess.Write))
        {
            WaitForDownload state = HttpRequestDownloader.DownloadFile(downloadUrl, stream, DownloadProgress);
            yield return state;

            if (!state.isDone)
            {
                Callback callback = delegate ()
                {
                    StartCoroutine(DownloadPatchFile(clientPatchNumberAll, serverPatchNumberAll));
                };

                ShowFailMessageBox(state.status, callback);
                yield break;
            }
        }
#if UNITY_DEBUG
        DLog.LogMSG("Download elapsedTime=" + (Time.realtimeSinceStartup - start));
#endif

        string completeFilePath = downloadFilePath.Remove(downloadFilePath.Length - 1);

        TryMoveFile(downloadFilePath, completeFilePath);
        StartCoroutine(ExtractPatchFile(clientPatchNumberAll, serverPatchNumberAll));
    }
    IEnumerator ExtractPatchFileHTTP(int clientPatchNumberAll, int serverPatchNumberAll)
    {
        _patchUI.SetStateText(Localization.Get("state_check_availability"));

        // 다운로드 완료된 파일이 있는지 체크
        string completeFilePath = string.Format("{0}/{1}_{2}", Application.persistentDataPath, clientPatchNumberAll, serverPatchNumberAll);
        if (File.Exists(completeFilePath))
        {
            // CRC 정보 읽기
            string crcUrl = string.Format("{0}{1}{2}/{3}.crc", _patchURL, filePath, serverPatchNumberAll, clientPatchNumberAll);
            using (MemoryStream stream = new MemoryStream())
            {
                WaitForDownload state = HttpRequestDownloader.DownloadFile(crcUrl, stream);
                yield return state;

                if (!state.isDone)
                {
                    Callback callback = delegate ()
                    {
                        StartCoroutine(ExtractPatchFileHTTP(clientPatchNumberAll, serverPatchNumberAll));
                    };

                    ShowFailMessageBox(state.status, callback);
                    yield break;
                }

                byte[] serverCrc = stream.ToArray();
                byte[] clientCrc = AssetBundleUtility.ComputeHash(completeFilePath);

                if ( Utility.ByteArrayCompare(serverCrc, clientCrc) )
                {
                    _patchUI.SetStateText(Localization.Get("state_apply_patch"));

#if UNITY_DEBUG
                    float start = Time.realtimeSinceStartup;
#endif
                    string assetBundleDir = Application.persistentDataPath + "/assetbundle";
                    yield return ZipUtility.Decompression(completeFilePath, assetBundleDir, PlayerPrefs.GetInt(_applyPatchKey, 0), ExtractProgress);
#if UNITY_DEBUG
                    DLog.LogMSG("Extract elapsedTime=" + (Time.realtimeSinceStartup - start));
#endif
                    PlayerPrefs.SetInt(_applyPatchKey, 0);

                    // 버전 업데이트
                    _patchUI.SetStateText(Localization.Get("state_remove_file"));
                    WriteFileValue(_versionFilePath, serverPatchNumberAll);

                    // 패치 파일 삭제
                    TryRemoveFile(completeFilePath);

                    // 필요 없어진 파일제거
                    string removeFilePath = string.Format("{0}/remove_{1}_{2}.txt", assetBundleDir, clientPatchNumberAll, serverPatchNumberAll);
                    if(File.Exists(removeFilePath))
                    {
                        try
                        {
                            string[] removeFiles = File.ReadAllLines(removeFilePath);
                            for ( int i = 0; i < removeFiles.Length; ++i )
                            {
                                TryRemoveFile(assetBundleDir + "/" + removeFiles[i]);
                            }
                        }
                        catch(Exception e)
                        {
                            DLog.LogMSG(e.ToString());
                        }

                        TryRemoveFile(removeFilePath);
                    }

                    GInfo.patchNumber = _serverPatchNumber;
                    _patchUI.SetStateText(Localization.Get("lastest_version"));


                    // 로그인 씬으로 이동
                    StartCoroutine(LoadingLevel());
                }
                else
                {
                    TryRemoveFile(completeFilePath);

                    StartCoroutine(DownloadPatchFileHTTP(clientPatchNumberAll, serverPatchNumberAll));
                }
            }
        }
        else
        {
            StartCoroutine(DownloadPatchFileHTTP(clientPatchNumberAll, serverPatchNumberAll));
        }
    }

    private int ReadFileValue(string path, int defaultValue)
    {
        try
        {
            if (File.Exists(path))
            {
                int value = 0;
                if (int.TryParse(File.ReadAllText(path), out value))
                {
                    return value;
                }
            }
        }
        catch(Exception e)
        {
            DLog.LogMSG(e.ToString());
        }
        return defaultValue;
    }

    void DownloadProgress(int bytesRead, int totalBytes)
    {
        _currentValue = bytesRead;
        _totalValue = totalBytes;
        _updateDownloadProgress = true;
    }

    void ExtractProgress(int current, int total, string fileName)
    {
        _currentValue = current + 1;
        _totalValue = total;
        _updateExtractProgress = true;

        PlayerPrefs.SetInt(_applyPatchKey, current);
    }
    
    void ShowFailMessageBox(WebExceptionStatus status, Callback onOK)
    {
        switch (status)
        {
            case WebExceptionStatus.ConnectFailure:
                _patchUI.ShowMessageBoxYesNo(Localization.Get("notice_error_connect_failure"), onOK, FailedUpdate, FailedUpdate);
                break;

            case WebExceptionStatus.ProtocolError:
                _patchUI.ShowMessageBoxYesNo(Localization.Get("notice_error_not_found"), onOK, FailedUpdate, FailedUpdate);
                break;

            default:
                _patchUI.ShowMessageBoxYesNo(Localization.Get("notice_error_default"), onOK, FailedUpdate, FailedUpdate);
                break;
        }
    }

    void FailedUpdate()
    {
        _patchUI.SetStateText(Localization.Get("failed_update"));
    }
    

    void WriteFileValue(string path, int value)
    {
        try
        {
            File.WriteAllText(path, value.ToString());
        }
        catch (Exception e)
        {
            DLog.LogMSG(e.ToString());
        }
    }

    bool TryMoveFile(string sourceFileName, string destFileName)
    {
        try
        {
            File.Move(sourceFileName, destFileName);
        }
        catch (Exception e)
        {
            DLog.LogMSG(e.ToString());

            return false;
        }

        return true;
    }

    bool TryRemoveFile(string path)
    {
        try
        {
            File.Delete(path);
        }
        catch (Exception e)
        {
            DLog.LogMSG(e.ToString());

            return false;
        }

        return true;
    }

    int[] ConvertVersion(string version)
    {
        string[] values = version.Trim().Split('.');
        if (values.Length != 2)
        {
            return null;
        }

        int[] versions = new int[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            if (!int.TryParse(values[i], out versions[i]))
            {
                return null;
            }
        }

        return versions;
    }


    IEnumerator LoadingLevel()
    {
        yield return new WaitForSeconds(1.0f);
        yield return ScreenBlinder.Instance.BlinderFadeIn();
        
        
        BKST.UISystem.Instance.RemoveAll();//bk uisystem 오브젝트까지 삭제, 이후 리소스 리셋 완료 하고 나서 ui 생성
        yield return null;

        AssetBundleManager.Release();
        yield return null;

        ResourceSystem.UnloadUnusedReference();
        yield return null;

        Resources.UnloadUnusedAssets();
        yield return null;
        
        SceneLoad.nextScene = "Login";
        SceneManager.LoadScene("Loading");
    }

    
    
}