using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.IO;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using UnityEngine.Networking;

public class MediaController : MonoBehaviour
{
    public GameObject content;
    //private static TextMeshProUGUI input;
    private TextMeshPro articleTitle_TMP;
    public List<GameObject> imageObject;
    [SerializeField] private VideoPlayer videoObject;
    [SerializeField] private GameObject videoWin;
    [SerializeField] private GameObject winPos;
    bool videoUpdate = true;
    public static int paragraphsCount = 0;
    public static bool contentChanged = true;
    Texture2D image;
    Material material;
    public GameObject imageObjectPrefb;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        //ContentUpdate();
        if(contentChanged == true)
        {
            Debug.Log("article changed");
            ParagraphsReader.LoadPage();
            paragraphsCount = 0;
            ContentUpdate(paragraphsCount);
            WriteParagraphs.WriteContent(paragraphsCount);
            contentChanged = false;
        }
            
        
    }
    public void lastParagraphs()
    {   
        if(paragraphsCount > 0)
        {
            paragraphsCount --;
            foreach(var o in GameObject.FindGameObjectsWithTag("imageWindow"))
                Destroy(o);
            imageObject.Clear();
            if(ParagraphsReader._videoPath[paragraphsCount] != ParagraphsReader._videoPath[paragraphsCount+1])
                videoUpdate = true;
            else
                videoUpdate = false;
            ContentUpdate(paragraphsCount);
            WriteParagraphs.WriteContent(paragraphsCount);
        }
        
    }
    public void nextParagraphs()
    {   
        if(paragraphsCount < ParagraphsReader._paragraphsTitle.Count - 1)
        {
            paragraphsCount ++;
            foreach(var o in GameObject.FindGameObjectsWithTag("imageWindow"))
                Destroy(o);
            imageObject.Clear();
            if(ParagraphsReader._videoPath[paragraphsCount] != ParagraphsReader._videoPath[paragraphsCount-1])
                videoUpdate = true;
            else
                videoUpdate = false;
            ContentUpdate(paragraphsCount);
            WriteParagraphs.WriteContent(paragraphsCount);
        }
        
    }

    public void ContentUpdate(int i)
    {
            Debug.Log("content updated");

            GameObject articleTitle_Text = GameObject.FindWithTag("ArticleTitle");
            articleTitle_TMP = articleTitle_Text.GetComponent<TextMeshPro>();
            articleTitle_TMP.text = ParagraphsReader._articleTitle;
 
            for(int j=0; j<ParagraphsReader._image[i].imageName.Count; j++)
            {    
                        SpawnImageWin(j);
                        material = new Material(Shader.Find("Standard"));
                        material.name = ParagraphsReader._image[i].imageName[j];
                        GameObject imageMaterial = imageObject[j].transform.Find("Window").gameObject;
                        GameObject imageTitle = imageObject[j].transform.Find("Title").gameObject;

                        imageMaterial.transform.GetComponent<MeshRenderer>().material = material;
                        float ratio = (float)ParagraphsReader._image[i].imageWidth[j] / (float)ParagraphsReader._image[i].imageHeight[j];
                        Vector3 imgScale = imageMaterial.transform.localScale;
                        imgScale.x = imageMaterial.transform.localScale.y*ratio;
                        imageMaterial.transform.localScale = imgScale;
                        Debug.Log(ratio);
                        imageTitle.GetComponent<TextMeshPro>().text = material.name;
                        float rectHeight = imageTitle.GetComponent<RectTransform>().sizeDelta.y;
                        imageTitle.GetComponent<RectTransform>().sizeDelta = new Vector2(imgScale.x, rectHeight);
                        StartCoroutine(GetImage(material));
                        
            }
            if(ParagraphsReader._videoPath[i] == "null")
            {
                    videoWin.SetActive(false);
                    videoObject.url = null;
            }
            else
            {
                if(i == 0 || videoUpdate == true)
                {
                    videoWin.SetActive(true);
                    videoObject.url = ParagraphsReader._videoPath[i];
                    GameObject.FindWithTag("VideoTitle").GetComponent<TextMeshPro>().text = ParagraphsReader._videoName[i];
                }
                
            }

           
    }

    public void SpawnImageWin(int s)
    {
        //Quaternion rotation = Quaternion.Euler(0f, -30f, 0f);
        GameObject p = (GameObject)Instantiate(imageObjectPrefb, transform.position, Quaternion.identity);
        p.transform.position = new Vector3(winPos.transform.position.x-0.5f, winPos.transform.position.y+0.2f-0.3f*s, winPos.transform.position.z);
        imageObject.Add(p);
    }

    IEnumerator GetImage(Material m)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture("https://github.com/ChelseaLiao/Doc/releases/download/media/"+m.name+".jpg"))//Application.persistentDataPath+"/Image/"+imageName+".jpg"
        {
            yield return uwr.SendWebRequest();

            if (uwr.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                // Get downloaded asset bundle
                image = (Texture2D)DownloadHandlerTexture.GetContent(uwr);
                m.mainTexture = image;
                //Debug.Log(image.height);
            }
        }
    }

    IEnumerator DownloadVideo(string links, string path) 
    {

            UnityWebRequest www = UnityWebRequest.Get(links);
            yield return www.SendWebRequest();

            if(www.isNetworkError || www.isHttpError) {
                Debug.Log(www.error);
            } else {
                File.WriteAllBytes(path, www.downloadHandler.data);
            }
        
            
    }
}
