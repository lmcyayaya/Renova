using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemysDamage : MonoBehaviour
{
    ProcessedData processedData;
    private GameObject player;
    private GameObject gm;
    private Vector3 scale;
    private float distance {get{return Vector3.Distance(this.transform.position,player.transform.position);}}
    private void Start()
    {
        gm = GameObject.FindWithTag("GM");
        player = GameObject.FindWithTag("Player");
        processedData = gm.GetComponent<ProcessedData>();
    }
    private void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag =="Bullet")
        {
            var damageText = ObjectPool.TakeFormPool("DamageText");
            ContactPoint contact = col.contacts[0];
            damageText.transform.position = contact.point;
            damageText.transform.position += new Vector3(Random.Range(distance/5*-0.3f,distance/5*0.3f),Random.Range(distance/5*0.1f,distance/5*0.3f),0);
            damageText.GetComponent<DamageText>().moveCheck =true;
            damageText.GetComponent<DamageText>().end = damageText.transform.position +new Vector3(0,0.3f,0);
            damageText.GetComponent<TextMeshPro>().text = col.gameObject.GetComponent<BulletMove>().Damage.ToString();
            damageText.GetComponent<TextMeshPro>().fontSize = (distance/5*3);
            this.gameObject.GetComponent<EnemysData>().currentHP -= col.gameObject.GetComponent<BulletMove>().Damage;
        }
    }
}
