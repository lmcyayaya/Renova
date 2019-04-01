using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemysDamage : MonoBehaviour
{
    public CharacterDatabase charData;
    private void OnCollisionEnter(Collision col)
    {
        if(col.collider.tag =="Bullet")
        {
            var damageText = ObjectPool.TakeFormPool("DamageText");
            damageText.SetParent(this.transform);
            damageText.transform.position = this.transform.position;
            damageText.transform.localPosition = new Vector3(Random.Range(-0.5f,0.5f),Random.Range(1.5f,3f),0);
            float damage = charData.ATK +Random.Range(-2,2);
            damageText.GetComponent<TextMeshPro>().text = damage.ToString();
            this.gameObject.GetComponent<EnemysData>().currentHP -= damage;
        }
    }
}
