using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AfterImagePool : MonoBehaviour
{

    //public CharacterControl myCharacterControl;
    public GameObject targetObject;     //Set these manually to the character object you're copying
    public Animator targetAnimator;   //Set these manually to the character object you're copying
    public GameObject prefab;           //This is the prefab you made in the scene. It's a parent transform with an animator and AfterImage script on it, with Armature and SkinnedMeshRenderer children
    public int poolSize = 10;
    public List<AfterImage> afterImages;

    public float interval = 10;

    public float time = 0;
    public StateManager state;
    string [] dir = {"forward","back","left","right"};

    // Use this for initialization
    void Start()
    {
        //myCharacterControl = transform.root.GetComponent<CharacterControl>();
        //Debug.Log("START AFTER IMAGE POOL");
        afterImages = new List<AfterImage>(poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            GameObject nextAfterImage = Instantiate(prefab);
            afterImages.Add(nextAfterImage.GetComponent<AfterImage>());

            nextAfterImage.GetComponent<AfterImage>().targetObject = targetObject;      //Game Object Target
            nextAfterImage.GetComponent<AfterImage>().targetAnimator = targetAnimator;     //Animator Target
        }
    }
    void Update()
    {
        //if (CitadelDeep.hitPause > 0 || CitadelDeep.debugPause) { return; }
        // time+=Time.deltaTime;
        // if (time > interval) { time = 0; AddAfterImage(); }
    }

    
    void AddAfterImage()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (!afterImages[i].active)
            { 
                afterImages[i].Activate();
                break; 
            }
        }
    }
    public IEnumerator  AddDodgeImage()
    {
        yield return new WaitForSeconds(0.02f);
        for (int i = 0; i < poolSize; i++)
        {
            afterImages[i].gameObject.SetActive(true);
            afterImages[i].dir = dir[i];
            afterImages[i].DodgeImage();
        }
    }
    // public void AddDodgeImage()
    // {
    //     for (int i = 0; i < poolSize; i++)
    //     {
    //         afterImages[i].gameObject.SetActive(true);
    //         afterImages[i].dir = dir[i];
    //         afterImages[i].DodgeImage();
    //     }
    // }
}