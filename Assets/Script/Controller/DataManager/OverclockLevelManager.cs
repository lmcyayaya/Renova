using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverclockLevelManager : MonoBehaviour
{
    private static OverclockLevelManager instance = null;
    public static OverclockLevelManager Instance
    {
        get {return instance;}
    }
    public float reduceSpeed;
    public float olCharge;
    public int overclockLevel;
    public Image level1;
    public Image level2;
    public Image level3;

    private void Update()
    {
        OverclockLevelCounter();
        OverlockLevelUI();
    }
    private void OverclockLevelCounter()
    {
        
        if(olCharge <= 0 )
        {
            if(overclockLevel >1)
            {
                olCharge = 99;
                overclockLevel -=1;
            }
            else
            {
                olCharge = 0;
            }
        }
        else if(olCharge >= 100)
        {
            if(overclockLevel<3)
            {
                olCharge = 1;
                overclockLevel +=1;
            }
            else
            {
                olCharge = 99;
            }
        }
        else
        {
            olCharge -= reduceSpeed*Time.deltaTime;
        }
    }
    private void OverlockLevelUI()
    {
        if(overclockLevel==3)
        {
            level1.fillAmount = 1;
            level2.fillAmount = 1;
            level3.fillAmount = olCharge/100;
        }
        else if(overclockLevel==2)
        {
            level1.fillAmount = 1;
            level2.fillAmount = olCharge/100;
            level3.fillAmount =0;
        }
        else
        {
            level1.fillAmount = olCharge/100;
            level2.fillAmount = 0;
            level3.fillAmount = 0;
        }
    }
}
