using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;

public class WriteParagraphs : MonoBehaviour
{
    private static TextMeshProUGUI input;
    // Start is called before the first frame update
    void Start()
    {
        input = this.gameObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void WriteContent(int i)
    {
            string contentText = "<size=3.5><b>"+ParagraphsReader._paragraphsTitle[i]+"</b></size>" + Environment.NewLine + Environment.NewLine + ParagraphsReader._content[i];
            Debug.Log(contentText);
            
            input.text = contentText;
    }
}
