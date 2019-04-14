using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ChargeTime : MonoBehaviour
{
    public SpawnProjectiles spawnProjectiles;
    Image chargeImage;
    Text chargeLevel;

    void Start()
    {
        chargeImage = this.gameObject.GetComponent<Image>();
        chargeLevel=this.gameObject.transform.GetChild(0).GetComponent<Text>();
    }

    void Update()
    {
        chargeImage.fillAmount = spawnProjectiles.chargeTime/3;
        if(spawnProjectiles.chargeTime<1)
            chargeLevel.text = "0";
        else
            chargeLevel.text = spawnProjectiles.chargeLevel.ToString();
    }
}
