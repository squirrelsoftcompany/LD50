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

    //Quit buttons
    public Button mQuitYes;
    public Button mQuitNo;

    //GAmeOver button
    public Button mGameoverOK;

    [Header("Annotation")]
    public Text mAnnotationText;

    [Header("Animations")]
    public Animator mAnnotationDisplay;
    public Animator mQuitDisplay;
    public Animator mGameOverDisplay;

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
    public void onMenuQuit()
    {
        mQuitDisplay.SetTrigger("Pop");
    }

    public void onGlobalQuit()
    {
        mQuitDisplay.SetTrigger("Pop");
    }

    public void onQuitYes()
    {
        // TODO: If we are on main menu, quit the game. Otherwise just back to the menu
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif // UNITY_EDITOR
    }

    public void onGameOver()
    {
        mGameOverDisplay.SetTrigger("Pop");
    }

    public void onGameOverOK()
    {
        mGameOverDisplay.SetTrigger("Depop");
        //Change game state to menu
    }

    public void onQuitNo()
    {
        mQuitDisplay.SetTrigger("Depop");
    }

    public void onFulldisplay()
    {
        //GameManager.Inst.Fulldisplay();
    }

    public void onValidate()
    {
        //GameManager.Inst.Validate();
    }

    public void popAnnotation()
    {
        mAnnotationDisplay.SetTrigger("Pop");
    }
}
