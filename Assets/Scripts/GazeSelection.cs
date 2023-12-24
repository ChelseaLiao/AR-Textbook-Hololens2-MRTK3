using MixedReality.Toolkit.UX;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GazeSelection : MonoBehaviour
{

    private Color originalColor;

    public void changeTextColorHover()
    {
        TextMeshPro text = this.GetComponent<TextMeshPro>();
        originalColor = text.color;
        if (text != null)
        {

            text.color = Color.red;
        }
    }
    public void changeTextColorExit()
    {
        TextMeshPro text = this.GetComponent<TextMeshPro>();
        if (text != null)
        {
            //Debug.Log("Exit Funtion Running:"+originalColor);
            text.color = Color.white;
        }
    }

    public void changeTextColorPinch()
    {
        TextMeshPro text = this.GetComponent<TextMeshPro>();
        if (text != null)
        {
            text.color = Color.green;
        }
    }

}
