using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIQuickMenuCamp : UIQuickMenu
{
    public GameObject newFriend;
    public GameObject newMission;
    public GameObject newQuest;
    public GameObject newMailBox;
    public GameObject newToggle;
    public GameObject newMagicShop;
    public GameObject newFriendShipGamble;

    //bk start pvp, challenge, private alarm
    public GameObject newPVP;
    public GameObject newChallenge;
    public GameObject newPrivateStore;

    public GameObject newInventory;
    //bk end

    private float elpasedTime = 0.0f;
    private float updateIntervalTime = 1.0f;
    private bool updateable = false;



    private void OnEnable()
    {
        //이벤트 등록
    }
    private void OnDisable()
    {
        //이벤트 해제
    }
    
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }




}
