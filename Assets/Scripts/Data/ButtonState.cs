using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit.UX;
public class ButtonState : MonoBehaviour
{
    public State currentState;
    public PressableButton pressableButton;
    public Material activeMaterial;
    public Material disableMaterial;
    // Start is called before the first frame update
    void Start()
    {
        //pressableButton = this.transform.Find("InteractiveButton").gameObject.GetComponent<PressableButton>();
        //disableMaterial = Resources.Load<Material>("UI/Materials/EMFT-UI-Rect-Gray_Button");
        //activeMaterial = Resources.Load<Material>("UI/Materials/EMFT-UI-Rect-Blue_Button");
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Disable:
                {
                    this.transform.GetComponent<MeshRenderer>().material = disableMaterial;
                    pressableButton.enabled = false;
                    break;
                }
            case State.Active:
                {
                    this.transform.GetComponent<MeshRenderer>().material = activeMaterial;
                    pressableButton.enabled = true;
                    break;
                }
        }
    }

    public enum State
    {
        Disable,
        Active
    }
}
