using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;
using System;
using System.IO;
using System.Text;

public partial class BuildTool : EditorWindow
{
	private void DrawPatchGUI()
    {
        GUILayout.Space(10);

        // MIN APPLICATION VERSION
        string minApplicationVersion = PlayerPrefs.GetString("min_application_version", Application.version);
        minApplicationVersion = EditorGUILayout.TextField("Min Application Version", minApplicationVersion);
        PlayerPrefs.SetString("min_application_version", minApplicationVersion);

        // MIN PATCH VERSION
        int minPatchVersion = PlayerPrefs.GetInt("min_patch_version", 1);
        minPatchVersion = EditorGUILayout.IntField("Min Patch Version", minPatchVersion);
        PlayerPrefs.SetInt("min_patch_version", minPatchVersion);
        
        // PATCH VERSION
        int patchVersion = PlayerPrefs.GetInt("patch_version", 1);
        patchVersion = EditorGUILayout.IntField("Patch Version", patchVersion);
        PlayerPrefs.SetInt("patch_version", patchVersion);

        // BUTTON BUILD
        if (GUILayout.Button("Build AssetBundle"))
        {
            Build(patchVersion);
        }

        // BUTTON PATCH
        if (GUILayout.Button("Make Patch"))
        {
            MakePatch(minApplicationVersion, minPatchVersion, patchVersion);
        }
        
    }

