using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool instence = null;
    public static ObjectPool Instence
    {
        get {return instence;}
    }
        

    [Header("Seeting")]
    public ObjPoolSetting[] objPool;

    //local use
    private static Dictionary<string, ObjPoolCounter> objPoolCounter = new Dictionary<string, ObjPoolCounter>();
    private static Dictionary<string, Dictionary<GameObject,bool>> poolObjStatus = new Dictionary<string, Dictionary<GameObject,bool>>();
    private static Dictionary<GameObject, string> poolObjList = new Dictionary<GameObject, string>();
    private static Dictionary<string, Transform> poolParentList = new Dictionary<string, Transform>();

    private void Awake()
    {
        if(instence ==null)
            instence = this;
        CraetPoolObject();
    }

    private void CraetPoolObject()
    {
        GameObject newobj = null;
        if(objPool.Length == 0) return;

        foreach(ObjPoolSetting ops in objPool)
        {
            GameObject pool = new GameObject(ops.name);
            pool.transform.SetParent(transform);
            objPoolCounter.Add(ops.name,new ObjPoolCounter(ops.Quantity,0,ops.Quantity));
            poolObjStatus.Add(ops.name,new Dictionary<GameObject,bool>());
            poolParentList.Add(ops.name,pool.transform);

            for(int i = 1; i<=ops.Quantity; i++)
            {
                newobj = Instantiate(ops.prefab,pool.transform);
                newobj.SetActive(ops.enableInPool);
                newobj.transform.position = transform.position;
                newobj.name = string.Concat(ops.name,"(" , i , ")");
                poolObjStatus[ops.name].Add(newobj,false);
                poolObjList.Add(newobj,ops.name);
            }

        }
    }

    public static Transform TakeFormPool(string pool)
    {
        if(objPoolCounter[pool].inObj == 0)
            return null;

        Transform t =null;
        Dictionary<GameObject,bool> objList = poolObjStatus[pool];
        foreach(GameObject g in objList.Keys)
        {
            if(!objList[g])
            {
                objList[g] = true;
                g.SetActive(true);
                t = g.transform;
                break;
            }
        }
        
        objPoolCounter[pool].Take();

        return t;

    }
    public static void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        string p = poolObjList[obj];
        poolObjStatus[p][obj] = false;
        obj.transform.SetParent(instence.transform);
        obj.transform.SetPositionAndRotation(instence.transform.position,instence.transform.rotation);

        objPoolCounter[p].Return();

    }



}
