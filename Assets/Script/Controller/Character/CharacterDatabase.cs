using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    
public class CharacterDatabase : MonoBehaviour
{
    public float HP;
    public float EP;
    public int Overclock_Level;
    public float moveSpeed;
    public float dodgeTime;
    public float ATK = 10;
    public float DEF;
    public float SP;//Stunned Power Point
    public float shootingSpeed;

    public Vector3 charPos;
    private GameObject player;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }
    private void FixedUpdate()
    {
        charPos= player.transform.position;
    }
}