    void Build(int patchVersion)
    {
        try
        {
            string assetbundleDir = _rootDir + "/android/assetbundle";
            string manifestDir = _rootDir + "/android/manifest";
            string manifestPath = string.Format("{0}/{1}.manifest", manifestDir, patchVersion);
            
            List<string> resourcesSelected = new List<string>();//리소스 폴더에 내에, ".meta", ".cs", ".js", ".xlsm", ".shader" 확장자명이 아니고, csv 폴더에 있지 않은 것들
            List<string> includeFiles = new List<string>();//dependency 파일 중 제외확장자 제외, launcher resource 폴더 내 파일 제외, 그 외 모두 포함
            List<string> excludeFiles = new List<string>();//에셋번들 제작에서 제외 되는 파일명을 모아두는 곳
            List<string> unusedAssets = new List<string>();

            int fCount = 0;//for문 count

            // check patch version  =================================================================================================
            if (File.Exists(manifestPath))
            {
                if(!EditorUtility.DisplayDialog("Build", "Same version manifest exists already.\n" + manifestPath + "\n\nContinue to build", "OK", "CANCEL"))
                {
                    return;
                }
            }

            // 1.Make csv.bytes in Resources folder =================================================================================================
            if (EditorUtility.DisplayCancelableProgressBar("Build", "Make csv.bytes in Resources folder ...", 0.1f))
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            string[] csvFiles = Directory.GetFiles(CSV_DIR, "*.csv", SearchOption.AllDirectories);
            fCount = csvFiles.Length;
            for (int i = 0; i < fCount; ++i)
            {
                csvFiles[i] = csvFiles[i].Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToLower();// \\을 /로
                csvFiles[i] = csvFiles[i].Replace(CSV_DIR.ToLower() + "/", "");//경로 지우고 파일 이름만
            }

            if (!ZipUtility.Compression(CSV_ZIP_PATH, CSV_DIR, csvFiles))//CSV_DIR 경로에 있는 csvFiles 파일들의 단일 압축파일을 CSV_ZIP_PATH 에 만든다.
            {
                throw new Exception("CSV packing failed");
            }

            if (!EncryptFile(CSV_ZIP_PATH, CSV_PACK_PATH))//CSV_PACK_PATH 에 csv.bytes 파일을 만든다.
            {
                throw new Exception("CSV encryption failed");
            }

            File.Delete(CSV_ZIP_PATH);//CSV_ZIP_PATH 에 만들어진 압축파일을 지운다..
            AssetDatabase.Refresh();


            // 2.Make lua.bytes in Resources folder =================================================================================================
            if (EditorUtility.DisplayCancelableProgressBar("Build", "Make lua.bytes in Resources folder ...", 0.2f))
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            string[] luaFiles = Directory.GetFiles(LUA_DIR, "*.lua", SearchOption.AllDirectories);
            fCount = luaFiles.Length;
            for (int i = 0; i < fCount; ++i)
            {
                luaFiles[i] = luaFiles[i].Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToLower();
                luaFiles[i] = luaFiles[i].Replace(LUA_DIR.ToLower() + "/", "");
            }

            if (!ZipUtility.Compression(LUA_ZIP_PATH, LUA_DIR, luaFiles))
            {
                throw new Exception("lua packing failed");
            }

            if (!EncryptFile(LUA_ZIP_PATH, LUA_PACK_PATH))
            {
                throw new Exception("lua encryption failed");
            }

            File.Delete(LUA_ZIP_PATH);
            AssetDatabase.Refresh();

            // 3.Make resourcesSelected with resources in Resources folder ==================================================================================
            if ( EditorUtility.DisplayCancelableProgressBar("Build", "Make resourcesSelected with resources in Resources folder ...", 0.3f))
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            //리소스 폴더의 모든 파일이 대상
            //제외 extension OR csv 이면 excludeFiles 에 add, 아니면 resourcesSelected 에 add (csv의 경우 csv.bytes로 만들어 두었으니 제외)
            string[] resourceFiles = Directory.GetFiles(RESOURCE_DIR, "*.*", SearchOption.AllDirectories);
            fCount = resourceFiles.Length;
            for (int i = 0; i < fCount; ++i)
            {
                string path = resourceFiles[i].Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToLower();
                if( IsExcludeExtension(path))//제외 extension
                {
                    excludeFiles.Add(path);//제외 대상에 포함
                }
                else
                {
                    if( Path.GetDirectoryName(path).Contains(CSV_DIR.ToLower()))//csv 이면(csv 는 resources 안에 있지만 먼저 처리 되었기 때문에 빼주자)
                    {
                        excludeFiles.Add(path);//제외 대상에 포함
                    }
                    else
                    {
                        resourcesSelected.Add(path);
                    }
                }
            }

            // 4. buildFiles key setting ==================================================================================
            if (EditorUtility.DisplayCancelableProgressBar("Build", "buildFiles key setting ...", 0.4f))
            {
                EditorUtility.ClearProgressBar();
                return;
            }
            //resourcesSelected 의 값들을 AssetBundle 작명법으로 수정하여, buildFiles의 key 값으로 세팅 & key 값 중복 제거
            Dictionary<string, List<string>> buildFiles = new Dictionary<string, List<string>>();
            fCount = resourcesSelected.Count;
            for ( int i = 0; i < fCount; ++i )
            {
                string key = GetAssetBundleName(resourcesSelected[i]);
                if (buildFiles.ContainsKey(key))
                {
                    throw new Exception("Duplicated file exist in Resource folder.\n" + AssetBundleUtility.GetPathWithoutExtension(resourcesSelected[i]));
                }
                else
                {
                    buildFiles.Add(key, new List<string>());//아직 buildFiles 의 key(에셋번들 작명법) 값만 정했지 내용은 비어있다.
                }
            }

            // 5. Make and add includeFiles & excludeFiles with dependency files ==================================================================================
            if (EditorUtility.DisplayCancelableProgressBar("Build", "Make and add includeFiles & excludeFiles with dependency files ...", 0.5f))
            {
                EditorUtility.ClearProgressBar();
                return;
            }
            //종속 에셋을 추려낸다. Resources 폴더 내부 + 외부 파일들
            //제외 extension과 launcher에 포함되는 리소스는 excludeFiles에 추가
            //그 외의 것은 includeFiles 에 추가.( 여기서 includeFiles 첫 등장 )
            string[] dependencyFiles = AssetDatabase.GetDependencies(resourcesSelected.ToArray());
            fCount = dependencyFiles.Length;
            for ( int i = 0; i< fCount; ++i )
            {
                string path = dependencyFiles[i].ToLower();

                if ( IsExcludeExtension(path) )//제외 확장장 거른다.
                {
                    excludeFiles.Add(path);
                }
                else
                {
                    // exclude files in launcher resource
                    if ( Path.GetDirectoryName(path).Contains(LAUNCHER_RESOURCE_DIR.ToLower()))//런처의 내용은 빌드에 포함 그러니 리소스에서는  제외
                    {
                        excludeFiles.Add(path);
                    }
                    else
                    {
                        includeFiles.Add(path);//소문자 path 형식
                    }
                }
            }

            // log includefiles
            includeFiles.Sort();
            //Logger.Write(includeFiles.ToArray(), "[include] ");

            // log excludefiles
            excludeFiles.Sort();
            //Logger.Write(excludeFiles.ToArray(), "[exclude] ");

            // 6. Add assetPaths to buildFiles from previous AssetBundle and includeFiles
            if (EditorUtility.DisplayCancelableProgressBar("Build", "Add assetPaths to buildFiles from previous AssetBundle and includeFiles ...", 0.6f))
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            // customAssetBundleNames 을 가져와
            string[] customAssetBundleNames = AssetDatabase.GetAllAssetBundleNames();
            fCount = customAssetBundleNames.Length;
            for(int i = 0; i< fCount; ++i)
            {
                //customAssetBundleNames 에 속한 파일 path를 가져와
                string[] paths = AssetDatabase.GetAssetPathsFromAssetBundle(customAssetBundleNames[i]);
                if ( paths.Length > 0)
                {
                    List<string> assetNameList = new List<string>();
                    for( int j=0; j< paths.Length; ++j)
                    {
                        //includeFiles 에 path가 있으면 지우고 buildFiles 에 path를 소유한 customAssetBundleNames을 추가한다.
                        string assetName = paths[j].ToLower();
                        if ( includeFiles.Remove(assetName))
                        {
                            assetNameList.Add(assetName);
                        }
                        else
                        {
                            unusedAssets.Add(assetName);
                        }
                    }
                    buildFiles.Add(customAssetBundleNames[i], assetNameList);
                }
            }
            // 커스텀 에셋번들이름을 차감한 includeFiles 를 바탕으로
            // buildFiles 내용 추가
            fCount = includeFiles.Count;
            for ( int i=0; i< fCount; ++i)
            {
                string key = GetAssetBundleName(includeFiles[i]);
                List<string> list;
                if (buildFiles.TryGetValue(key, out list))
                {
                    list.Add(includeFiles[i]);
                }
                else
                {
                    list = new List<string>();
                    list.Add(includeFiles[i]);

                    buildFiles.Add(key, list);
                }
            }

            // 7.Make AssetBundleBuild ==================================================================================
            if( EditorUtility.DisplayCancelableProgressBar("Build", "Make AssetBundleBuild...", 0.7f) )
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            int index = 0;
            AssetBundleBuild[] bundleBuilds = new AssetBundleBuild[buildFiles.Count];
            foreach( string key in buildFiles.Keys )
            {
                string[] assetNames = buildFiles[key].ToArray();
                bundleBuilds[index].assetBundleName = key;
                bundleBuilds[index].assetNames = assetNames;

                if( assetNames.Length > 1 )
                {
                    //Logger.Write(assetNames, "[combine] ");
                }

                index++;
            }

            // unusedAsset 목록 로그 남기기
            if ( unusedAssets.Count > 0 )
            {
                //Logger.Write(unusedAssets.ToArray(), "[unused] ");
            }

            // 빌드 폴더 없으면 생성
            if (!Directory.Exists(assetbundleDir))
            {
                Directory.CreateDirectory(assetbundleDir);
            }

            BuildPipeline.BuildAssetBundles(assetbundleDir, bundleBuilds, 
                BuildAssetBundleOptions.UncompressedAssetBundle | BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ForceRebuildAssetBundle,
                BuildTarget.Android //빌드 타겟을 안드로이드로 맞추는 부분, IOS도 대응 시켜야 함
            );
            
            // copy manifest
            string assetbundleManifestPath = assetbundleDir + "/assetbundle";
            if(File.Exists(assetbundleManifestPath))
            {
                if(!Directory.Exists(manifestDir))
                {
                    Directory.CreateDirectory(manifestDir);
                }

                File.Copy(assetbundleManifestPath, manifestPath, true);
            }
            else
            {
                throw new Exception("Can't Find Manifest File\n\n" + assetbundleManifestPath);
            }

            // 에셋번들 미디어 파일을 원본 파일로 바꿔치기한다.
            if( EditorUtility.DisplayCancelableProgressBar("Build", "media file copying ...", 0.9f) )
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            if( !Directory.Exists(RESOURCE_MEDIA_DIR) )
            {
                Directory.CreateDirectory(RESOURCE_MEDIA_DIR);
            }
            string[] mediaFiles = Directory.GetFiles(RESOURCE_MEDIA_DIR, "*.bytes", SearchOption.AllDirectories);
            fCount = mediaFiles.Length;
            for(int i=0; i<fCount; ++i)
            {
                string path = mediaFiles[i].Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                File.Copy(path, assetbundleDir + "/" + AssetBundleManager.GetAssetBundleName("Media/" + Path.GetFileNameWithoutExtension(path)), true);
            }

            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("Build", "Completed", "Confirm");
        }
        catch (Exception e)
        {
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("Build Failed", e.Message, "Confirm");
        }
        finally
        {
            AssetDatabase.DeleteAsset(CSV_ZIP_PATH);
            AssetDatabase.DeleteAsset(CSV_PACK_PATH);//CSV_PACK_PATH 에 만들어 둔 csv.bytes 파일을 지운다.
            AssetDatabase.DeleteAsset(LUA_ZIP_PATH);
            AssetDatabase.DeleteAsset(LUA_PACK_PATH);
            AssetDatabase.Refresh();
        }
    }

