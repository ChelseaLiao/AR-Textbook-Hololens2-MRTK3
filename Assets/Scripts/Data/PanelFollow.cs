using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelFollow : MonoBehaviour
{
    public GameObject panel;

    // Update is called once per frame
    void Update()
    {
        panel.transform.position = this.transform.position;
        panel.transform.rotation = this.transform.rotation;

        
    }
}
