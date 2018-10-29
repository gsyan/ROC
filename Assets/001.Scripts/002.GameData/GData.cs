using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;//Serializable

/// <summary>
/// 변하지 않는 정보값 담당, GInfo 와 대척점
/// </summary>
public class GData
{
    private static GData _instance;
    public static GData Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new GData();
                _instance.Init();
            }
            return _instance;
        }
    }


    //data  /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    private List<ServerData> _serverList = new List<ServerData>();
    private Dictionary<int, LevelData> _levelDatas = new Dictionary<int, LevelData>();
    private Dictionary<int, MonsterData> _monsterDatas = new Dictionary<int, MonsterData>();
    private Dictionary<int, SkillData> _skillDatas = new Dictionary<int, SkillData>();
    private Dictionary<string, SkillData> _skillDatasByName = new Dictionary<string, SkillData>();
    //private Dictionary<SkillSlotType, List<SkillSlotData>> _skillSlotDatas = new Dictionary<SkillSlotType, List<SkillSlotData>>();
    private Dictionary<int, EffectData> _effectDatas = new Dictionary<int, EffectData>();
    private Dictionary<string, EffectData> _effectDatasByName = new Dictionary<string, EffectData>();
    
    private Dictionary<int, WeaponData> _weaponDatas = new Dictionary<int, WeaponData>();
    private Dictionary<int, ArmorData> _armorData = new Dictionary<int, ArmorData>();
    private Dictionary<int, CharacterSkinData> _characterSkinDatas = new Dictionary<int, CharacterSkinData>();
    private Dictionary<int, WeaponSkinData> _weaponSkinDatas = new Dictionary<int, WeaponSkinData>();

    private Dictionary<string, Dictionary<int, ShipInfo>> _shipData = new Dictionary<string, Dictionary<int, ShipInfo>>();
    private Dictionary<FleetFormationType, Vector3[]> _shipPositionData = new Dictionary<FleetFormationType, Vector3[]>();





    
    
    //private Dictionary<int, CombinableData> _combinableDatas = new Dictionary<int, CombinableData>();
    //private Dictionary<ConsumableType, ConsumableData> _consumableDatasByType = new Dictionary<ConsumableType, ConsumableData>();
    //private Dictionary<int, ConsumableData> _consumableDatas = new Dictionary<int, ConsumableData>();

    private Dictionary<WeaponType, Dictionary<int, EquipmentGradeData>> _weaponGradeDatas = new Dictionary<WeaponType, Dictionary<int, EquipmentGradeData>>();
    private Dictionary<WeaponType, Dictionary<int, EquipmentLevelData>> _weaponLevelDatas = new Dictionary<WeaponType, Dictionary<int, EquipmentLevelData>>();
    private Dictionary<WeaponType, Dictionary<int, EquipmentEnhanceData>> _weaponEnhanceDatas = new Dictionary<WeaponType, Dictionary<int, EquipmentEnhanceData>>();
    private Dictionary<ArmorType, Dictionary<int, EquipmentGradeData>> _armorGradeDatas = new Dictionary<ArmorType, Dictionary<int, EquipmentGradeData>>();
    private Dictionary<ArmorType, Dictionary<int, EquipmentLevelData>> _armorLevelDatas = new Dictionary<ArmorType, Dictionary<int, EquipmentLevelData>>();
    private Dictionary<ArmorType, Dictionary<int, EquipmentEnhanceData>> _armorEnhanceDatas = new Dictionary<ArmorType, Dictionary<int, EquipmentEnhanceData>>();
    private Dictionary<int, EquipmentGradeData> _characterSkinGradeDatas = new Dictionary<int, EquipmentGradeData>();
    private Dictionary<int, EquipmentLevelData> _characterSkinLevelDatas = new Dictionary<int, EquipmentLevelData>();
    private Dictionary<int, EquipmentEnhanceData> _characterSkinEnhanceDatas = new Dictionary<int, EquipmentEnhanceData>();
    private Dictionary<GemType, Dictionary<int, GemLevelData>> _gemLevelDatas = new Dictionary<GemType, Dictionary<int, GemLevelData>>();

    private List<ProductData> _productDatas = new List<ProductData>();
    //private Dictionary<int, List<ProductRewardData>> _productRewardDatas = new Dictionary<int, List<ProductRewardData>>();
    private Dictionary<int, List<ItemData>> _treasureDatas = new Dictionary<int, List<ItemData>>();

    //private Dictionary<int, StageData> _stageDatas = new Dictionary<int, StageData>();
    //private Dictionary<int, FieldData> _fieldDatas = new Dictionary<int, FieldData>();
    //private Dictionary<int, DungeonData> _dungeonDatas = new Dictionary<int, DungeonData>();
    //private Dictionary<int, DungeonRoomData> _dungeonRoomDatas = new Dictionary<int, DungeonRoomData>();

    //private Dictionary<KeyValuePair<int, DifficultyType>, DailyDungeonData> _dailyDungeonDatas = new Dictionary<KeyValuePair<int, DifficultyType>, DailyDungeonData>();
    //private Dictionary<KeyValuePair<WeaponType, DifficultyType>, DailyMonsterData> _dailyMonsterDatas = new Dictionary<KeyValuePair<WeaponType, DifficultyType>, DailyMonsterData>();

    //private Dictionary<int, MissionData> _missionDatas = new Dictionary<int, MissionData>();
    //private Dictionary<CompleteType, List<MissionData>> _missionDatasByType = new Dictionary<CompleteType, List<MissionData>>();
    //private Dictionary<AchievementType, List<AchievementData>> _achievementDatas = new Dictionary<AchievementType, List<AchievementData>>();
    //private List<int> achievementIds = new List<int>();

    //private Dictionary<RankType, List<RankRewardData>> _rankRewardDatas = new Dictionary<RankType, List<RankRewardData>>();

    //private Dictionary<int, List<AttendEventData>> _attendDatas = new Dictionary<int, List<AttendEventData>>();
    //private Dictionary<int, VipGradeData> _vipGradeDatas = new Dictionary<int, VipGradeData>();

    //private Dictionary<int, QuestData> _questDatas = new Dictionary<int, QuestData>();
    //private Dictionary<int, List<DialogData>> _dialogDatas = new Dictionary<int, List<DialogData>>();
    private Dictionary<int, NpcData> _npcDatas = new Dictionary<int, NpcData>();

    private Dictionary<ErrorResult, ErrorCodeData> _errorDatas = new Dictionary<ErrorResult, ErrorCodeData>();

    //private List<StaminaPriceData> _staminaPriceDatas = new List<StaminaPriceData>();

    //private Dictionary<int, TutorialData> _tutorialDatas = new Dictionary<int, TutorialData>();
    //private Dictionary<DayRewardType, DayRewardData> _dayRewardDatas = new Dictionary<DayRewardType, DayRewardData>();
    //private Dictionary<int, ChallengeMonsterData> _challengeMonsterDatas = new Dictionary<int, ChallengeMonsterData>();

    private Dictionary<string, string> _nationNameDatas = new Dictionary<string, string>();









