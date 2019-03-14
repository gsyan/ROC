using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Text;

//참고사항
//CallerFilePath, CallerLineNumber, CallerMemberName 어트리븃을 사용하기 위해서는
//CustomEditor 에서 하듯 namespace System.Runtime.CompilerServices 에 추가적으로 등록해준다. 사실 이건 꼼수라고 한다.
//유니티에 포함된 .Net 버전이 4.5 가 안되어서 없는것을 인위적으로 넣어준것


public static class DLog
{
    public static void LogMSG(string message, [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0, [CallerMemberName] string memberName = "")
    {
#if UNITY_DEBUG

#if UNITY_EDITOR
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(message);
        sb.Append("File: " + sourceFilePath);
        sb.Append("// Line: " + sourceLineNumber);
        sb.Append("(Function: " + memberName + ")");

        //Create log file
        //string FileName = @"D:\" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".log";
        //if (File.Exists(FileName))
        //{
        //    File.Delete(FileName); // remove existing
        //}
        //using (StreamWriter sw = File.CreateText(FileName))
        //{
        //    sw.Write(sb.ToString());         // write entire contents
        //    sw.Close();
        //}
        
        Debug.Log(sb.ToString());
#else
        NativeBridge.Log(message);
#endif



#endif
    }


}


