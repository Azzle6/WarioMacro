using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollUI : MonoBehaviour
{
    [SerializeField]
    private float               m_lerpTime;
    private ScrollRect          m_scrollRect;
    private Button[]            m_buttons;
    private int                 m_index;
    private float               m_verticalPosition;
    private float               m;
    

    void Update()
    {
        m_scrollRect.verticalNormalizedPosition = Mathf.Lerp(m_scrollRect.verticalNormalizedPosition, m_verticalPosition, Time.deltaTime / m_lerpTime);
    }
}
