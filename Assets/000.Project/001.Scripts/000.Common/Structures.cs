using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;



/// <summary>
/// 서버에서 받는 글로벌 정보
/// </summary>
public class ServerGlobalValues
{
    public int goldGambleCost = 0;
}

public class ProductInfo           //현금 구매 아이템 정보
{
    public int productID;
    public DateTime startTime;
    public DateTime endTime;
    public string imgUrl = string.Empty;
    public StoreType storeType;
    public ProductType productType;

    public virtual ProductInfo Clone()
    {
        ProductInfo data = new ProductInfo();
        data.productID = this.productID;
        data.startTime = this.startTime;
        data.endTime = this.endTime;
        data.imgUrl = this.imgUrl;
        data.storeType = this.storeType;
        data.productType = this.productType;
        return data;
    }

    public virtual ProductType GetProductType()
    {
        return productType;
    }

}
public class ProductInfo_Package : ProductInfo
{
    public bool bPurchased;
    public override ProductInfo Clone()
    {
        ProductInfo_Package data = new ProductInfo_Package();
        data.productID = this.productID;
        data.startTime = this.startTime;
        data.endTime = this.endTime;
        data.imgUrl = this.imgUrl;
        data.storeType = this.storeType;
        data.productType = this.productType;
        data.bPurchased = this.bPurchased;//<-
        return data;
    }
    public ProductInfo_Package()
    {
        productType = ProductType.Package;
    }

}

public class FriendInfo
{
    public long usn;
    public string name = "";
    public int avataID;
    public int skinID;

    public int level;
    public bool isOnline = false;
    public DateTime lastLoginTime;
    public DateTime lastQuestSendTime;

    public string socialID = "";                   //인증 여부를 확인하는 user의 id로, 게임 회원번호
    public string socialThumnailURL = "";
    public string socialName = "";

    public string facebookID = "";
    public string facebookThumnailURL = "";
    public string facebookName = "";
    
    //public string kakaoUUID;
    //public bool kakaoisAppRegistered;      //앱등록 여부
    //public string kakaotalkOs;
    //public bool kakaoisAllowedMsg;
    
    public string nationType = string.Empty;
}



//bk 확인

public class ItemInfo
{
    public virtual ItemType GetItemType()
    { return ItemType.None; }

    public virtual int GetItemId()
    { return 0; }

    public virtual void Write()
    {
        //프라우드넷 을 이용하는데 ...
    }

    public virtual void Read()
    {
        //프라우드넷 을 이용하는데 ...
    }

    public bool IsEquipable()
    {
        switch(GetItemType())
        {
            case ItemType.Weapon:
            case ItemType.Armor:
            case ItemType.Skin:
            case ItemType.WeaponSkin:
                return true;
        }
        return false;
    }
    
}
public class WeaponInfo : ItemInfo
{
    public long uid;
    public int id;
    public WeaponType type;
    public int grade;
    public int level;
    public int enhance;
    public int[] gemLevels = new int[(int)GemType.Max];
    public ushort enhanceFailCount;

    public override ItemType GetItemType() { return ItemType.Weapon; }
    public override int GetItemId() { return id; }
    public override void Write()
    {
        //프라우드넷 을 이용하는데 ...
    }
    public override void Read()
    {
        //프라우드넷 을 이용하는데 ...
    }


}
public class ArmorInfo : ItemInfo
{
    public long uid;
    public int id;
    public ArmorType type;
    public int grade;
    public int level;
    public int enhance;
    public ushort enhanceFailCount;

    public override ItemType GetItemType() { return ItemType.Armor; }
    public override int GetItemId() { return id; }
    public override void Write()
    {
        //프라우드넷 을 이용하는데 ...
    }
    public override void Read()
    {
        //프라우드넷 을 이용하는데 ...
    }



}
public class SkinInfo : ItemInfo
{
    public long uid;
    public int id;
    public int grade;
    public int level;
    public int enhance;
    public ushort enhanceFailCount;

    public override ItemType GetItemType() { return ItemType.Skin; }
    public override int GetItemId() { return id; }
    public override void Write()
    {
        //프라우드넷 을 이용하는데 ...
    }
    public override void Read()
    {
        //프라우드넷 을 이용하는데 ...
    }
}
public class WeaponSkinInfo : ItemInfo
{
    public long uid;
    public int id;
    public bool equip;      //  장착여부 ( true or false )

