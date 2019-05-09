using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public Transform player;
    public InputHandler inputHandler;
    public Skill_DataBackup skill_DataBackup;
    void Update()
    {
        if(inputHandler.o_input)
        {
            Debug.Log("Button Down");
            skill_DataBackup.InitSkill(player);
            
        }
    }
    public IEnumerator DataBackup()
    {
        yield return null;
    }
}
