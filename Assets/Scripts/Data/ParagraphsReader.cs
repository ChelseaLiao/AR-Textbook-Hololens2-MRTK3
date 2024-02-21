using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

[System.Serializable]
public class ArticleData
{
    public string articleTitle="null";
    public string author = "null";
    public List<string> paragraphsTitle;
    public List<string> content;
    public List<VideoDetail> video;
    public List<ImageDetail> image;
    
}
[System.Serializable]
public class VideoDetail
{
    public List<string> videoPath;
    public List<string> videoName;
}
[System.Serializable]
public class ImageDetail
{
    public List<string> imageName;
    public List<string> imagePath;
    public List<int> imageWidth;
    public List<int> imageHeight;
}
public class ParagraphsReader : MonoBehaviour
{

    public static List<string> _paragraphsTitle;
    public static string _articleTitle;
    public static string _author;
    public static List<string> _content;
    public static string JsonPath;
    public static bool pageLoad = false;
    public static int articleIndex = 0;
    public static string articleIndexString;
    public static List<VideoDetail> _video;
    public static List<string> _videoName;
    public static List<string> _videoPath;
    public static List<ImageDetail> _image;
    public static List<string> _imageName;
    public static List<int> _imageWidth;
    public static List<int> _imageHeight;
    public static ArticleData articleDataTemp;
    public static string rootPath = "https://github.com/ChelseaLiao/AR-Textbook-Hololens2-MRTK3/releases/download/Materials";
    // Start is called before the first frame update
    void Start()
    {
       //LoadPage();
        
    }

    // Update is called once per frame
    void Update()
    {

            
    }
    public static void LoadPage()
    {
        
        ArticleData _info = readJSon();
        _articleTitle = _info.articleTitle;
        _author = _info.author;
        _paragraphsTitle = _info.paragraphsTitle;
        _content = _info.content;
        _video = _info.video;
        _image = _info.image;
    }
    public static ArticleData readJSon()
    {
        try
        {
            articleIndexString = articleIndex.ToString();
            JsonPath = rootPath + "/article_"+articleIndexString+".json";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(JsonPath);
            WebResponse response = request.GetResponse();
            Stream resStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(resStream);
            string json = sr.ReadToEnd();
            Debug.Log(json);
            articleDataTemp = JsonUtility.FromJson<ArticleData>(json);
            return articleDataTemp;
        }
        catch(WebException we)
        {
            var resp = we.Response as WebResponse;
            if (resp == null)
                throw;
            return null;
        }
       
    }

}
