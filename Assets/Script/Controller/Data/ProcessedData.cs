using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessedData : MonoBehaviour
{
    private static ProcessedData instance = null;
    public static ProcessedData Instance
    {
        get {return instance;}
    }
    public float ATK;
    public float moveSpeed;
    void Awake()
    {
        instance =this;
    }
}
