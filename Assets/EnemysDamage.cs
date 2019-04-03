using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemysDamage : MonoBehaviour
{
    public CharacterDatabase charData;
    public GameObject player;
    private Vector3 scale;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }
    private void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag =="Bullet")
        {
            var damageText = ObjectPool.TakeFormPool("DamageText");
            ContactPoint contact = col.contacts[0];
            damageText.transform.position = contact.point;
            damageText.transform.position += new Vector3(Random.Range(-0.3f,0.3f),Random.Range(0.1f,0.3f),0);
            damageText.GetComponent<DamageText>().moveCheck =true;
            damageText.GetComponent<DamageText>().end = damageText.transform.position +new Vector3(0,0.3f,0);
            float damage = charData.ATK +Random.Range(-2,2);
            damageText.GetComponent<TextMeshPro>().text = damage.ToString();
            damageText.GetComponent<TextMeshPro>().fontSize = (Vector3.Distance(this.transform.position,charData.charPos)/5*3);
            this.gameObject.GetComponent<EnemysData>().currentHP -= damage;
        }
    }
}
