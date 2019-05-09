using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_LockOn : MonoBehaviour
{
    public CameraManager cameraManager;
    [Header("Canvas")]
    public Image aim;
    public Image lockAim;
    public Vector2 uiOffset;
    private bool show;
    void Start()
    {
        //cameraManager = Camera.main.transform.root.GetComponent<CameraManager>();
    }
    void Update()
    {
        if(cameraManager.lockonTarget!=null)
            aim.transform.position =Camera.main.WorldToScreenPoint(cameraManager.lockonTarget.transform.position + (Vector3)uiOffset);
        if(cameraManager.lockonTarget!=null && !show)
        {
            LockInterface(true);
            show = true;
        }
        else if(cameraManager.lockonTarget==null && show)
        {
            LockInterface(false);
            show = false;
        }
    }
    void LockInterface(bool state)
    {
        aim.gameObject.SetActive(state);
        lockAim.gameObject.SetActive(state);
        float size = state ? 1 : 2;
        float fade = state ? 1 : 0;
        lockAim.DOFade(fade, .15f);
        lockAim.transform.DOScale(size, .15f).SetEase(Ease.OutBack);
        lockAim.transform.DORotate(Vector3.forward * 180, .15f, RotateMode.FastBeyond360).From();
        aim.transform.DORotate(Vector3.forward * 90, .15f, RotateMode.LocalAxisAdd);
    }
}
