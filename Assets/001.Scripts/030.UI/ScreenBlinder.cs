using UnityEngine;

public class ScreenBlinder : MonoBehaviour
{
    private static ScreenBlinder _instance;
    public static ScreenBlinder Instance
    {
        get
        {
            if(_instance == null)
            {
                GameObject go = new GameObject("_ScreenBlinder");
                _instance = go.AddComponent<ScreenBlinder>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private Texture _texture;
    private Color _color = Color.black;
    private float _amount = 0.0f;

    private bool _isFading = false;//update의 조건, in에서 out으로 또는 그 반대로 변경시킬지 여부
    public bool isScreenVisible
    {
        get
        {
            //isFading 이 false 이고, 컬러의 알파값이 0 이하(완전한 투명) 상태여야 isScreenVisible 이 true
            return !_isFading && _color.a <= 0.0f;
        }
    }

    private void Awake()
    {
        _texture = ResourceSystem.LoadRef("Texture/white", gameObject) as Texture;
        _color.a = 0.0f;
    }
    
    private void OnDestroy()
    {
        _texture = null;
        _instance = null;
    }

    private void Update()
    {
        if(_isFading)
        {
            _color.a += _amount * Time.deltaTime;//amount는 time이 1일 경우 1, 0.5일 경우 2/ 얼마나 빨리 in out 되는가 여부
            if(_amount > 0.0f)//FadeOut 상황
            {
                if(_color.a >= 1.0f)//검정화면이 완벽한 불투명이 되었을 때
                {
                    _color.a = 1.0f;//깔금한 수치로 하고
                    _isFading = false;//업데이트 그만한다.
                }
            }
            else if( _amount < 0.0f)//FadeIn 상황
            {
                if(_color.a <= 0.0f)//검정화면이 완벽하게 투명화 되었을 때
                {
                    _color.a = 0.0f;//깔금한 수치로 하고
                    _isFading = false;//업데이트 그만한다.
                    gameObject.SetActive(false);//오브젝트 비활성화
                }
            }
            else//변화의 폭이 되는 _amount 가 0이면 변화가 없이 계속 업데이트 되니깐 인위적으로 업데이트 취소
            {
                _isFading = false;
            }


        }
    }


    private void OnGUI()
    {
        GUI.color = _color;
        GUI.DrawTexture(new Rect(0.0f, 0.0f, Screen.width, Screen.height), _texture);
    }

    public void BlinderOn() { _color.a = 1.0f; }
    public void BlinderOff() { _color.a = 0.0f; }

    public WaitForSeconds BlinderFadeOut(float time = 0.5f)
    {
        _color.a = 1.0f;
        _amount = -1.0f / time;
        _isFading = true;

        return new WaitForSeconds(time);
    }

    public WaitForSeconds BlinderFadeIn(float time = 0.5f)
    {
        gameObject.SetActive(true);

        _color.a = 0.0f;
        _amount = 1.0f / time;
        _isFading = true;

        return new WaitForSeconds(time);
    }
}
