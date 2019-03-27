using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathBK
{
    /// <summary>
    /// 내적 수치를 근거로 두 백터의 각도 계산
    /// dot: have to calculated with identity vectors
    /// </summary>
    public static void AngleFromDot(float dot, ref float angle)
    {
        float rad = Mathf.Acos(dot);
        angle = Mathf.Rad2Deg * rad;
    }


	
}
