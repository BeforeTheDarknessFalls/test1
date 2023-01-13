using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    public int PartID;

    Animator Ani;
    public float showTime = 0.1f;
    float wTime = 0;
    Action<int> callBack = null;
    public bool isLoad;
    // Start is called before the first frame update
    void Start()
    {
        if(Ani == null){
            InitAni();
        }   
    }

    void InitAni()
    {
        Ani = GetComponent<Animator>();
        if (Ani != null)
        {
            Ani.enabled = false;
        }
    }

    public void RunAni(Action<int> callback)
    {
        if (Ani == null)
        {
            InitAni();
        }
        if (Ani != null)
        {
            Ani.enabled = true;
        }
        this.callBack = callback;
        wTime = showTime;
        StartCoroutine("Check_Ani");
    }
    
    IEnumerator Check_Ani()
    {
        while (true)
        {
            wTime -= Time.deltaTime;
            if (wTime <= 0)
            {
                ShowEnd();
                yield break;
            }
            yield return null;
        }
     
    }

    void ShowEnd()
    {
        if (callBack != null)
        {
            callBack(PartID);
        }
    }

    public void ShowNext()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
