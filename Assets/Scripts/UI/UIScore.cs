using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScore : MonoBehaviour
{
    [TextArea(3, 5)]
    public string m_formatText = "Score  {0}";
    private Text m_text = null;

    // Start is called before the first frame update
    void Start()
    {
        m_text = GetComponentInChildren<Text>();
    }

    public void UpdateScoreText(int p_score)
    {
        m_text.text = string.Format(m_formatText, p_score);
    }
}
