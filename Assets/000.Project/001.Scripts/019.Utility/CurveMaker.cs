using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveMaker : MonoBehaviour
{
    public Transform moveObject;
    public float moveSpeed;

    public Transform wayPointStart;
    public Transform wayPointEnd;
    public int wayPointCount;
    public float wayPointDistance;
    public List<Transform> wayPointList = new List<Transform>();

    public int curvedPointCount;
    public List<Vector3> curvedPointList = new List<Vector3>();


    float index = 0.0f;

    private void Start()
    {
        index = 0;
        moveObject.position = curvedPointList[(int)index];
    }

    private void Update()
    {
        index += Time.deltaTime * moveSpeed * curvedPointCount;
        if (index >= curvedPointList.Count)
        {
            index = 0;
        }

        moveObject.position = curvedPointList[(int)index];
    }




    void OnDrawGizmos()
    {
        curvedPointList = MakeSmoothCurve(ToVector3Array(wayPointList), curvedPointCount);

        bool ptset = false;
        Vector3 lastpt = Vector3.zero;
        for (int j = 0; j < curvedPointList.Count; j++)
        {
            Vector3 wayPoint = curvedPointList[j];
            if (ptset)
            {
                Gizmos.color = new Color(0, 0, 1, 0.5f);
                Gizmos.DrawLine(lastpt, wayPoint);
            }
            lastpt = wayPoint;
            ptset = true;
        }
        //if (isCircular)
        //{
        //    Gizmos.DrawLine(lastpt, wayPoints[0].position);
        //}

    }
    private List<Vector3> MakeSmoothCurve(Vector3[] wayPointArray, int curvedPointCount)
    {
        int wayPointArrayLength = wayPointArray.Length;
        if (curvedPointCount < wayPointArrayLength)
        {
            curvedPointCount = wayPointArrayLength;
        }

        List<Vector3> points;
        List<Vector3> curvedPoints = new List<Vector3>(curvedPointCount);

        float t = 0.0f;
        for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedPointCount; pointInTimeOnCurve++)
        {
            t = Mathf.InverseLerp(0, curvedPointCount - 1, pointInTimeOnCurve);

            points = new List<Vector3>(wayPointArray);

            for (int j = wayPointArrayLength - 1; j > 0; j--)
            {
                for (int i = 0; i < j; i++)
                {
                    points[i] = (1 - t) * points[i] + t * points[i + 1];
                }
            }

            //GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            //obj.transform.localScale = Vector3.one * 0.1f;
            //obj.transform.position = points[0];

            curvedPoints.Add(points[0]);
        }
        //return (curvedPoints.ToArray());
        return curvedPoints;
    }

    private Vector3[] ToVector3Array(List<Transform> wayPointList)
    {
        int count = wayPointList.Count;
        Vector3[] curvePoints = new Vector3[count];

        for (int i = 0; i < count; ++i)
        {
            curvePoints[i] = wayPointList[i].position;
        }

        return curvePoints;
    }



}
