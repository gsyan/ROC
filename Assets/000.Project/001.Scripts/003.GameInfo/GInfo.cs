using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine.Assertions;



/// <summary>
/// 변하는 정보값 담당, GData(불변데이터) 와 대척점
/// </summary>
public class GInfo
{
    public static ServerType serverType;    //개발, 사용, 등등 인지 구분
    public static int serverGroup = 0;      //이건 ?
    public static int patchVersion = 0;     //패치 버전 0 부터 1, 2, 3, ....


    //time
    public static System.DateTime serverTimeAtLogin;        //게임 로그인 시 서버 시간을 저장
    public static float realtimeSinceStartupAtlogin;        //게임 로그인 시 Time.realtimeSinceStartup 을 저장
    public static System.DateTime serverTime
    {
        get
        {
            //서버로 부터 받은 DateTime 정보에  <현재까지 흐른 시간> - 로그인 시점의 <현재까지 흐른시간> 을 더해줌 
            return serverTimeAtLogin.AddSeconds(Time.realtimeSinceStartup - realtimeSinceStartupAtlogin);
        }
    }


    //접속시 한번만 처리하기
    public static bool bCheckViewNotice = true;
    public static bool bCheckViewRecommendPackage = true;
    public static bool bRestorePurchase = true;

    //유저 정보, 게임 정보 중 player에 국한되는, 맞춰진 정보들
    public static PlayerInfo playerInfo;


    //추천 패키지 상품
    public static List<ProductInfo> productInfoList = new List<ProductInfo>();
    public static List<ProductInfo_Package> recommentPackageList = new List<ProductInfo_Package>();


    //공지
    public static NoticeType noticeType = NoticeType.None;
    public static List<KeyValuePair<string, string>> noticeBannerList = new List<KeyValuePair<string, string>>();//string key, string imageURL
    


    public static bool isAutoActivity = false;

    public static int battleCount = 0;
    private static bool _isContinueBattle = false;
    public static bool isContinueBattle
    {
        get
        {
            return _isContinueBattle;
        }
        set
        {
            if (value)
            {
                isAutoActivity = true;
            }
            else
            {
                battleCount = 0;
                continueBattleConsumableList.Clear();
            }
            _isContinueBattle = value;
        }
    }
    
    public static List<int> continueBattleConsumableList = new List<int>();


    private static bool _bSetup = false;
    public static void Setup(PlayerInfo inPlayerInfo)
    {
        Clear();

        playerInfo = inPlayerInfo;
        


        _bSetup = true;
    }
    /// <summary>
    /// 테스트용 Setup, 로그인 scene이 아닌 개별 scene에서 시작할 때(테스트)를 위해서 만듬
    /// </summary>
    public static void SetupTest()
    {
        if (_bSetup) { return; }

        PlayerInfo aPlayerInfo = new PlayerInfo();
        aPlayerInfo.TestInit();
        aPlayerInfo.usn = 1234L;

        serverTimeAtLogin = DateTime.Now;
        realtimeSinceStartupAtlogin = Time.realtimeSinceStartup;


        Setup(aPlayerInfo);
        
    }


    public static void Clear()
    {
        if(playerInfo != null)
        {
            playerInfo.Clear();
        }
        
        //partial 된 class 각각의 clear()



    }




    public static AssetInfo GetAssetInfo(AssetType aType)
    {
        return playerInfo.assetInfoList.Find( (AssetInfo ai) => { return ai.type == aType; } );
        //return playerInfo.assetInfoList.Find( delegate(AssetInfo ai) { return ai.type == aType; }); //형태는 다르나 같은 작동
    }
    public static void UpdateAsset(AssetInfo assetInfo)
    {
        Assert.IsNotNull<AssetInfo>(assetInfo);

        AssetInfo ai = playerInfo.assetInfoList.Find((AssetInfo _ai) => { return _ai.type == assetInfo.type; });
        if( ai != null)
        {
            ai.curCount = assetInfo.curCount;
            ai.extraCount = assetInfo.extraCount;
            ai.nextChageTime = assetInfo.nextChageTime;
            ai.lastChageTime = assetInfo.lastChageTime;
        }
        else
        {
            playerInfo.assetInfoList.Add(assetInfo);
        }
    }




    public static bool IsShowNoticeBannerInfo()
    {
        return noticeBannerList.Count > 0 ? true : false;
    }
    public static void AddNoticeType(NoticeType inNoticeType)
    {
        if (!BKEnum.Has(noticeType, inNoticeType))
        {
            noticeType = BKEnum.Include(noticeType, inNoticeType);
            //Messenger.Broadcast(EventKey.UpdateNotice, MessengerMode.DONT_REQUIRE_LISTENER);
        }
    }
    public static void DeleteNoticeType(NoticeType inNoticeType)
    {
        if (BKEnum.Has(noticeType, inNoticeType))
        {
            noticeType = BKEnum.Exclude(noticeType, inNoticeType);
            //Messenger.Broadcast(EventKey.UpdateNotice, MessengerMode.DONT_REQUIRE_LISTENER);
        }
    }
    public static void IncludeNoticeType(NoticeType inNoticeType)
    {// just include, no update
        if (!BKEnum.Has(noticeType, inNoticeType))
        {
            noticeType = BKEnum.Include(noticeType, inNoticeType);
        }
    }









}
