using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Part : MonoBehaviour
{
    public int PartID;

    Animator Ani;
    public float showTime = 0.1f;
    float wTime = 0;
    Action<int> callBack = null;
    public bool isLoad;
    public Image[] imgs;
    // Start is called before the first frame update
    void Start()
    {
        if(Ani == null){
            InitAni();
        }   
    }
    /// <summary>
    /// 设置图片
    /// </summary>
    /// <param name="texs">所有图片</param>
    /// <param name="index">当前已设置的位置</param>
    /// <returns></returns>
    public int SetPictures(List<Texture2D> texs,int index)
    {
        if (imgs == null) return index;
        if (imgs.Length==0) return index;
        for (int i=0;i< imgs.Length;i++)
        {
            int temp = index + 1;
            if (temp >= texs.Count)
            {
                temp = UnityEngine.Random.Range(0, texs.Count-1);
            }
            else
            {
                index = temp;
            }
            Texture2D text = texs[temp];
            imgs[i].sprite = Sprite.Create(text, new Rect(0, 0, text.width, text.height), Vector2.zero);
        }
        return index;
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