    public override ItemType GetItemType() { return ItemType.WeaponSkin; }
    public override int GetItemId() { return id; }
    public override void Write()
    {
        //프라우드넷 을 이용하는데 ...
    }
    public override void Read()
    {
        //프라우드넷 을 이용하는데 ...
    }
}

/// <summary>
/// 재화 정보
/// </summary>
public class AssetInfo : ItemInfo
{
    public int id;
    public AssetType type;//재화의 종류
    /// <summary>
    /// 기본 수, 맥스 이상 불가
    /// </summary>
    public int curCount;
    /// <summary>
    /// curCount와 별개로 존재하는 수, 맥스값 이상으로 쌓아둘 수 있는 개념
    /// </summary>
    public int extraCount;
    /// <summary>
    /// curCount 의 최대값
    /// </summary>
    public int maxCount;
    /// <summary>
    /// curCount + extraCount
    /// </summary>
    public int totalCount;

    public int refreshDelay;
    public DateTime lastChageTime;
    public DateTime nextChageTime;
    /// <summary>
    /// 구매 횟수
    /// </summary>
    public int buyCount;

    public override ItemType GetItemType()
    {
        return ItemType.Rechargeable;
    }

    public override int GetItemId()
    {
        return id;
    }

    public virtual void Write()
    {
        //프라우드넷 을 이용하는데 ...
    }

    public virtual void Read()
    {
        //프라우드넷 을 이용하는데 ...
    }

    public int GetTotalCount()
    {
        totalCount = curCount + extraCount;
        return totalCount;
    }


}






[Serializable]
public class CharacterStats
{
    public int attackPower;
    public int penetration;     //관통
    public int critical;
    public int accuracy;
    public int hp;
    public int defence;
    public int criticalDefence;
    public int evasion;
    
    public CharacterStats(){}

    public CharacterStats(CharacterStats rhs)
    {
        attackPower = rhs.attackPower;
        penetration = rhs.penetration;
        critical = rhs.critical;
        accuracy = rhs.accuracy;
        hp = rhs.hp;
        defence = rhs.defence;
        criticalDefence = rhs.criticalDefence;
        evasion = rhs.evasion;
    }

    public static CharacterStats operator +(CharacterStats lhs, CharacterStats rhs)
    {
        CharacterStats rvalue = new CharacterStats();

        rvalue.attackPower = lhs.attackPower + rhs.attackPower;
        rvalue.penetration = lhs.penetration + rhs.penetration;
        rvalue.critical = lhs.critical + rhs.critical;
        rvalue.accuracy = lhs.accuracy + rhs.accuracy;
        rvalue.hp = lhs.hp + rhs.hp;
        rvalue.defence = lhs.defence + rhs.defence;
        rvalue.criticalDefence = lhs.criticalDefence + rhs.criticalDefence;
        rvalue.evasion = lhs.evasion + rhs.evasion;

        return rvalue;
    }

    public static CharacterStats operator -(CharacterStats lhs, CharacterStats rhs)
    {
        CharacterStats rvalue = new CharacterStats();

        rvalue.attackPower = lhs.attackPower - rhs.attackPower;
        rvalue.penetration = lhs.penetration - rhs.penetration;
        rvalue.critical = lhs.critical - rhs.critical;
        rvalue.accuracy = lhs.accuracy - rhs.accuracy;
        rvalue.hp = lhs.hp - rhs.hp;
        rvalue.defence = lhs.defence - rhs.defence;
        rvalue.criticalDefence = lhs.criticalDefence - rhs.criticalDefence;
        rvalue.evasion = lhs.evasion - rhs.evasion;

        return rvalue;
    }
}




public class SkillInfo
{
    public int id;
	public int level;

    public void Write()
    {
        //프라우드넷 을 이용하는데 ...
    }


    public void Read()
    {
        //프라우드넷 을 이용하는데 ...
    }

    public SkillInfo()
    {
        id = 0;
        level = 0;
    }

    public SkillInfo(int skillId, int lv)
    {
        id = skillId;
        level = lv;
    }
}


public class SocialIDInfo
{
    public string facebook_id = "";
    public string facebook_token = "";
    public string google_id = "";
    public string kakao_id = "";
    public string kakao_token = "";
}



public class BlockedAccountInfo
{
    public BlockType blockType = BlockType.None;
    public BlockStatus status = BlockStatus.Clear;
    public DateTime endTime = DateTime.Now;
    public string comment = "";
}



