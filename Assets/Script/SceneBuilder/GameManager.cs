using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get {return instance;}
    }
    public GameObject player; 
    private void Awake()
    {
        if(instance ==null)
            instance = this;
    }
}
