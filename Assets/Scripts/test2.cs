using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class test2 : MonoBehaviour
{
    private TextMeshPro text;
    // Start is called before the first frame update
    void Start()
    {
        text = this.gameObject.GetComponent<TextMeshPro>();
        string path = Application.persistentDataPath;//U:/Users/91406/AppData/Local/Packages/AR-Learning.../LocalState
        text.text = path;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
