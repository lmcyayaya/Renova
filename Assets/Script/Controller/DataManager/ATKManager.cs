using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATKManager : MonoBehaviour
{
    private StateManager states;
    private BaseData baseData;
    private ProcessedData processedData;
    private SpawnProjectiles spawnProjectiles;
    public void Init()
    {
        baseData = this.gameObject.GetComponent<BaseData>();
        processedData = this.gameObject.GetComponent<ProcessedData>();
        spawnProjectiles = GameObject.FindGameObjectWithTag("Gun").GetComponent<SpawnProjectiles>();
    }
    private void Update()
    {
        processedData.ATK =(10*baseData.atkModeData.ATK)*spawnProjectiles.damagePlus * spawnProjectiles.chargeLevel +Random.Range(-2,2);
        if(processedData.ATK<0)
            processedData.ATK = 0;
    }
    public void CalculateATK()
    {
        processedData.ATK =(10*baseData.atkModeData.ATK)*spawnProjectiles.damagePlus * spawnProjectiles.chargeLevel +Random.Range(-2,2);
        if(processedData.ATK<0)
            processedData.ATK = 0;
    }
}
