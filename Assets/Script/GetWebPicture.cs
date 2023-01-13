using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class GetWebPicture : MonoBehaviour
{
    public PartManager partManager;
    public GameObject errorObj;
    public GameObject BG1;
    public GameObject page1;
    public GameObject page2;
    public InputField Web_Url;
    public Button btn1;
    public Button btn2;
    public Button btn3;
    string pageInfo;
    string pageUrl;
    string errorUrl;
    public GameObject imgPanel;
    List<string> httpImg = new List<string>();
    public List<Texture2D> texs = new List<Texture2D>();
    // Start is called before the first frame update
    void Start()
    {
        btn1.onClick.AddListener(OnClickBtn1);
        btn2.onClick.AddListener(OnClickBtn2);
        btn3.onClick.AddListener(OnClickBtn3);
    }
    /// <summary>
    /// 点击获取图片
    /// </summary>
    void OnClickBtn1()
    {
        string weburl = Web_Url.text;
        if (weburl == "") return;
        if (weburl == errorUrl) { ShowError(); return; }
        if (weburl== pageUrl)
        {
            ShowPage2();
            return;
        }
        pageUrl = weburl;
        string info= getwebcode1(pageUrl);
        if (info == "") { errorUrl = pageUrl; ShowError(); return; }
        info = info.Replace(" ","");
        pageInfo = info;
        FindImgUrl(pageInfo);
    }
    /// <summary>
    /// 点击返回
    /// </summary>
    //
    void OnClickBtn2()
    {
        ShowPage1();
    }

    void ShowPage1()
    {
        BG1.SetActive(true);
        page2.SetActive(false);
        page1.SetActive(true);
    }
    void ShowPage2()
    {
        BG1.SetActive(false);
        page2.SetActive(true);
        page1.SetActive(false);
    }
    /// <summary>
    /// 点击预览
    /// </summary>
    void OnClickBtn3()
    {
        partManager.ShowNewPics(texs);
        page1.SetActive(false);
        page2.SetActive(false);
        errorObj.SetActive(false);
        BG1.SetActive(false);
        imgPanel.SetActive(false);
    }

    void ShowError()
    {
        errorObj.SetActive(false);
        errorObj.SetActive(true);
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
        WebClient myWebClient= new WebClient();
        byte[] myDataBuffer = null;
        try 
        {
            myDataBuffer = myWebClient.DownloadData(url);
        } 
        catch (Exception ex) 
        {
            Debug.Log("无法打开网页");
            return "";
        }
        
        string SourceCode = Encoding.GetEncoding(encoder).GetString(myDataBuffer);
        return SourceCode;
    }

    public void LoadImg()
    {
        CheckPicturePanel();
        texs.Clear();
        StartCoroutine("LoadImg1");
        ShowPage2();
       
    }
    IEnumerator LoadImg1()
    {
        for(int i=0;i< httpImg.Count; i++)
        {
            WWW www=null;
            try { 
            www = new WWW(httpImg[i]);
            } catch (Exception ex) 
            {
                Debug.Log("连接错误");
                yield break;
            }
            yield return www;

            Texture2D tex = www.texture;
            texs.Add(tex);
            SetPicture(tex,i);
        }
    }

    void CheckPicturePanel()
    {
        Transform imgs= imgPanel.transform.GetChild(1);
        int num = imgs.childCount;
        int imgNum = httpImg.Count;
        Debug.Log("num:"+num+"  /"+ "imgNum" + imgNum);
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
            for (int i = 0; i< count; i++)
            {
                GameObject newGa= GameObject.Instantiate(ga);
                newGa.transform.SetParent(imgPanel.transform.GetChild(1));
                newGa.transform.localScale = Vector3.one;
            }
            SortImg();
        }
        
    }

    void SortImg()
    {
        Transform imaPanel = imgPanel.transform.GetChild(1);
        int count = imaPanel.childCount;
        int column = 0;
        int row = 0;
        Vector2 pos = new Vector2(0,0);
        for(int i = 0; i < count; i++)
        {
            row = i / 3;
            column = i % 3;
            pos.x = column * 250 - 250;
            pos.y = 250- row * 250;
            imaPanel.GetChild(i).localPosition = pos;
        }
    }

    void SetPicture(Texture2D tex,int index)
    {
        Transform imgs = imgPanel.transform.GetChild(1);
        Transform imgObg = imgs.GetChild(index);
        imgObg.gameObject.SetActive(true);
        Sprite s = Sprite.Create(tex, new Rect(0,0, tex.width, tex.height),Vector2.zero); ;
        imgObg.GetComponent<UnityEngine.UI.Image>().sprite = s;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
