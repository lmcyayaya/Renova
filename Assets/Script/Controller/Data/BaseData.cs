using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseData : MonoBehaviour
{
    private static BaseData instance = null;
    public static BaseData Instance
    {
        get {return instance;}
    }
    private GameObject player;
    private MoveSpeedManager msManager;
    private ATKManager atkManager;
    private InputHandler inputHandler;
    [SerializeField]
    private AttackModeData[] amData;
    public AttackModeData atkModeData
    {
        get
        {   
            return amData[order];
        }
    }
    private int order = 0;

    [Header("Stats")]
    public float walkSpeed;
    public float runSpeed;
    public float rollSpeed;
    public float jumpSpeed;
    public float rotateSpeed;
    public float toGround;
    public float invincibleTime;
    public float perfectDodgeTime;

    public bool changeMode;
    private float changeTimer;
    private float changeModeCoolTime =1.5f;
    void Awake()
    {
        instance = this;
    }
    public void Init(InputHandler input)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        inputHandler = input;
        msManager = this.gameObject.GetComponent<MoveSpeedManager>();
        msManager.Init(input.states);
        atkManager = this.gameObject.GetComponent<ATKManager>();
        atkManager.Init();
    }
    private void Update()
    {
        ChangeAttackMode();
    }
    private void ChangeAttackMode()
    {
        changeMode = inputHandler.up_input ||inputHandler.down_input;
        changeTimer += Time.deltaTime;
        if(!changeMode)
            return;
        if(inputHandler.up_input && changeTimer > changeModeCoolTime)
        {
            order += 1;
            if(order > 2)
                order = 0;
            changeTimer = 0;
        }
        else if(inputHandler.down_input && changeTimer > changeModeCoolTime)
        {
            order -= 1;
            if(order < 0)
                order = 2;
            changeTimer = 0;
        }
    }
}
