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

        //float[,] mat = { { 2, 1, -1, 8 }, { -3, -1, 2, -11 }, { -2, 1, 2, -3 } };
    }

    
	void Update ()
    {
        //Vector3 point1 = new Vector3(5, 1, 3);
        //Vector3 vector1 = new Vector3(1,4,-2);
        //Debug.DrawLine(point1, vector1 * 100, Color.blue);

        //Vector3 point2 = new Vector3(4, 1, 3);
        //Vector3 vector2 = new Vector3(2, 4, -2);
        //Debug.DrawLine(point2, vector2 * 100, Color.green);

        //Vector3 p0Position = Vector3.zero;
        //bool intersection = LineIntersection(point1, vector1, point2, vector2, ref p0Position);
        //p0.position = p0Position;

        _lr1.SetPosition(0, p1.position);
        _lr1.SetPosition(1, p2.position);

        _lr2.SetPosition(0, p3.position);
        _lr2.SetPosition(1, p4.position);

        Vector3 p0Position = Vector3.zero;
        bool intersection = LineIntersection(p1.position, p2.position - p1.position, p3.position, p4.position - p3.position, ref p0Position);
        p0.position = p0Position;

        
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

    bool LineIntersection(Vector3 p1, Vector3 v1, Vector3 p2, Vector3 v2, ref Vector3 p0)
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

        //직선의 백터가 하나라도 0인 경우는 걸러서 가우스 소거법을 쓴다.

        float A1, B1, C1;
        float A2, B2, C2;
        A1 = v1.x;
        B1 = v2.x;
        C1 = p2.x - p1.x;
        A2 = v1.y;
        B2 = v2.y;
        C2 = p2.y - p1.y;

        float[,] mat = { { A1, B1, C1 }, { A2, B2, C2 } };
        float[] ans = GaussianElimination(2, mat);

        p0 = p1 + ans[0] * v1;

        return true;
    }

    bool LineIntersection2(Vector3 p1, Vector3 v1, Vector3 p2, Vector3 v2, ref Vector3 p0)
    {
        float det = v1.cross

        return true;
    }

    float[] GaussianElimination(int row, float[,] mat)
    {
        int column = row + 1;
        float[] ans = new float[row];
        float divisor = 1.0f;
        //상삼각행렬을 만드는 작업
        //row 1, col 0 에 기준점을 잡고 시작
        //row 1, col 0 을 포함해 col 0의 row 값들을 모두 0으로 만드는 작업 수행
        //col 0 가 끝나면
        //row 2, col 1 에 기준점을 잡고 다시 시작
        //row 2, col 1 을 포함해 col 1의 row 값들을 모두 0으로 만드는 작업 수행, 반복
        for (int c = 0; c < column - 1; c++)
        {
            for (int r = c + 1; r < row; r++)//row 는 두번째 줄 부터 시작한다.
            {
                ReRowOfMat(ref mat, row, column, c, c);
                if(mat[r, c] == 0.0f) { continue; }//0으로 만들어야 할 놈이 이미 0이라면 continue

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
        for (int r = row - 1; r >= 0; --r)
        {
            float t = mat[r, column - 1];
            for (int c = column - 2; c > r; --c)
            {

                t -= (mat[r, c] * ans[c]);
            }
            ans[r] = t / mat[r, r];
        }
        return ans;
    }
    private bool ReRowOfMat(ref float[,] mat, int row, int col, int checkRow, int checkCol)
    {
        //divisor 가 0이 되면 안된다. 0이면 밑에 row 들을 검색해서 0이 아닌 row와 스위칭을 한다.
        //false 리턴: row 재조정 없이 통과
        //true 리턴 : row 재조정 된 것
        float[] zeroRowValues = new float[col];//check point 값이 0인 row의 모든 값들을 임시로 담을 그릇

        if (mat[checkRow, checkCol] == 0.0f)
        {
            for(int c = 0; c < col; ++c)
            {
                zeroRowValues[c] = mat[checkRow, c];
            }

            for( int r = checkRow + 1; r < row; ++r)
            {
                if (mat[r, checkCol] != 0.0f)
                {
                    for (int c = 0; c < col; ++c)
                    {
                        //스위칭
                        mat[checkRow, c] = mat[r, c];
                        mat[r, c] = zeroRowValues[c];
                    }
                    return true;
                }
            }
        }
        else
        {
            return false;
        }
        return false;
    }


    bool LUDecomposition()
    {
        //행결의 하삼각행렬(L)과 상삼각행렬(U)로 분해(Decomposition)한다

        return true;
    }

    

    
}
