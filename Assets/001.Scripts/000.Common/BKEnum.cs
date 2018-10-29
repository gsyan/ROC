using UnityEngine;
using System.Collections;

public class BKEnum
{
    //DamageType
    public static bool Is(DamageType a, DamageType b)
    {
        return a == b;
    }
    public static bool Has(DamageType a, DamageType b)
    {
        return (a & b) == b;
    }
    public static DamageType Include(DamageType a, DamageType b)
    {
        return a | b;
    }
    public static DamageType Exclude(DamageType a, DamageType b)
    {
        return a & ~b;
    }
    public static void PrintFlag(DamageType type)
    {
        Debug.Log(((DamageType)type).ToString());
    }

    public static bool Is(AttackType a, AttackType b)
    {
        return a == b;
    }
    public static bool Has(AttackType a, AttackType b)
    {
        return (a & b) == b;
    }
    public static AttackType Include(AttackType a, AttackType b)
    {
        return a | b;
    }
    public static AttackType Exclude(AttackType a, AttackType b)
    {
        return a & ~b;
    }


    public static bool Has(NoticeType a, NoticeType b)
    {
        return (a & b) == b;
    }
    public static NoticeType Include(NoticeType a, NoticeType b)
    {
        return a | b;
    }
    public static NoticeType Exclude(NoticeType a, NoticeType b)
    {
        return a & ~b;
    }
    
}

public enum GameMode
{
    None,
    PVE,
    PVP,
    RVE,
    Challenge,
    DailyDungeon,
    Max,
}
public enum TeamType
{
    None,
    Red,    //적군
    Blue,   //아군
    Green,  //중립
    Max,
}

public enum ShipType
{
    None = 0,
    D,
    C,
    B,
    A,
    E,
    T,
    L,
    Max
}


public enum BeamType
{
    None,
    Style1,
    Style2,
    Style3,
    Max
}
public enum MissleType
{
    None,
    Style1,
    Style2,
    Style3,
    Max
}
public enum FighterType
{
    None,
    Style1,
    Style2,
    Style3,
    Max
}
public enum ShieldType
{
    None,
    Style1,
    Style2,
    Style3,
    Max
}


public enum StoreType
{
    None,
    Apple,
    Google,
    Naver,
    OneStore,
    Max
}
public enum ProductType
{
    None,
    Cube,
    Gold,
    Avatar,
    Weapon,
    Regular,
    Package,
    Event,
    Max,
}

public enum FleetFormationType
{
    None = -1,
    Wing = 0,
    Wing_Foward,
    Line_Abreast,       //횡렬 라인 진형
    Line_In_depth,      //종심 라인 진형

    Max
}




[System.Flags]//이것을 붙임으로서 3을 프린트 할때 3이 아니라 physics, pure 로 출력 된다.
public enum DamageType
{
    None = 0,
    Physics = 1,
    Pure = 2,
    Penetration = 4,
    Dot = 8,
    Interrupt = 16,
    Critical = 32,
    Hit = 64,
    Miss = 128,
    Invincible = 256,
    Defence = 512,
    PerfectDefence = 1204,
    All = int.MaxValue
}
[System.Flags]
public enum AttackType
{
    None = 0,
    Physics = 1,
    Pure = 2,
    Penetration = 4,
    Dot = 8,
    Interrupt = 16,
    All = int.MaxValue
}
[System.Flags]
public enum NoticeType
{
    None = 0,
    EventMail = 1,          // 이벤트 우편 받음 알림
    SocialMail = 2,         // 소셜 퀘스트 우편 받음 알림
    RequestFriend = 4,      // 친구 요청 받음 알림
    SocialQuest = 8,	    // 소셜 퀘스트 받음 알림
    All = int.MaxValue
}





public enum ItemType
{
    None,
    Weapon,
    Armor,
    Skin,//Avatar
    Combinable,
    Consumable,
    Rechargeable,
    Currency,
    Gamble,
    WeaponSkin,
    Max,
}
public enum WeaponType
{
    None,
    Mace = 1,
    Axe = 3,
    GreatSword = 5,

