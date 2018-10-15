using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelItemManageFleetShipInfo : MonoBehaviour
{
    private ShipInfo _info;

    public UISprite shipImage;
    
    public UILabel beamType;
    public UILabel missleType;
    public UILabel fighterType;
    public UILabel shieldType;
    public UILabel defence;
    public UILabel hp;
    public UILabel moveSpeed;
    public UILabel rotateSpeed;

    public UILabel shipName;
    public UILabel shipLevel;
    public UILabel link;

    public GameObject spriteGroup;
    public GameObject infoGroup;
    
    public void SetInfo(ShipInfo inInfo)
    {
        _info = inInfo;

        //shipImage.spriteName = "";

        beamType.text = "Beam: " + inInfo.battleInfo.beamType + ", Num: " + inInfo.battleInfo.beamGunCount.ToString();
        missleType.text = "Missle: " + inInfo.battleInfo.missleType + ", Num: " + inInfo.battleInfo.missleGunCount.ToString();
        fighterType.text = "Fighter: " + inInfo.battleInfo.fighterType + ", Num: " + inInfo.battleInfo.fighterCountCur.ToString() + " / " + inInfo.battleInfo.fighterCountMax.ToString();
        shieldType.text = "Shield: " + inInfo.battleInfo.shieldType.ToString();
        defence.text = "Defence: " + inInfo.battleInfo.defence.ToString();
        hp.text = "hp: " + inInfo.battleInfo.hpCur.ToString() + " / " + inInfo.battleInfo.hpMax.ToString();
        moveSpeed.text = "Move: " + inInfo.battleInfo.moveSpeed.ToString();
        rotateSpeed.text = "Rotate: " + inInfo.battleInfo.rotateSpeed.ToString();

        shipName.text = inInfo.name;
        shipLevel.text = "Grade: " + inInfo.grade.ToString();
        link.text = "Link: " + inInfo.linkCur.ToString() + " / " + inInfo.linkMax.ToString();
        
    }

    public void VisibleUI(bool bVisible)
    {
        spriteGroup.SetActive(bVisible);
        infoGroup.SetActive(bVisible);
    }



}
