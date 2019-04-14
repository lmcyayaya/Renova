using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ATKMode : MonoBehaviour
{
    BaseData baseData;
    void Start()
    {
        baseData = BaseData.Instance;
    }
    void Update()
    {
        this.gameObject.GetComponent<Text>().text = baseData.atkModeData.modeName;
    }
}
