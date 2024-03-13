using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    [SerializeField]
    GameObject m_enemyInfoObj = null;
    [SerializeField]
    GameObject m_contentItemObj = null;

    public void OnPressBackLobby()
    {
        if(StageManager.Instance != null)
        {
            StageManager.Instance.OnPressBackLobby();
        }
    }

    public void OnPressIconClear()
    {
        if (m_enemyInfoObj.transform.childCount != 0)
        {
            for(int i= 0; i < m_enemyInfoObj.transform.childCount; i++)
            {
                Destroy(m_enemyInfoObj.transform.GetChild(i).gameObject);
            }
        }

        if(m_contentItemObj.transform.childCount != 0)
        {
            for (int i = 0; i < m_contentItemObj.transform.childCount; i++)
            {
                Destroy(m_contentItemObj.transform.GetChild(i).gameObject);
            }
        }
    }
}
