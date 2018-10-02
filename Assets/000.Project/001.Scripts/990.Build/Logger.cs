using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System;

public static class Logger {

    private static string _filePath;
    public static string filePath
    {
        get
        {
            return _filePath;
        }
    }

    public static void Initialize(string path)
    {
        _filePath = path;
    }

    public static void Write(string path, string msg)
    {
        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine("{0} {1}: {2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), msg);
            sw.Flush();
        }
    }

    public static void Write(string path, string[] list, string prefix = "", string suffix = "")
    {
        using (StreamWriter sw = File.AppendText(path))
        {
            string date = DateTime.Now.ToShortDateString();
            string time = DateTime.Now.ToShortTimeString();

            int fCount = list.Length;
            for (int i = 0; i < fCount; i++)
            {
                sw.WriteLine("{0} {1}: {2}{3}{4}", date, time, prefix, list[i], suffix);
            }

            sw.Flush();
        }
    }

    public static void Write(string msg)
    {
        CreateDirectory(_filePath);

        using (StreamWriter sw = File.AppendText(_filePath))
        {
            sw.WriteLine("{0} {1}: {2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString(), msg);
            sw.Flush();
        }
    }
    
    public static void Write(string[] list, string prefix = "", string suffix = "")
    {
        CreateDirectory(_filePath);

        using (StreamWriter sw = File.AppendText(_filePath))
        {
            string date = DateTime.Now.ToShortDateString();
            string time = DateTime.Now.ToShortTimeString();
            int fCount = list.Length;
            for( int i = 0; i < fCount; ++i )
            {
                sw.WriteLine("{0} {1}: {2}{3}{4}", date, time, prefix, list[i], suffix);
            }
            sw.Flush();
        }
    }


    public static void WriteSpace()
    {
        CreateDirectory(_filePath);

        using (StreamWriter sw = File.AppendText(_filePath))
        {
            sw.WriteLine("");
            sw.Flush();
        }
    }




    static void CreateDirectory(string path)
    {
        string dir = Path.GetDirectoryName(path);
        if( !string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);//경로가 없으면 만든다.
        }
    }

}
