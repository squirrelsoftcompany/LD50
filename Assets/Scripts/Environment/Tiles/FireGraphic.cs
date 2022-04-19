using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGraphic : MonoBehaviour
{
    public GameObject m_firePivot;

    public void UpdateFire(int p_intensity)
    {
        if (p_intensity <= 0)
            m_firePivot.transform.localScale = Vector3.zero;
        else
            m_firePivot.transform.localScale = Vector3.one * p_intensity / 4f;
    }
}
