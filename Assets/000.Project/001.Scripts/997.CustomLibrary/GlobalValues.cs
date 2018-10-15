using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 서버에서 받는 글로벌 정보(serverGlobalValues) + 클라 자체 글로벌 정보
/// </summary>
public static class GlobalValues
{
    public static ServerGlobalValues serverGlobalValues;

    //패널 종류
    public const string PANEL_MAIN = "Panel Main";//Panel Main
    public const string PANEL_LOADING = "Panel Loading";//로딩시 나오는 패널
    public const string PANEL_CONNECTING = "Panel Connecting";//서버 접속 시도시 접속중 이미지
    public const string PANEL_ASSET = "Panel Asset";
    public const string PANEL_CASHSTORE = "Panel CashStore";
    public const string PANEL_MANAGE_FLEET = "Panel ManageFleet";//함대 관리 창
    public const string PANEL_BATTLE_MENU = "Panel BattleMenu";//전투 스테이지 선택 UI



    //일반 UI
    public const string PLAYER_GRAPHIC_STATUS = "Player Status";//던전 내의 체력, 분노 게이지, 버프




    //숫자변수
    public const int energyCountMax = 100;
    public const int goldRechargeMax = 1000000;

    public const int FleetCount = 1;
    

    public static void Setup(ServerGlobalValues server)
    {
        serverGlobalValues = server;//혹시나 얕은 복사가 일어나는게 문제가 되는지 확인해보자


    }


}