    void MakePatch(string minApplicationVersion, int minPatchVersion, int targetVersion)
    {
        try
        {
            //Logger.Initialize(string.Format("{0}/android/log/patch_{1}.txt", _rootDir, DateTime.Now.ToString("yyyyMMddHHmmss")));

            string androidDir = _rootDir + "/android";
            string assetbundleDir = androidDir + "/assetbundle";
            string assetbundleManifestPath = assetbundleDir + "/assetbundle";
            string patchDir = androidDir + "/patch/" + targetVersion;
            string manifestDir = androidDir + "/manifest";

            if( targetVersion != 1 && minPatchVersion > targetVersion )
            {
                throw new Exception("targetVersion smaller than minPatchVersion");
            }

            // check manifest that need
            for (int i = minPatchVersion; i< targetVersion; ++i)
            {
                string filePath = string.Format("{0}/{1}.manifest", manifestDir, i);
                if (!File.Exists(filePath))
                {
                    throw new Exception("Can't Fine Manifest File.\n\n" + filePath);
                }
            }

            // 현재 빌드된 버전의 매니페스트 파일과 캐싱된 매니페스트 파일이 일치하는지 확인한다.
            if (!CompareFileByte(assetbundleManifestPath, string.Format("{0}/{1}.manifest", manifestDir, targetVersion)))
            {
                throw new Exception("빌드된 메니페스트와 캐싱된 메니페스트 파일이 일치하지 않습니다.");
            }

            //check Directory exist
            if( Directory.Exists(patchDir) )
            {
                if (EditorUtility.DisplayDialog("Patch", "Same version Patch folder exist.\n Delete and continue to Remake?\n\n" + patchDir, "OK", "CANCEL"))
                {
                    Directory.Delete(patchDir, true);
                }
                else
                {
                    return;
                }
            }

            // android/assetbundle 에 있는 assetbundle 라는 이름의 manifest 파일을 읽는다.
            AssetBundleManifest assetbundleManifest = ReadManifest(assetbundleManifestPath);
            string[] assetbundleManifestAllFiles = assetbundleManifest.GetAllAssetBundles();

            if (EditorUtility.DisplayCancelableProgressBar("Patch", "Log writing ...", 0.1f))
            {
                EditorUtility.ClearProgressBar();
                return;
            }

            //Logger.Write(string.Format("START MAKE PATCH {0} to {1}", 0, targetVersion));
            //Logger.Write(assetbundleManifestAllFiles, "[+] ");
            //Logger.Write("END MAKE PATCH");
            //Logger.WriteSpace();

            //Make Full version zip and CRC
            List<string> assetBundleList = new List<string>();
            assetBundleList.Add("assetbundle");
            assetBundleList.AddRange(assetbundleManifestAllFiles);
            MakeZip(patchDir + "/0.zip", assetbundleDir, assetBundleList.ToArray());

            //Make patch version zip and CRC
            List<string> addList = new List<string>();
            List<string> removeList = new List<string>();
            List<string> modifyList = new List<string>();

            List<int> versionList = new List<int>();
            if(minPatchVersion > 1)
            {
                versionList.Add(1);
            }

            for(int i = minPatchVersion; i< targetVersion; ++i)
            {
                versionList.Add(i);
            }

            for (int i = 0; i < versionList.Count; ++i)
            {
                int oldVersion = versionList[i];
                addList.Clear();
                removeList.Clear();
                modifyList.Clear();
                assetBundleList.Clear();

                // android/manifest 안의 oldVersion의 *.manifest 파일 내용을 읽어들임
                AssetBundleManifest manifestManifest = ReadManifest(string.Format("{0}/{1}.manifest", manifestDir, oldVersion));
                string[] manifestManifestAllFiles = manifestManifest.GetAllAssetBundles();
                for(int j =0; j < assetbundleManifestAllFiles.Length; ++j)
                {
                    // 최신 버전인 assetbundleManifestAllFiles 내용을 예전 버전인 manifestManifestAllFiles 과 비교
                    // manifestManifestAllFiles 에 assetbundleManifestAllFiles 내용이 있고, 같다면 통과
                    // 다르다면 modifyList 에 추가
                    // 없다면 addList 에 추가
                    if ( StringArrayContainString(manifestManifestAllFiles, assetbundleManifestAllFiles[j]) )
                    {
                        //not same
                        string s1 = manifestManifest.GetAssetBundleHash(assetbundleManifestAllFiles[j]).ToString();
                        string s2 = assetbundleManifest.GetAssetBundleHash(assetbundleManifestAllFiles[j]).ToString();
                        if ( string.Compare(s1, s2) != 0 )
                        {
                            modifyList.Add(assetbundleManifestAllFiles[j]);//있는데 내용이 다르면 수정사항으로 추가
                        }
                    }
                    else
                    {
                        addList.Add(assetbundleManifestAllFiles[j]);//없으면 새로운 항목으로 추가
                    }
                }

                // 삭제된 파일
                for(int j=0; j< manifestManifestAllFiles.Length; ++j)
                {
                    if(!StringArrayContainString(assetbundleManifestAllFiles, manifestManifestAllFiles[j]))
                    {
                        removeList.Add(manifestManifestAllFiles[j]);
                    }
                }

                //Logger.WriteSpace();
                //Logger.Write(string.Format("START MAKE PATCH {0} TO {1}", oldVersion, targetVersion));
                //Logger.Write(addList.ToArray(), "[+] ");
                //Logger.Write(modifyList.ToArray(), "[!] ");
                //Logger.Write(removeList.ToArray(), "[-] ");
                //Logger.Write("END MAKE PATCH");
                //Logger.WriteSpace();

                assetBundleList.Add("assetbundle");
                assetBundleList.AddRange(addList);
                assetBundleList.AddRange(modifyList);

                string removeFileName = string.Format("remove_{0}_{1}.txt", oldVersion, targetVersion);
                string removeFilePath = assetbundleDir + "/" + removeFileName;
                if(removeList.Count > 0)
                {
                    File.WriteAllLines(removeFilePath, removeList.ToArray());
                    assetBundleList.Add(removeFileName);
                }

                MakeZip(string.Format("{0}/{1}.zip", patchDir, oldVersion), assetbundleDir, assetBundleList.ToArray());

                if( File.Exists(removeFilePath) )
                {
                    File.Delete(removeFilePath);
                }


            }

            // write server condition
            ServerCondition serverCondition = new ServerCondition();
            serverCondition.is_opened = true;
            serverCondition.is_regular = true;
            serverCondition.close_hour = 0;
            serverCondition.close_minute = 0;
            serverCondition.open_hour = 0;
            serverCondition.open_minute = 0;
            serverCondition.min_application_version = minApplicationVersion;
            serverCondition.min_patch_version = minPatchVersion;
            serverCondition.patch_version = targetVersion;
            serverCondition.application_download_url = "";
            serverCondition.nstore_download_url = "";
            serverCondition.playstore_download_url = "market://details?id=com.bkst.roc";
            serverCondition.onestore_download_url = "";//http://onesto.re/0000723853
            serverCondition.tester_app_version = "";

            //register tester by somthing infomation
            for (int i = 0; i < serverCondition.tester.Length; ++i)
            {
                serverCondition.tester[i] = "";
            }
            
            File.WriteAllText(androidDir + "/server_condition.json", JsonUtility.ToJson(serverCondition, true));

            // write cdn files
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("server_condition.json");
            sb.AppendLine(string.Format("server_condition_{0}.json", Application.version));
            sb.AppendLine(string.Format("android/{0}/0.crc", targetVersion));
            sb.AppendLine(string.Format("android/{0}/0.zip", targetVersion));

            for (int i = 0; i < versionList.Count; i++)
            {
                sb.AppendLine(string.Format("android/{0}/{1}.crc", targetVersion, versionList[i]));
                sb.AppendLine(string.Format("android/{0}/{1}.zip", targetVersion, versionList[i]));
            }

            File.WriteAllText(androidDir + "/update_files.txt", sb.ToString());
            
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("Patch", "Success", "Confirm");

        }
        catch (Exception e)
        {
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("Patch Failed", e.Message, "Confirm");
        }
    }



    
    private void MakeZip(string zipPath, string baseDir, string[] assetBundles)
    {
        if (!ZipUtility.Compression(zipPath, baseDir, assetBundles, CompressionProgress))
        {
            throw new Exception("Failed to Compress files.");
        }

        EditorUtility.DisplayProgressBar("Build", "CRC being Created..", 1.0f);
        byte[] hash = AssetBundleUtility.ComputeHash(zipPath);
        if(hash != null)
        {
            File.WriteAllBytes(Path.ChangeExtension(zipPath, ".crc"), hash);
        }

    }
    private bool CompressionProgress(string zipName, int current, int total)
    {
        return EditorUtility.DisplayCancelableProgressBar("Making Patch", string.Format("{0} Compressing Files ... {1} / {2}", Path.GetFileName(zipName), current, total), (float)current / (float)total);
    }