#if UNITY_EDITOR
    private string _currentCSVFile = "";
#endif
    private System.IO.Stream GetStringReader(string path)
    {
#if UNITY_EDITOR
        _currentCSVFile = path;
#endif
        return new System.IO.MemoryStream(ResourceSystem.LoadCSV(path));
    }
    


    private void Init()
    {
        try
        {
            LoadCSV();
        }
        catch(System.Exception e)
        {
#if UNITY_EDITOR
            Debug.Log(string.Format("failed read csv... file={0}.csv message{1}", _currentCSVFile, e.Message));
#endif
        }
    }
    private void LoadCSV()
    {
        List<string> columns = new List<string>();

        // using 문: font, file, db 같은 unmanaged resources 나 class 는 반드시 IDisposable Interface 를 구현
        // IDisposable Interface 구현한 객체를 사용하고 난후에는 반드시 IDisposable Interface 의 Dispose() 메소드를 호출해야한다.
        // using scope 벗어나는 순간 자동으로 Dispose() 메소드 호출한다. exception 발생하더라도 Dispose() 호출을 보장한다. 
        // IDisposable Interface 구현한 객체는 using 문 사용 하면 편리
        using (CsvFile.CsvFileReader csv = new CsvFile.CsvFileReader(GetStringReader("ShipBodyData")))
        {
            csv.ReadRow(columns);//제일 첫 콜럼( 항목 이름 줄) 무시

            while (csv.ReadRow(columns))
            {
                int p = 0;
                string ss = columns[p++];
                ss = columns[p++];
                ss = columns[p++];
                ss = columns[p++];
            }
        }

        using (CsvFile.CsvFileReader csv = new CsvFile.CsvFileReader(GetStringReader("ShipPositionData")))
        {
            csv.ReadRow(columns);//제일 첫 콜럼( 항목 이름 줄) 무시

            while (csv.ReadRow(columns))
            {
                int p = 0;
                
                FleetFormationType formationType = (FleetFormationType)Enum.Parse(typeof(FleetFormationType), columns[p++]);

                Vector3[] positions = new Vector3[8];
                for (int i = 0; i < 8; ++i)
                {
                    positions[i] = Vector3.zero;
                    positions[i].x = float.Parse(columns[p++]);
                    positions[i].y = float.Parse(columns[p++]);
                    positions[i].z = float.Parse(columns[p++]);
                }

                _shipPositionData.Add(formationType, positions);
            }
        }

        //_shipData
        using (CsvFile.CsvFileReader csv = new CsvFile.CsvFileReader(GetStringReader("ShipDataName")))
        {
            csv.ReadRow(columns);//첫 줄 무시
            
            while (csv.ReadRow(columns))
            {
                Dictionary<int, ShipInfo> dic = new Dictionary<int, ShipInfo>();
                for ( int i=0; i<10; ++i)
                {
                    int p = 0;
                    
                    ShipInfo shipInfo = new ShipInfo();
                    shipInfo.name = columns[p++];
                    shipInfo.type = (ShipType)Enum.Parse(typeof(ShipType), columns[p++]);
                    shipInfo.grade = int.Parse(columns[p++]);

                    BattleInfo battleInfo = new BattleInfo();
                    battleInfo.beamType = (BeamType)Enum.Parse(typeof(BeamType), columns[p++]);
                    battleInfo.beamGunCount = int.Parse(columns[p++]);
                    battleInfo.beamCool = int.Parse(columns[p++]);
                    battleInfo.missleType = (MissleType)Enum.Parse(typeof(MissleType), columns[p++]);
                    battleInfo.missleGunCount = int.Parse(columns[p++]);
                    battleInfo.missleCool = int.Parse(columns[p++]);
                    battleInfo.fighterType = (FighterType)Enum.Parse(typeof(FighterType), columns[p++]);
                    battleInfo.fighterCountCur = battleInfo.fighterCountMax = int.Parse(columns[p++]);
                    battleInfo.fighterCool = int.Parse(columns[p++]);
                    battleInfo.shieldType = (ShieldType)Enum.Parse(typeof(ShieldType), columns[p++]);
                    battleInfo.defence = int.Parse(columns[p++]);
                    battleInfo.hpCur = battleInfo.hpMax = int.Parse(columns[p++]);
                    battleInfo.moveSpeed = int.Parse(columns[p++]);
                    battleInfo.rotateSpeed = int.Parse(columns[p++]);
                    battleInfo.attackRange = int.Parse(columns[p++]);

                    shipInfo.SetBattleInfo(battleInfo);
                    dic.Add(shipInfo.grade, shipInfo);
                    if( i != 9 )
                    {
                        csv.ReadRow(columns);
                    }
                }
                _shipData.Add(dic[dic.Count-1].name, dic);
            }
        }


        //Login 시 보이는 server list
        using (CsvFile.CsvFileReader csv = new CsvFile.CsvFileReader(GetStringReader("ServerList")))
        {
            csv.ReadRow(columns);
            while (csv.ReadRow(columns))
            {
                int p = 0;
                ServerData data = new ServerData();

                data.type = (ServerType)Enum.Parse(typeof(ServerType), columns[p++]);
                data.group = int.Parse(columns[p++]);
                data.name = columns[p++];
                data.address = columns[p++];
                data.port = int.Parse(columns[p++]);

                _serverList.Add(data);
            }
        }


        //미리 약속된 에러코드
        using (CsvFile.CsvFileReader csv = new CsvFile.CsvFileReader(GetStringReader("ErrorCode")))
        {
            csv.ReadRow(columns);
            while (csv.ReadRow(columns))
            {
                int p = 0;

                ErrorCodeData ecd = new ErrorCodeData();
                ErrorResult result = (ErrorResult)Enum.Parse(typeof(ErrorResult), columns[p++]);
                ecd.bPopupRelease = int.Parse(columns[p++]) > 0 ? true : false;
                ecd.bPopupDevelopment = int.Parse(columns[p++]) > 0 ? true : false;
                ecd.message = columns[p++];

                _errorDatas.Add(result, ecd);
            }    
        }


        //국가 코드(2글자) 와 국가명 정보
        using (CsvFile.CsvFileReader csv = new CsvFile.CsvFileReader(GetStringReader("NationCode2")))
        {
            csv.ReadRow(columns);
            while (csv.ReadRow(columns))
            {
                int p = 0;
                string code = columns[p++];
                string nationName = columns[p++];
                _nationNameDatas.Add(code, nationName);
            }
        }


    }




    public ShipInfo GetShipInfo(string shipName, int level)
    {
        Dictionary<int, ShipInfo> dic;
        if (_shipData.TryGetValue(shipName, out dic))
        {
            ShipInfo shipInfo;
            dic.TryGetValue(level, out shipInfo);
            return shipInfo;
        }
        return null;
    }




    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public List<ProductData> GetProductDatas()
    {
        return _productDatas;
    }
    public ProductData GetProductData(int inID)
    {
        ProductData findData = _productDatas.Find(delegate (ProductData pd) {
            return pd.id == inID;
        });
        return null;
    }

    public Vector3[] GetShipPosition(FleetFormationType type)
    {
        Vector3[] positions = null;
        _shipPositionData.TryGetValue(type, out positions);
        return positions;
    }



    public CharacterSkinData GetCharacterSkinData(int avatarID)
    {
        CharacterSkinData data;
        _characterSkinDatas.TryGetValue(avatarID, out data);
        return data;
    }

    public NpcData GetNpcDataFromID(int a_id)
    {
        NpcData data;
        _npcDatas.TryGetValue(a_id, out data);
        return data;
    }

    public NpcData GetNpcDataFromKeyName(string a_namekey)
    {
        Dictionary<int, NpcData>.Enumerator rator = _npcDatas.GetEnumerator();
        while(rator.MoveNext())
        {
            if (rator.Current.Value.nameKey != a_namekey)
            { continue; }
            return rator.Current.Value;
        }
        return null;
    }
    
    public EquipmentGradeData GetWeaponGradeData(WeaponType type, int grade)
    {
        Dictionary<int, EquipmentGradeData> dic;
        if(_weaponGradeDatas.TryGetValue(type, out dic))
        {
            EquipmentGradeData data;
            dic.TryGetValue(grade, out data);
            return data;
        }
        return null;
    }
    public EquipmentLevelData GetWeaponLevelData(WeaponType type, int level)
    {
        Dictionary<int, EquipmentLevelData> dic;
        if (_weaponLevelDatas.TryGetValue(type, out dic))
        {
            EquipmentLevelData data;
            dic.TryGetValue(level, out data);
            return data;
        }
        return null;
    }
    public EquipmentEnhanceData GetWeaponEnhanceData(WeaponType type, int enhance)
    {
        Dictionary<int, EquipmentEnhanceData> dic;
        if (_weaponEnhanceDatas.TryGetValue(type, out dic))
        {
            EquipmentEnhanceData data;
            dic.TryGetValue(enhance, out data);
            return data;
        }
        return null;
    }

    public WeaponSkinData GetWeaponSkinData(int id)
    {
        WeaponSkinData data;
        _weaponSkinDatas.TryGetValue(id, out data);
        return data;
    }


    public EquipmentGradeData GetArmorGradeData(ArmorType type, int grade)
    {
        Dictionary<int, EquipmentGradeData> dic;
        if (_armorGradeDatas.TryGetValue(type, out dic))
        {
            EquipmentGradeData data;
            dic.TryGetValue(grade, out data);
            return data;
        }
        return null;
    }
    public EquipmentLevelData GetArmorLevelData(ArmorType type, int level)
    {
        Dictionary<int, EquipmentLevelData> dic;
        if (_armorLevelDatas.TryGetValue(type, out dic))
        {
            EquipmentLevelData data;
            dic.TryGetValue(level, out data);
            return data;
        }
        return null;
    }
    public EquipmentEnhanceData GetArmorEnhanceData(ArmorType type, int enhance)
    {
        Dictionary<int, EquipmentEnhanceData> dic;
        if (_armorEnhanceDatas.TryGetValue(type, out dic))
        {
            EquipmentEnhanceData data;
            dic.TryGetValue(enhance, out data);
            return data;
        }
        return null;
    }

    public EquipmentGradeData GetSkinGradeData(int grade)
    {
        EquipmentGradeData data;
        _characterSkinGradeDatas.TryGetValue(grade, out data);
        return data;
    }
    public EquipmentLevelData GetSkinLevelData(int level)
    {
        EquipmentLevelData data;
        _characterSkinLevelDatas.TryGetValue(level, out data);
        return data;
    }
    public EquipmentEnhanceData GetSkinEnhanceData(int enhance)
    {
        EquipmentEnhanceData data;
        _characterSkinEnhanceDatas.TryGetValue(enhance, out data);
        return data;
    }
    
    public GemLevelData GetGemLevelData(GemType type, int level)
    {
        Dictionary<int, GemLevelData> dic;
        if(_gemLevelDatas.TryGetValue(type, out dic))
        {
            GemLevelData data;
            dic.TryGetValue(level, out data);
            return data;
        }
        return null;
    }




    public SkillData GetSkillDataByID(int id)
    {
        SkillData data;
        _skillDatas.TryGetValue(id, out data);
        return data;
    }
    public SkillData GetSkillDataByName(string name)
    {
        SkillData data;
        _skillDatasByName.TryGetValue(name, out data);
        return data;
    }




    public List<ServerData> GetServerList(ServerType serverType, int serverGroup)
    {
        List<ServerData> list = new List<ServerData>();
        for(int i=0;i<_serverList.Count; ++i)
        {
            if(_serverList[i].type == serverType && _serverList[i].group == serverGroup)
            {
                list.Add(_serverList[i]);
            }
        }
        return list;
    }






    public ErrorCodeData GetErrorCodeData(ErrorResult a_result)
    {
        ErrorCodeData ecd;
        _errorDatas.TryGetValue(a_result, out ecd);
        return ecd;
    }


    public string GetNationName(string code)
    {
        string name;
        _nationNameDatas.TryGetValue(code, out name);
        return name;
    }





}


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

