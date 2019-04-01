using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EnemysHPBar : MonoBehaviour
{
    EnemysData eneData;
    public Image HpBar;
    void Start()
    {
        eneData = transform.parent.GetComponent<EnemysData>();
        //HpBar = GetComponent<Image>();
    }
    void Update()
    {
        HpBar.fillAmount = eneData.currentHP/eneData.maxHP;
        Vector3 v = Camera.main.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(Camera.main.transform.position - v);
        transform.Rotate(0, 180, 0);
    
    }
}
