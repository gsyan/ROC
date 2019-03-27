using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelItemManageFleetShipInfo : MonoBehaviour
{
    private ShipInfo _info;

    public Image shipImage;
    
    public Text beamType;
    public Text missleType;
    public Text fighterType;
    public Text shieldType;
    public Text defence;
    public Text hp;
    public Text moveSpeed;
    public Text rotateSpeed;

    public Text shipName;
    public Text shipLevel;
    public Text link;

    public GameObject spriteGroup;
    public GameObject infoGroup;
    
    public void SetInfo(ShipInfo inInfo)
    {
        _info = inInfo;

        //shipImage.spriteName = "";

        beamType.text = "Beam: " + inInfo.attackInfo.beamType + ", Num: " + inInfo.attackInfo.beamGunCount.ToString();
        missleType.text = "Missle: " + inInfo.attackInfo.missleType + ", Num: " + inInfo.attackInfo.missleGunCount.ToString();
        fighterType.text = "Fighter: " + inInfo.attackInfo.fighterType + ", Num: " + inInfo.attackInfo.fighterCountCur.ToString() + " / " + inInfo.attackInfo.fighterCountMax.ToString();
        shieldType.text = "Shield: " + inInfo.defenceInfo.shieldType.ToString();
        defence.text = "Defence: " + inInfo.defenceInfo.defence.ToString();
        hp.text = "hp: " + inInfo.hpCur.ToString() + " / " + inInfo.hpMax.ToString();
        moveSpeed.text = "Move: " + inInfo.moveInfo.moveSpeed.ToString();
        rotateSpeed.text = "Rotate: " + inInfo.moveInfo.rotateSpeed.ToString();

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
