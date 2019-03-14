using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;
using System.IO;

public class UserIdFile
{
    ///////////////////////////////////////////////////////////////////////////////////////////
    static string GetFilePath(int index)
    {
        return string.Format("{0}/user_id_{1}.data", Application.persistentDataPath, index);
    }
    public static void Write(string userId)
    {
        File.WriteAllText(GetFilePath(PlayerPrefsManager.Instance.Last_Selected_Server), userId);
    }
    public static void Write(List<string> userIdList)
    {
        StringBuilder sb = new StringBuilder();

        for (int i=0; i<userIdList.Count; ++i)
        {
            sb.Append(userIdList[i]);
            if(i < userIdList.Count - 1)
            {
                sb.Append(',');
            }
        }

        File.WriteAllText( GetFilePath(PlayerPrefsManager.Instance.Last_Selected_Server), sb.ToString() );
    }
    public static bool Read(out List<string> userIdList)
    {
        string filePath = GetFilePath(PlayerPrefsManager.Instance.Last_Selected_Server);
        userIdList = new List<string>();

        if(File.Exists(filePath))
        {
            string text = File.ReadAllText(filePath);
            string[] uerIds = text.Split(',');
            for(int i=0; i < uerIds.Length; ++i)
            {
                userIdList.Add(uerIds[i]);
            }
            return true;
        }
        return false;
    }
    public static void Delete()
    {
        File.Delete(GetFilePath(PlayerPrefsManager.Instance.Last_Selected_Server));
    }

    ///////////////////////////////////////////////////////////////////////////////////////////
    static string GetPrevFilePath(int index)
    {
        return string.Format("{0}/prev_user_id_{1}.data", Application.persistentDataPath, index);
    }
    public static void WritePrev(string userId)
    {
        File.WriteAllText(GetPrevFilePath(PlayerPrefsManager.Instance.Last_Selected_Server), userId);
    }
    public static bool ReadPrev(out List<string> userIdList)
    {
        string filePath = GetPrevFilePath(PlayerPrefsManager.Instance.Last_Selected_Server);
        userIdList = new List<string>();

        if (File.Exists(filePath))
        {
            string text = File.ReadAllText(filePath);
            string[] uerIds = text.Split(',');
            for (int i = 0; i < uerIds.Length; ++i)
            {
                userIdList.Add(uerIds[i]);
            }
            return true;
        }
        return false;
    }
    public static void DeletePrev()
    {
        File.Delete(GetPrevFilePath(PlayerPrefsManager.Instance.Last_Selected_Server));
    }
    
}