    private AssetBundleManifest ReadManifest(string path)
    {
        AssetBundle bundle = AssetBundle.LoadFromFile(path);
        AssetBundleManifest manifest = bundle.LoadAsset<AssetBundleManifest>("assetbundlemanifest");
        bundle.Unload(false);
        return manifest;
    }




    private bool EncryptFile(string originPath, string outputPath)
    {
        try
        {
            byte[] origin = File.ReadAllBytes(originPath);
            byte[] encrypted = Cryptography.EncryptBytes(origin, "+A899GRS6uVTBkLUIO0Jtbfw318p0P18ZrvINAyXSaE=", "4JXSJAJD71whV9G/eeRcJA==");

            File.WriteAllBytes(outputPath, encrypted);
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
            return false;
        }

        return true;
    }

    //두 파일의 내용이 같은지 판단한다. (assetbundle의 매니페스트)
    private bool CompareFileByte(string pathA, string pathB)
    {
        byte[] pathAByte = AssetBundleUtility.ComputeHash(pathA);
        byte[] pathBByte = AssetBundleUtility.ComputeHash(pathB);

        return ByteArrayCompare(pathAByte, pathBByte);
    }
    //두 byte 배열의 내용이 같은지 판단한다.
    private bool ByteArrayCompare(byte[] ba1, byte[] ba2)
    {
        if (ba1.Length != ba2.Length)
        {
            return false;
        }

        int fCount = ba1.Length;
        for (int i = 0; i < fCount; ++i)
        {
            if (ba1[i] != ba2[i])
            {
                return false;
            }
        }

        return true;
    }

    private bool StringArrayContainString(string[] stringArray, string findingString)
    {
        int fCount = stringArray.Length;
        for (int i = 0; i < fCount; ++i)
        {
            if (string.Compare(stringArray[i], findingString) == 0)
            {
                return true;
            }
        }
        return false;
    }

}
