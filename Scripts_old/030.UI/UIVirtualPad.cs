using UnityEngine;

public class UIVirtualPad : MonoBehaviour
{
    static public UIVirtualPad current;

    public Transform background;
    public Transform stick;
    public float threshold = 0.2f;

    private bool _isMoving = false;
    public bool isMoving
    {
        get
        {
            return _isMoving;
        }
    }

    //private ClientActor player;
    private bool isPressed = false;
    private bool isEnableInput = false;

    private void OnEnable()
    {
        current = this;
    }

    private void OnDisable()
    {
        if (current == this)
        {
            current = null;
        }
    }

    //public void SetupPlayer(ClientActor actor)
    //{
    //    player = actor;
    //}

    public void OnPress(bool pressed)
    {
        if (!isEnableInput)
        {
            return;
        }

        if (pressed)
        {
            background.position = UICamera.currentCamera.ScreenToWorldPoint(UICamera.currentTouch.pos);
            stick.position = background.position;
            isPressed = true;
        }
        else
        {
            background.localPosition = Vector2.zero;
            stick.localPosition = Vector3.zero;
            isPressed = false;
        }

        UpdatePosition();
    }

    public void OnDrag()
    {
        if (!isEnableInput)
        {
            return;
        }

        if (isPressed)
        {
            stick.position = UISystem.Instance.mainCamera.ScreenToWorldPoint(UICamera.currentTouch.pos);
            UpdatePosition();
        }
    }


    private void UpdatePosition()
    {
        Vector3 toward = stick.position - background.position;
        float distance = toward.magnitude;

        if (distance > 0.01f)
        {
            if (distance > threshold)
            {
                stick.position = background.position + toward.normalized * threshold;
            }

            if (!isMoving)//현 상태가 뛰기가 아니라면
            {
                //player.ChangeState("run");
                _isMoving = true;
            }

            if (GInfo.isAutoActivity)
            {
                Messenger.Broadcast(EventKey.StopAutoMoving, MessengerMode.DONT_REQUIRE_LISTENER);
            }

            //player.moveDirection = new Vector3(toward.x / distance, 0.0f, toward.y / distance);
        }
        else
        {
            StopMove();
        }

    }
    private void StopMove()
    {
        if (_isMoving)
        {
            //player.ChangeState("nonbattle_idle");
            _isMoving = false;
        }
    }

    public void Release()
    {
        stick.localPosition = Vector2.zero;
        background.localPosition = Vector2.zero;
        isPressed = false;

        UpdatePosition();
        StopMove();//없어도 될듯 UpdatePosition 내부에서 수행 될듯한데
    }

    public void OnFinishTween()
    {
        isEnableInput = true;
    }

    
}