[Serializable]
public class ServerData
{
    public ServerType type;
    public int group;
    public string name;
    public string address;
    public int port;
}

[Serializable]
public class LevelData
{
    public int level;
    public int exp;

    public CharacterStats stats;
}

[Serializable]
public class MonsterData
{
    public int id;
    public string name;
    public string prefab;
    public string script;
    public bool hasAppearance;
    public int level;
    public int difficulty;
    public int type;

    public CharacterStats stats;
}

[Serializable]
public class SkillData
{
    public int id;
    public string name;
    public WeaponType type;
    public int slotIndex;
    public int requireLevel;
    public int learnPrice;
    public int upgradePrice;
    public int[] itemIds = new int[3];
    public int[] itemBaseCount = new int[3];
    public int[] itemUpgradeCount = new int[3];
    public int[] effects = new int[3];
}

[Serializable]
public class EffectData
{
    public int id;
    public string name;
    public EffectType type;
    public bool isVisible;
}

[Serializable]
public class WeaponData
{
    public int id;
    public WeaponType type;
}

[Serializable]
public class ArmorData
{
    public int id;
    public ArmorType type;
}

[Serializable]
public class CharacterSkinData//AvatarData
{
    public int id;
    public string prefab;
    public string iconName;
    public ColorType colorType;
}

