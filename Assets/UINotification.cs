using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{

    public class UINotification : MonoBehaviour
    {
        public Text mNotificationText;
        public Animator mNotificationAnimation;

        // Start is called before the first frame update
        public void textChange(string pNewText)
        {
            mNotificationText.text = pNewText;
        }

        public void show(bool pShow)
        {
            if(pShow)
            {
                mNotificationAnimation.SetTrigger("Pop");
            }
            else 
            {
                mNotificationAnimation.SetTrigger("Depop");
            }
        }
    }
}