    Dagger = 2,
    Sword = 4,
    Staff = 6,
    Max,
}
public enum ArmorType
{
    Headgear,
    Shoulders,
    Chest,
    Gloves,
    Bracer,
    Belt,
    Leggings,
    Boots,
    Earring,
    Necklace,
    Ring,
    Max,
}
public enum PartsType
{
    Headgear = 0,
    Shoulders,
    Chest,
    Gloves,
    Bracer,
    Belt,
    Leggings,
    Boots,
    Earring,
    Necklace,
    Ring,
    Avatar,
    Weapon,
    WeaponAvatar,
    Max,
}
public enum GemType
{
    Emerald,
    Ruby,
    Sapphire,
    Topaz,
    Diamond,
    Max,
}


public enum ColorType
{
    A,
    B,
    C,
    D,
    Max,
}

public enum CharacterType
{
    None,
    Nick,
    Ahn,
    
    Max,
}
public enum CharacterRank
{
    None,
    Admiral,                //장성급
    Captain,                //함장
    Max,
}



public enum SkillSlotType
{
    Slot_1 = 0,
    Slot_2,
    Slot_3,
    Slot_4,
    Slot_5,
    Slot_6,
    Max,//SlotMax
}





public enum BuyType
{
    None = 0,
    Cash,
    Cube,
    Gold,
}




public enum DungeonState
{
    None,
    Locked,
    Progress,
    Clear,
    Max,
}

public enum FieldLevel
{
    None,
    Normal,
    Hard,
    Expert,
    Max,
}

public enum ChatType
{
    Normal = 0,     // 일반
    Whisper = 1,    // 귓속말    
    Notify = 2,     // 공지
    Guild = 3       // 길드
}

public enum ServerType
{
    None,
    Local,
    Dev,            // 개발용
    Staging,        
    Production,     // 상용
    Custom,
    Max,
}

public enum CurrencyType
{
    None = 0,
    Gold,
    Cube,
    PvP_Point,
    Challenge_Point,
    Friendship_Point,
}


public enum MarketType
{
    None = 0,
    AppleStore,
    GoogleStore,
    NStore,
    OneStore,
    WindowStore,
}

public enum EffectType
{
    None,
    Buff,
    Debuff,
    Max,
}

public enum AssetType
{
    None = 0,
    Energy,
    Gold,
    Cube,


    Max
}
public enum AssetRechargeType
{
    None = 0,
    Interval,
    Midnight,

    Max
}

public enum BlockType
{
    None = 0,
    Login,
    Chatting,
}
public enum BlockStatus
{
    Period = 0,
    Endless = 1,
    Clear = 2,
}

public enum SceneLayerType
{
    None = 0,                       //0
    Camp = (1 << 0),                //1
    SafetyRoom = (1 << 1),          //2
    BattleStage = (1 << 2),          //4
    FieldDungeon = (1 << 3),        //8
    FieldDailyDungen = (1 << 4),    //16
    FieldChallenge = (1 << 5),      //32
    FieldPVP = (1 << 6),            //64
    PVP = (1 << 7),                 //128
    Tutorial = (1 << 8),            //256
    TutorialBattle = (1 << 9),      //512
    SafetyDungeonRoom = (1 << 10),  //1024
    FieldBossRaid = (1 << 11),      //2048
}

public enum MessageBoxType
{
    Ok,
    OkCancel,
    YesNo,
    YesNoCancel,
}


//Delegates
public delegate void Callback();
public delegate void Callback<T>(T arg1);
public delegate void Callback<T1, T2>(T1 arg1, T2 arg2);
public delegate void Callback<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
public delegate void Callback<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
public delegate void Callback<T1, T2, T3, T4, T5>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
public delegate void Callback<T1, T2, T3, T4, T5, T6>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
public delegate void Callback<T1, T2, T3, T4, T5, T6, T7>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

public delegate TResult CallbackResult<TResult>();
public delegate TResult CallbackResult<TResult, T1>(T1 arg1);
public delegate TResult CallbackResult<TResult, T1, T2>(T1 arg1, T2 arg2);
public delegate TResult CallbackResult<TResult, T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3);
public delegate TResult CallbackResult<TResult, T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);