[Serializable]
public class WeaponSkinData//WeaponAvatarData
{
    public int id;
    public string prefab;
    public WeaponType weaponType;
    public int level;
}

//CombinableData







[Serializable]
public class ProductData
{
    public int id;
    public ProductType type;
    public int period;
    public BuyType buyType;
    public string price;
    public int sortNo;
    public string nStoreID;
    public string playStoreID;
    public string appStoreID;
    public string oneStoreID;
    public int rewardID;
    public ItemData item;
}

[Serializable]
public class ItemData
{
    public ItemType type;
    public int itemId;
    public int grade;
    public int count;
    public int ratio;
}





[Serializable]
public class NpcData
{
    public int id;
    public string nameKey;
    public string textKey;
    public string prefab;
    public string icon;
    public string panelName;
}


[Serializable]
public class ErrorCodeData
{
    public bool bPopupRelease;
    public bool bPopupDevelopment;
    public string message;
}



[Serializable]
public class UpgradeRequirement
{
    public int gold;                    //업그래이드의 골드 비용?
    public int[] ids = new int[4];      //업그래이드의 아이디는 뭐지?
    public int[] counts = new int[4];   //업그래이드의 카운트는 뭐지?
}

[Serializable]
public class EquipmentGradeData
{
    public int grade;
    public CharacterStats stats;
    public UpgradeRequirement upgradeRequirement;
    public int maxLevel;
    public int maxEnhance;
}
[Serializable]
public class EquipmentLevelData
{
    public int level;
    public CharacterStats stats;
    public UpgradeRequirement upgradeRequirement;
}
[Serializable]
public class EquipmentEnhanceData
{
    public int enhance;
    public CharacterStats stats;
    public UpgradeRequirement upgradeRequirement;
    public int enhanceChance;	// 0 ~ 1000
}

[Serializable]
public class GemLevelData
{
    public int level;
    public CharacterStats stats;
    public UpgradeRequirement upgradeRequirement;
}




public class DungeonClearInfo
{
    public int id;
    public bool hpClear;            // HP에 의한 별 획득 여부
    public bool timeClear;          // 클리어시간에 의한 별 획득 여부
}
public class DungeonStateInfo
{
    public int id;
    public int startCount;
    public bool prevDungeonClear;   //이전에 클리어 기록 있는지?
    public bool isFieldOpen;
    public DungeonState state;
    public DungeonClearInfo clearInfo;

    public void Clear()
    {
        id = 0;
        startCount = 0;
        prevDungeonClear = false;
        isFieldOpen = false;
        state = DungeonState.Locked;
        clearInfo = null;
    }
}



