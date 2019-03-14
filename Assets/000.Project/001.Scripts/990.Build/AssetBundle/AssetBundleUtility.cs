using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Security.Cryptography;
using System.IO;


public static class AssetBundleUtility
{
    public static string GetPathWithoutExtension(string path)
    {
        string dirName = Path.GetDirectoryName(path);
        if(dirName != "")
        {
            return dirName + "/" + Path.GetFileNameWithoutExtension(path);
        }
        else
        {
            return Path.GetFileNameWithoutExtension(path);
        }
    }

    public static byte[] ComputeHash(string path)
    {
        using (var md5 = MD5.Create())
        {
            using (var stream = File.OpenRead(path))
            {
                return md5.ComputeHash(stream);
            }
        }
    }




}
