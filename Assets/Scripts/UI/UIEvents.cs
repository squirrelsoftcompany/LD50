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

    [Header("UIs")]
    public GameObject mMenuUI;
    public GameObject mNotificationUI;
    public GameObject mGameOverUI;
    public GameObject mInGameUI;
    public GameObject mConfirmationQuitUI;
    public GameObject mRewardUI;


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
        GameManagerCool.Inst.Play();

    }
    public void onMenuQuit()
    {
        mConfirmationQuitUI.GetComponentInChildren<Animator>().SetTrigger("Pop");
    }

    public void onGlobalQuit()
    {
        mConfirmationQuitUI.GetComponentInChildren<Animator>().SetTrigger("Pop");
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
        mGameOverUI.GetComponentInChildren<Animator>().SetTrigger("Pop");
    }

    public void onGameOverOK()
    {
        mGameOverUI.GetComponentInChildren<Animator>().SetTrigger("Depop");
        //Change game state to menu
    }

    public void onQuitNo()
    {
        mConfirmationQuitUI.GetComponentInChildren<Animator>().SetTrigger("Depop");
    }

    public void onFulldisplay()
    {
        //GameManagerCool.Inst.Fulldisplay();
    }

    public void onValidate()
    {
        //GameManagerCool.Inst.Validate();
    }

    public void popAnnotation()
    {
        mNotificationUI.GetComponentInChildren<Animator>().SetTrigger("Pop");
    }
}
