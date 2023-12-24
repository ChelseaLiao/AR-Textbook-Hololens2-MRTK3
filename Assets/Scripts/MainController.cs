using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public List<GameObject> gameObjects;
    public MediaController mediaController;

    void Start()
    {

    }
   
 

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchScene(int key)
    {
        MediaController.contentChanged = true;
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        if (currentScene > 0 || currentScene < SceneManager.sceneCountInBuildSettings - 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + key);
            Debug.Log("scene has changed ");
        }
        //ParagraphsReader.articleIndex = 0;
        
    }
    public void nextArticle()
    {
        ParagraphsReader.articleIndex += 1;
        if(ParagraphsReader.readJSon() != null)
        {
            foreach(var o in GameObject.FindGameObjectsWithTag("imageWindow"))
                Destroy(o);
            mediaController.imageObject.Clear();
            Debug.Log("image clear");
            MediaController.contentChanged = true;
        }
        else
            ParagraphsReader.articleIndex -= 1;
        

    }
    public void previousArticle()
    {
        if(ParagraphsReader.articleIndex > 0)
        {
            foreach(var o in GameObject.FindGameObjectsWithTag("imageWindow"))
                Destroy(o);
            mediaController.imageObject.Clear();
            ParagraphsReader.articleIndex -= 1;
            MediaController.contentChanged = true;
        }
        

    }

}
