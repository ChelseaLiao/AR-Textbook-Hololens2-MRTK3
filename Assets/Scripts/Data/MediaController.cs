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
    public List<GameObject> videoObject;
    [SerializeField] private GameObject videoWin;
    [SerializeField] private GameObject winPos;
    bool videoUpdate = true;
    public static int paragraphsCount = 0;
    public static bool contentChanged = true;
    Texture2D image;
    Material material;
    string rootPath;
    public ButtonState lastButtonState, nextButtonState;
    public GameObject imageObjectPrefb;

    // Start is called before the first frame update
    void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        //ContentUpdate();
        if (contentChanged == true)
        {
            Debug.Log("article changed");
            ParagraphsReader.LoadPage();
            rootPath = ParagraphsReader.rootPath;
            paragraphsCount = 0;
            ContentUpdate(paragraphsCount);
            WriteParagraphs.WriteContent(paragraphsCount);
            contentChanged = false;
        }


    }
    public void lastParagraphs()
    {
        if (paragraphsCount > 0)
        {
            paragraphsCount--;
            foreach (var o in GameObject.FindGameObjectsWithTag("imageWindow"))
                Destroy(o);
            imageObject.Clear();
            foreach (var o in GameObject.FindGameObjectsWithTag("videoWindow"))
                Destroy(o);
            videoObject.Clear();
            // if(ParagraphsReader._video[paragraphsCount].videoPath != ParagraphsReader._video[paragraphsCount+1].videoPath)
            // {
            //     videoUpdate = true;
            //     foreach(var o in GameObject.FindGameObjectsWithTag("videoWindow"))
            //     Destroy(o);
            //     videoObject.Clear();
            // }
            // else
            //     videoUpdate = false;
            ContentUpdate(paragraphsCount);
            WriteParagraphs.WriteContent(paragraphsCount);
        }

    }
    public void nextParagraphs()
    {
        if (paragraphsCount < ParagraphsReader._paragraphsTitle.Count - 1)
        {
            paragraphsCount++;
            foreach (var o in GameObject.FindGameObjectsWithTag("imageWindow"))
                Destroy(o);
            imageObject.Clear();
            foreach (var o in GameObject.FindGameObjectsWithTag("videoWindow"))
                Destroy(o);
            videoObject.Clear();
            // if(ParagraphsReader._video[paragraphsCount].videoPath != ParagraphsReader._video[paragraphsCount-1].videoPath)
            // {
            //     Debug.Log(ParagraphsReader._video[paragraphsCount].videoPath);
            //     videoUpdate = true;
            //     foreach(var o in GameObject.FindGameObjectsWithTag("videoWindow"))
            //     Destroy(o);
            //     videoObject.Clear();
            // }
            // else
            //     videoUpdate = false;
            ContentUpdate(paragraphsCount);
            WriteParagraphs.WriteContent(paragraphsCount);
        }

    }

    public void ContentUpdate(int i)
    {
        GameObject articleTitle_Text = GameObject.FindWithTag("ArticleTitle");
        articleTitle_TMP = articleTitle_Text.GetComponent<TextMeshPro>();
        articleTitle_TMP.text = ParagraphsReader._articleTitle;
        for (int j = 0; j < ParagraphsReader._image[i].imageName.Count; j++)
        {
            SpawnImageWin(j);
            material = new Material(Shader.Find("Standard"));
            material.name = ParagraphsReader._image[i].imageName[j];
            GameObject imageMaterial = imageObject[j].transform.Find("Window").gameObject;
            GameObject imageTitle = imageObject[j].transform.Find("Title").gameObject;

            imageMaterial.transform.GetComponent<MeshRenderer>().material = material;
            float ratio = (float)ParagraphsReader._image[i].imageWidth[j] / (float)ParagraphsReader._image[i].imageHeight[j];
            Vector3 imgScale = imageMaterial.transform.localScale;
            imgScale.x = imageMaterial.transform.localScale.y * ratio;
            imageMaterial.transform.localScale = imgScale;
            Debug.Log(ratio);
            imageTitle.GetComponent<TextMeshPro>().text = material.name;
            float rectHeight = imageTitle.GetComponent<RectTransform>().sizeDelta.y;
            imageTitle.GetComponent<RectTransform>().sizeDelta = new Vector2(imgScale.x, rectHeight);
            string links = rootPath + ParagraphsReader._image[i].imagePath[j];
            StartCoroutine(GetImage(links, material));

        }
        for (int j = 0; j < ParagraphsReader._video[i].videoPath.Count; j++)
        {
            SpawnVideoWin(j);
            GameObject videoWinow = videoObject[j].transform.Find("Window").gameObject;
            VideoPlayer videoPlayer = videoWinow.transform.Find("Video").GetComponent<VideoPlayer>();
            videoPlayer.url = rootPath + ParagraphsReader._video[i].videoPath[j];
            videoWinow.transform.Find("Title").GetComponent<TextMeshPro>().text = ParagraphsReader._video[i].videoName[j];
        }
        if (i == 0)
        {
            lastButtonState.currentState = ButtonState.State.Disable;

            if (ParagraphsReader._paragraphsTitle.Count > 1)
                nextButtonState.currentState = ButtonState.State.Active;
            else
                nextButtonState.currentState = ButtonState.State.Disable;
        }
        else if (i > 0 && lastButtonState.currentState == ButtonState.State.Disable)
            lastButtonState.currentState = ButtonState.State.Active;

        else if (i >= ParagraphsReader._paragraphsTitle.Count - 1)
            nextButtonState.currentState = ButtonState.State.Disable;

        else if (i < ParagraphsReader._paragraphsTitle.Count - 1 && nextButtonState.currentState == ButtonState.State.Disable)
            nextButtonState.currentState = ButtonState.State.Active;
    }

    public void SpawnImageWin(int s)
    {
        //Quaternion rotation = Quaternion.Euler(0f, -30f, 0f);
        GameObject _imgW = (GameObject)Instantiate(imageObjectPrefb, transform.position, Quaternion.identity);
        _imgW.transform.position = new Vector3(winPos.transform.position.x - 0.4f, winPos.transform.position.y - 0.3f * s, winPos.transform.position.z);
        imageObject.Add(_imgW);
    }

    public void SpawnVideoWin(int s)
    {
        GameObject _mp4W = (GameObject)Instantiate(videoWin, transform.position, Quaternion.identity);
        _mp4W.transform.position = new Vector3(winPos.transform.position.x + 0.6f, winPos.transform.position.y - 0.3f * s, winPos.transform.position.z);
        videoObject.Add(_mp4W);
    }

    IEnumerator GetImage(string links, Material m)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(links))//Application.persistentDataPath+"/Image/"+imageName+".jpg"
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
                Debug.Log(m.name);
            }
        }
    }

    IEnumerator DownloadVideo(string links, string path)
    {

        UnityWebRequest www = UnityWebRequest.Get(links);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            File.WriteAllBytes(path, www.downloadHandler.data);
        }


    }
}
