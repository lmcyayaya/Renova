using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ObjPoolSetting
{
    public string name;
    public GameObject prefab;
    [Range(20, 1000)]
    public int Quantity;
    public bool enableInPool;
}
public struct ObjPoolCounter
{
    public int totalObj;
    public int outObj;
    public int inObj;
    public ObjPoolCounter(int totalObj,int outObj,int inObj)
    {
        this.totalObj = totalObj;
        this.outObj = outObj;
        this.inObj = inObj;
    }
    public void Take()
    {
        outObj++;
        inObj--;
    }
    public void Return()
    {
        outObj--;
        inObj++;
    }

}