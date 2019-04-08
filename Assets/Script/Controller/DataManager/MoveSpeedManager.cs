using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSpeedManager : MonoBehaviour
{
    private StateManager states;
    private BaseData baseData;
    private ProcessedData processedData;
    public void Init(GameObject player)
    {
        baseData = this.gameObject.GetComponent<BaseData>();
        processedData = this.gameObject.GetComponent<ProcessedData>();
        states = player.GetComponent<StateManager>();
    }
    void Update()
    {
        processedData.moveSpeed= BaseSpeed() * baseData.atkModeData.moveSpeed ;
    }
    private float BaseSpeed()
    {
        if(states.run == true)
            return baseData.runSpeed;
        else
            return baseData.walkSpeed;
    }
}
