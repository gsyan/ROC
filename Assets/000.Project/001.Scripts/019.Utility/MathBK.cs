using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathBK
{
    /// <summary>
    /// dot: have to calculated with identity vectors
    /// </summary>
    /// <param name="dot"></param>
    /// <param name="angle"></param>
    public static void AngleFromDot(float dot, ref float angle)
    {
        float rad = Mathf.Acos(dot);
        angle = Mathf.Rad2Deg * rad;
    }


	
}
