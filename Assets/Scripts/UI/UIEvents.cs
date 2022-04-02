using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEvents : MonoBehaviour
{
    [Header("Buttons")]
    //Menu buttons
    public Button mMenuStart;
    public Button mMenuQuit;
    //Global buttons
    public Button mGlobalMusic;
    public Button mGlobalQuit;
    //Reward buttons TODO add buttons for every slot
    //public Button mSlot[];

    [Header("Annotation")]
    public Text mAnnotationText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void onStart()
    {
        //GameManager.Inst.StartGame();
    }
    public void onQuit()
    {
        //GameManager.Inst.BackToNextMenu();
    }

    public void onFulldisplay()
    {
        //GameManager.Inst.Fulldisplay();
    }

    public void onValidate()
    {
        //GameManager.Inst.Validate();
    }
}
