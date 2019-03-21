using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleHealthBar : MonoBehaviour
{
    public Image roundImage;
    public RectTransform pointImageParent;

    public float healthValue = 0;
	
	// Update is called once per frame
	void Update () {
        HealthChange(healthValue);
    }

    void HealthChange(float value)
    {
        float amount = (value / 100.0f);
        roundImage.fillAmount = amount;
        float angle = amount * 360.0f;
        pointImageParent.localEulerAngles = new Vector3(0, 0, -angle);
    }

}
