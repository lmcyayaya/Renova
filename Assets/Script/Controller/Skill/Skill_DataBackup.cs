using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Skill_DataBackup : MonoBehaviour
{
    Vector3 save;
    bool canUsed = true;
    public bool canTrigger = false;
    float duration = 5;
    float skillDelay = 1;
    float coolTime = 1;
    Transform m_player;
    public SkinnedMeshRenderer body;
    public void InitSkill(Transform player)
    {
        if(!canUsed)
            return;
        save = player.position;
        m_player = player;
        StartCoroutine(SkillDelay());
        StartCoroutine(SkillCoolTime());
        AfterImagePool.Instance.StayImage();
        canUsed = false;
        Debug.Log("Start Init Skill");
    }
    public void StartSkill()
    {
        Debug.Log("StartSkill");
        ShowBody(false);
        AfterImagePool.Instance.StayCrossImage();
        StopCoroutine("SkillDuration");
        
        m_player.DOMove(save, 1f).SetEase(Ease.InExpo).OnComplete(()=>EndSkill());

        
    }
    public void EndSkill()
    {
        ShowBody(true);
        canTrigger = false;
        AfterImagePool.Instance.CloseImage();
        
    }
    public IEnumerator SkillDelay()
    {
        yield return new WaitForSeconds(skillDelay);
        Debug.Log("Start Skill");
        canTrigger = true;
        StartCoroutine(SkillDuration());
    }
    public IEnumerator SkillDuration()
    {
        yield return new WaitForSeconds(duration);
        if(canTrigger)
        {
            Debug.Log("End Skill");
            canTrigger = false;
            AfterImagePool.Instance.CloseImage();
        }
        
    }
    public IEnumerator SkillCoolTime()
    {
        yield return new WaitForSeconds(coolTime);
        Debug.Log("CoolTime Reset");
        canUsed = true;
    }
    void ShowBody(bool state)
    {
        body.enabled = state;
    }
}
