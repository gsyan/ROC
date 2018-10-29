using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Security.Cryptography;
using System.Text;
using System.IO;

public class CachingTexture
{
    private static CachingTexture _instance;
    public static CachingTexture Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new CachingTexture();
            }
            return _instance;
        }
    }

    private string directoryPath
    {
        get
        {
            return Application.persistentDataPath + "/Textures";
        }
    }

    private SHA1 _sha = new SHA1CryptoServiceProvider();
    private StringBuilder _stringBuilder = new StringBuilder();
    private Dictionary<string, Texture2D> _textureDic = new Dictionary<string, Texture2D>();

    public void AddTex2D(string url, Texture2D tex2D)
    {
        string hashString = GetHash(url);
        if( !_textureDic.ContainsKey(hashString) )
        {
            if(!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            _textureDic.Add(hashString, tex2D);
            File.WriteAllBytes(directoryPath + "/" + hashString, tex2D.EncodeToPNG());
        }
    }
    public Texture2D GetTex2D(string url)
    {
        Texture2D tex2D = null;
        string hash = GetHash(url);

        if( !_textureDic.TryGetValue(hash, out tex2D))//_textureDic 에 없으면
        {
            if( !Directory.Exists(directoryPath) )
            {
                Directory.CreateDirectory(directoryPath);
            }

            string filePath = directoryPath + "/" + hash;
            if( File.Exists(filePath) )
            {
                System.DateTime lastWriteTime = File.GetLastWriteTime(filePath);
                if( (System.DateTime.Now - lastWriteTime).Days > 30 )
                {
                    File.Delete(filePath);//지우고 끝이네, 파일을 다운로드 받는 다든지, 저장한다든지는 다른 곳에서 하는 듯
                }
                else
                {
                    tex2D = new Texture2D(1, 1);
                    tex2D.LoadImage(File.ReadAllBytes(filePath));
                    _textureDic.Add(hash, tex2D);
                }
            }
        }
        return tex2D;
    }





    public string GetHash(string url)
    {
        //스트링 빌더 다 지운다
        if ( _stringBuilder.Length > 0)
        {
            _stringBuilder.Remove(0, _stringBuilder.Length);
        }

        byte[] bytes = _sha.ComputeHash(Encoding.UTF8.GetBytes(url));
        for(int i=0; i<bytes.Length; ++i)
        {
            _stringBuilder.Append(bytes[i]);
        }
        return _stringBuilder.ToString();//hash 코드를 문자열로 리턴
    }



    
}
