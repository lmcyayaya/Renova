using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public class EnemyTarget : MonoBehaviour
    {

        void Start()
        {
        }

        public Transform GetTarget()
        {
            return this.gameObject.transform;
        }


    }

