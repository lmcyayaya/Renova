using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATKManager : MonoBehaviour
{
    private SpawnProjectiles spawnProjectiles;
    public void Init()
    {

        spawnProjectiles = GameObject.FindGameObjectWithTag("Gun").GetComponent<SpawnProjectiles>();
    }
    private void Update()
    {
        ProcessedData.Instance.ATK =(10*BaseData.Instance.atkModeData.ATK)*spawnProjectiles.damagePlus * spawnProjectiles.chargeLevel +Random.Range(-2,2);
        if(ProcessedData.Instance.ATK<0)
            ProcessedData.Instance.ATK = 0;
    }
    public void CalculateATK()
    {
        
        ProcessedData.Instance.ATK =(10*BaseData.Instance.atkModeData.ATK)*spawnProjectiles.damagePlus * spawnProjectiles.chargeLevel +Random.Range(-2,2);
        if(ProcessedData.Instance.ATK<0)
            ProcessedData.Instance.ATK = 0;
    }
}
