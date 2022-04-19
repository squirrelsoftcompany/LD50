using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspectRatioSelector : MonoBehaviour
{
    public GameObject m_16_10Object;
    public GameObject m_16_9Object;

    public void Awake()
    {
        Select();
    }

#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        Select();
    }
#endif // UNITY_EDITOR

    private void Select()
    {
        bool is16_10 = Mathf.Abs(Camera.main.aspect - 1.6f) < Mathf.Abs(Camera.main.aspect - (16f / 9));
        m_16_10Object.SetActive(is16_10);
        m_16_9Object.SetActive(!is16_10);
    }
}
