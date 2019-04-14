using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedManager : MonoBehaviour
{
    private StateManager states;
    private BaseData baseData;
    //private ProcessedData processedData;
    public void Init(StateManager stat)
    {
        baseData = gameObject.GetComponent<BaseData>();
        //processedData = gameObject.GetComponent<ProcessedData>();
        states = stat;
    }
    void Update()
    {
        ProcessedData.Instance.moveSpeed= BaseSpeed() * baseData.atkModeData.moveSpeed ;
    }
    private float BaseSpeed()
    {
        if(states.run == true)
            return baseData.runSpeed;
        else
            return baseData.walkSpeed;
    }
}
