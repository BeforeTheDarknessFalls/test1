using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class GetWebPicture : MonoBehaviour
{
    public GameObject page1;
    public GameObject page2;
    public InputField Web_Url;
    public Button btn1;
    public Button btn2;
    public Button btn3;
    string pageInfo;
    public GameObject imgPanel;
    List<string> httpImg = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        btn1.onClick.AddListener(OnClickBtn1);
        btn2.onClick.AddListener(OnClickBtn2);
        btn3.onClick.AddListener(OnClickBtn3);
    }

    void OnClickBtn1()
    {
        string weburl = Web_Url.text;
        string info= getwebcode1(weburl);
        info = info.Replace(" ","");
        pageInfo = info;
        FindImgUrl(pageInfo);
    }
    void OnClickBtn2()
    {
        page2.SetActive(false);
        page1.SetActive(true);
    }
    void OnClickBtn3()
    {
        //
    }


    void FindImgUrl(string info)
    {
        char c = '"';
        string[] li = info.Split(c);
        httpImg.Clear();
        for (int i=0;i<li.Length;i++)
        {
            if (!li[i].StartsWith("http")) continue;
            if (!(li[i].EndsWith("png") || li[i].EndsWith("jpg"))) continue;
            if (!li[i].Contains("100x100xz")) continue;
            if (li[i].Length < 20) continue;
            string str = li[i].Substring(0, li[i].Length - 14);
            if (httpImg.Contains(str)) continue;
            Debug.Log(str);
            httpImg.Add(str);
            
        }
        Debug.Log(httpImg.Count);
        LoadImg();
    }
    

    public static string Getwebcode(string url, string encoder="UTF-8")
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //request.Method = "GET ";
        Stream receiveStream = response.GetResponseStream();
        StreamReader readStream = new StreamReader(receiveStream, Encoding.GetEncoding(encoder));
        string SourceCode = readStream.ReadToEnd();
        response.Close();
        readStream.Close();
        return SourceCode;
    }
    public static string getwebcode1(string url, string encoder= "UTF-8")
    {
        WebClient myWebClient = new WebClient();
        byte[] myDataBuffer = myWebClient.DownloadData(url);
        string SourceCode = Encoding.GetEncoding(encoder).GetString(myDataBuffer);
        return SourceCode;
    }

    public void LoadImg()
    {
        CheckPicturePanel();
        StartCoroutine("LoadImg1");
        page1.SetActive(false);
        page2.SetActive(true);
    }
    IEnumerator LoadImg1()
    {
        for(int i=0;i< httpImg.Count; i++)
        {
            WWW www = new WWW(httpImg[i]);
            yield return www;
            Texture2D tex = www.texture;
            SetPicture(tex,i);
        }
    }

    void CheckPicturePanel()
    {
        Transform imgs= imgPanel.transform.GetChild(1);
        int num = imgs.childCount;
        int imgNum = httpImg.Count;
        if (num == imgNum) return;
        if (num > imgNum)
        {
            for (int i = num - 1; i >= imgNum; i--)
            {
                GameObject.Destroy(imgs.GetChild(i));
            }
            return;
        }
        if (num < imgNum)
        {
            int count = imgNum - num;
            GameObject ga = imgPanel.transform.GetChild(0).gameObject;
            for (int i = num - 1; i >= imgNum; i--)
            {
                GameObject newGa= GameObject.Instantiate(ga);
                newGa.transform.SetParent(imgPanel.transform);
            }
        }
        
    }

    void SetPicture(Texture2D tex,int index)
    {
        Transform imgs = imgPanel.transform.GetChild(1);
        Transform imgObg = imgs.GetChild(index);
        //imgObg.GetComponent<Image>().mainTexture = tex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
