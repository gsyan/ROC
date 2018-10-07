using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public LineRenderer _lr1;
    public LineRenderer _lr2;

    public Transform p1;
    public Transform p2;
    public Transform p3;
    public Transform p4;
    public Transform p0;

    




    void Start ()
    {
        //_lr1.SetPosition(0, p1.position);
        //_lr1.SetPosition(1, p2.position);

        //_lr1.SetPosition(0, p3.position);
        //_lr1.SetPosition(1, p4.position);

        float[,] mat = { { 2, 1, -1, 8 }, { -3, -1, 2, -11 }, { -2, 1, 2, -3 } };
        //float[,] mat = { { 0, 0, 1, 1 }, { 3, 0, 2, 8 }, { 0, 2, 3, 9 } };
        float[] ans = GaussianElimination1(mat);

        Debug.Log(ans);

    }

    
	void Update ()
    {
        Vector3 point1 = new Vector3(5, 1, 3);
        Vector3 vector1 = new Vector3(1,4,-2);
        Debug.DrawLine(point1, vector1 * 100, Color.blue);

        Vector3 point2 = new Vector3(4, 1, 3);
        Vector3 vector2 = new Vector3(2, 4, -2);
        Debug.DrawLine(point2, vector2 * 100, Color.green);

        bool intersection = LineIntersection(point1, vector1, point2, vector2);
    }


    void LineDraw(Vector3 position, Vector3 vector )
    {
        Vector3[] points = new Vector3[4];
        for(float t = 0; t < 4; t += 1)
        {
            float lineX = position.x + vector.x * t;
            float lineY = position.y + vector.y * t;
            float lineZ = position.z + vector.z * t;
            points[(int)t] = new Vector3(lineX, lineY, lineZ);
                
        }


        p1.position = points[0];
        p2.position = points[1];
        p3.position = points[2];
        p4.position = points[3];

        _lr1.positionCount = 4;
        _lr1.SetPositions(points);
    }

    bool LineIntersection(Vector3 p1, Vector3 v1, Vector3 p2, Vector3 v2)
    {
        //아래의 주석된 내용을 바탕으로 계산
        //p1 = (x1, y1, z1), v1 = (a1, b1, c1) , p2 = (x2, y2, z2), v2 = (a2, b2, c2)

        //교점은
        //x1 + a1 * t = x2 + a2 * s
        //y1 + a1 * t = y2 + a2 * s
        //z1 + a1 * t = z2 + a2 * s

        //t, s 는 벡터을 변화 시키는 변수임, t 와 s를 구하기 위해 식을 정리
        //a1 * t - a2 * s + (x1 - x2) = 0
        //b1 * t - b2 * s + (y1 - y2) = 0
        //c1 * t - c2 * s + (z1 - z2) = 0


        

        return true;
    }

    float[] GaussianElimination1(float[,] mat)
    {
        //상삼각행렬을 만드는 작업
        int row = 3;
        int column = 4;
        float[] ans = new float[row];
        for (int c = 0; c < column - 1; c++)
        {
            for (int r = c + 1; r < row; r++)//row 는 두번째 줄 부터 시작한다.
            {
                float t = -mat[r, c] / mat[c, c];//곱해질 수
                float[] l = new float[column];

                for (int k = 0; k < column; k++)
                {
                    l[k] = mat[c, k] * t + mat[r, k];
                }
                for (int k = 0; k < column; k++)
                {
                    mat[r, k] = l[k];
                }
            }
        }
        for (int i = row - 1; i >= 0; i--)
        {
            //ans[i] = a[i][column-1]/a[i][column-2];
            float t = mat[i, column - 1];
            for (int j = column - 2; j > i; j--)
            {

                t -= (mat[i,j] * ans[j]);
            }
            ans[i] = t / mat[i,i];
        }
        return ans;
    }

    float[] GaussianElimination2(float[,] mat)
    {
        int row = 3;
        int column = 4;
        float[] ans = new float[row];

        


        return ans;
    }

    bool LUDecomposition()
    {
        //행결의 하삼각행렬(L)과 상삼각행렬(U)로 분해(Decomposition)한다

        return true;
    }

    

    
}
