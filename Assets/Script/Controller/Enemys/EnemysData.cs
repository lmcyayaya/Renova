using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysData : MonoBehaviour
{
    public float maxHP;
    public float currentHP;
    private void Update()
    {
        if(currentHP <0)
        {
            currentHP = maxHP;
        }
    }
    private void Start()
    {
        currentHP = maxHP;
    }

}
