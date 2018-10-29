using UnityEngine;

public class UIColorSetting : MonoBehaviour
{
    // 재화(큐브 스태미나 골드, pvp, 챌린저 제외) 부족, 만족
    public Color lackAsset;
    public Color abundanceAsset;

    public Color lackChallengeAsset;
    public Color abundanceChallengeAsset;

    public Color lackPvpAsset;
    public Color abundancePvpAsset;

    public Color lackGold;
    public Color abundanceGold;

    public Color lackCube;
    public Color abundanceCube;

    public Color lackStamina;
    public Color abundanceStamina;

    // 재화를 제외한 부족, 만족
    public Color lackDefault;
    public Color abundanceDefault;

    // 버튼 필드 타이틀
    public Color buttomFieldTitleOpen;
    public Color buttomFieldTitleClose;

    // 탭버튼 눌렸을때 안눌렸을때
    public Color tabButtonDefault;
    public Color tabButtonSelect;

    // 채팅
    public Color chatNormal;
    public Color chatGuild;
    public Color chatWhisper;
    public Color chatNotify;
    //public Color highlightItem;


    public Color GetLackColorCurrency(CurrencyType currencyType)
    {
        switch (currencyType)
        {
            case CurrencyType.Gold:
                return lackGold;

            case CurrencyType.Cube:
                return lackCube;

            case CurrencyType.Challenge_Point:
                return lackChallengeAsset;

            case CurrencyType.PvP_Point:
                return lackPvpAsset;
        }

        return lackAsset;
    }


    public Color GetAbundanceColorCurrency(CurrencyType currencyType)
    {
        switch (currencyType)
        {
            case CurrencyType.Gold:
                return abundanceGold;

            case CurrencyType.Cube:
                return abundanceCube;

            case CurrencyType.Challenge_Point:
                return abundanceChallengeAsset;

            case CurrencyType.PvP_Point:
                return abundancePvpAsset;
        }

        return abundanceAsset;
    }


    public Color GetChatColor(ChatType type)
    {
        switch (type)
        {
            case ChatType.Normal:
                return chatNormal;

            case ChatType.Guild:
                return chatGuild;

            case ChatType.Whisper:
                return chatWhisper;

            case ChatType.Notify:
                return chatNotify;
        }

        return chatNormal;
    }


}
