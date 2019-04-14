using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShootCount : MonoBehaviour
{
    public SpawnProjectiles spawnProjectiles;
    // Update is called once per frame
    void Update()
    {
        this.gameObject.GetComponent<Text>().text = spawnProjectiles.shootCount.ToString();
    }
}
