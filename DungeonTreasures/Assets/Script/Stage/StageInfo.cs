using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StageInfo : MonoBehaviour
{
    [SerializeField]
    string m_nextGameScene = null;
    [SerializeField]
    int m_activePoint = 10;
    [SerializeField]
    int m_gold = 500;
    [SerializeField]
    int m_exp = 150;
    [SerializeField]
    int m_stageIndex = 0;

    [SerializeField]
    GameObject m_enemyGridObj = null;
    [SerializeField]
    GameObject m_contentGridObj = null;

    [SerializeField]
    GameObject[] m_enemyInfoObj = null;
    [SerializeField]
    GameObject[] m_contentInfoObj = null;

    [SerializeField]
    GameObject m_clearCheckObj = null;

    [SerializeField]
    bool m_isClear;

    [SerializeField]
    Text m_apText = null;

    GameObject m_itemIcon;
    GameObject m_enemyIcon;

    public void OnPressStageButton()
    {
        SetStageIcon();
        if (m_nextGameScene != null)
        {
            if (StageDataManager.Instance != null)
                StageDataManager.Instance.SetStageInfo(m_stageIndex, m_activePoint, m_exp, m_gold, m_isClear);
        }

        if(StageManager.Instance != null)
        {
            StageManager.Instance.SetCharacterIcon();
           StageManager.Instance.SetAP(m_activePoint);
            StageManager.Instance.SetNextScene(m_nextGameScene);
        }
    }

    public void SetStageIcon()
    {
        for (int i = 0; i < m_enemyInfoObj.Length; i++)
        {
            m_enemyIcon = Instantiate(m_enemyInfoObj[i]) as GameObject;
            m_enemyIcon.transform.SetParent(m_enemyGridObj.transform,false);
        }

        for (int i = 0; i < m_contentInfoObj.Length; i++)
        {
            m_itemIcon = Instantiate(m_contentInfoObj[i]) as GameObject;
            m_itemIcon.transform.SetParent(m_contentGridObj.transform, false);
            var info = m_itemIcon.transform.GetChild(m_itemIcon.transform.childCount - 1).GetComponent<Text>();
            
            if(i == 0)   //경험치
            {
                info.text = string.Format("{0}exp", m_exp);
            }
            else if(i== 1)  //코인
            {
                info.text = string.Format("{0}G", m_gold);
            }
        }
    }

    void ShowClearMark()
    {
        if (PlayerDataManager.Instance != null)
        {
            m_isClear = PlayerDataManager.Instance.GetDungeonClear(m_stageIndex);

            if (m_isClear == true)
            {
                m_clearCheckObj.SetActive(true);
            }
        }
    }

    void ShowStageAP()
    {
        m_apText.text = string.Format("{0}", m_activePoint);
    }

    private void Start()
    {
        ShowClearMark();
       ShowStageAP();
    }

}
