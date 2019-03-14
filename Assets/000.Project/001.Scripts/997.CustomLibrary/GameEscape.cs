using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEscape
{
    public enum EscapeType
    {
        Policy,
        Login,
        Town,
        Field,
        Dungeon,
        PvpMatching,
        CreateCharacter,
    }

    private MonoBehaviour _owner = null;
    private Callback _EscapeCallback = null;

    public void Initialize(MonoBehaviour mono, EscapeType escapeType)
    {
        DLog.LogMSG("GameEscape Initialize, mono.name:" + mono.name);
        DLog.LogMSG("GameEscape Initialize, escapeType:" + escapeType);

        _owner = mono;

        switch(escapeType)
        {
            case EscapeType.Policy:
                _EscapeCallback = EscapePolicy;
                break;
            case EscapeType.Login:
                _EscapeCallback = EscapeLogin;
                break;
            case EscapeType.Town:
                _EscapeCallback = EscapeTown;
                break;
            case EscapeType.Field:
                _EscapeCallback = EscapeField;
                break;
            case EscapeType.Dungeon:
                _EscapeCallback = EscapeDungeon;
                break;
            case EscapeType.PvpMatching:
                _EscapeCallback = EscapePvpMatching;
                break;
            case EscapeType.CreateCharacter:
                _EscapeCallback = EscapeCreateCharacter;
                break;
        }
    }

    //_owner 의 업데이트 합수에서 콜 해줘야 함
    public void Update()
    {
        if(_EscapeCallback != null)
        {
            _EscapeCallback();
        }
    }
           
    private void EscapePolicy()
    {

    }
    private void EscapeLogin()
    {
        
    }
    private void EscapeTown()
    {
        if( CheckOkEscape() )
        {
            Utility.ShowMessageBox( Localization.Get("EscapeTown Test Title"),
                                    Localization.Get("EscapeTown Test2 Message"),
                                    MessageBoxType.OkCancel,
                                    delegate (){ Application.Quit(); }
                                    );
        }
    }
    private void EscapeField()
    {
        if( CheckOkEscape() )
        {
            _EscapeCallback = null;
            _owner.StartCoroutine(LoadScene());
        }
    }
    private void EscapeDungeon()
    {
        if( CheckOkEscape() )
        {
            Transform tm = UISystem.Instance.FindPanel("Panel Pause");
            if(tm != null && tm.gameObject.activeSelf)
            {
                UIPanelPause pp = tm.GetComponent<UIPanelPause>();
                if(pp != null)
                {
                    _EscapeCallback = null;
                    pp.Exit();
                }
            }
            else
            {
                Messenger.Broadcast(EventKey.ShowPause, MessengerMode.DONT_REQUIRE_LISTENER);
            }
        }
    }
    private void EscapePvpMatching()
    {
        if( CheckOkEscape() )
        {
            Transform tm = UISystem.Instance.FindPanel("Panel Lobby PVP");
            if(tm != null && tm.gameObject.activeSelf)
            {
                UIPanelLobbyPVP pp = tm.GetComponent<UIPanelLobbyPVP>();
                if(pp != null)
                {
                    _EscapeCallback = null;
                    pp.Exit();
                }
            }
        }
    }
    private void EscapeCreateCharacter()
    {

    }
    
    private bool CheckOkEscape()
    {
        if(Input.GetKeyDown(KeyCode.Escape))//일단 나가기 버튼을 눌러야 모든 검사 시작
        {
            if (Loading.isLoading) return false;//로팅 상태면 false, 로딩 완료 됐다면 다음 줄 검사
            if (!ScreenBlinder.Instance.isScreenVisible) return false;//화면이 보이는 상태(=암막이 완전히 제거된)가 되지 않았다면 false, 암막이 완전 제거되어 화면이 보인다면 다음줄 검사

            //if (TutorialController.isProgress) return false;//튜토리얼 상태라면 false, 아니라면 다음줄 검사
            
            if (UISystem.Instance.IsExistToHide()) return false;//UISystem.instance.Escape() 은 나가기 버튼 눌렀을때 더 이상 닫을 UI 없는가? 를 알아보는 메소드, 있다면 그걸 닫고 false, 없다면 다음줄 검사

            return true;//최종적으로 게임을 나가겠냐는 UI를 노출
        }
        return false;
    }

    private IEnumerator LoadScene()
    {
        yield return ScreenBlinder.Instance.BlinderFadeIn();

        SceneLoad.nextScene = "Town";
        SceneLoad.spawnPoint = "Spawn Default";
        SceneManager.LoadScene("Loading");
    }


}
