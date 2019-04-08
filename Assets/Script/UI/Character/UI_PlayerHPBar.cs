using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_PlayerHPBar : MonoBehaviour
{
    // Start is called before the first frame update
    StateManager state;
    public Image HpBar;
    void Start()
    {
        state = transform.parent.GetComponent<StateManager>();
        //HpBar = GetComponent<Image>();
    }
    void Update()
    {
        Vector3 v = Camera.main.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(Camera.main.transform.position - v);
        transform.Rotate(0, 180, 0);
    
    }
}
