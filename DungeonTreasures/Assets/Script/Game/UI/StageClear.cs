using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageClear : MonoBehaviour
{
    [SerializeField]
    GameObject[] m_contentInfoObj = null;
    [SerializeField]
    GameObject m_contentGridObj = null;

    int m_gold = 0;
    int m_exp = 0;

    public void SetContentInfo()
    {
        if (StageDataManager.Instance != null)
        {
            m_gold = StageDataManager.Instance.GetStageGoldInfo();
            m_exp = StageDataManager.Instance.GetStageExpInfo();
        }
    }

    public void CheckStageClear()
    {
        if(StageDataManager.Instance != null)
        {
            StageDataManager.Instance.ClearCheck();
        }
    }

    public void ShowContentList()
    {
        if (m_gold != 0)
        {
            var icon = Instantiate(m_contentInfoObj[0]) as GameObject;
            var infoText = icon.GetComponentInChildren<Text>();

            icon.transform.SetParent(m_contentGridObj.transform,false);
            infoText.text = string.Format("{0}G", m_gold);

            icon.transform.localPosition = new Vector3(icon.transform.position.x, icon.transform.position.y, 0f);
            icon.transform.localScale = new Vector3(2, 2);
        }

        if (m_exp != 0)
        {
            var icon = Instantiate(m_contentInfoObj[1]) as GameObject;
            var infoText = icon.GetComponentInChildren<Text>();

            icon.transform.SetParent(m_contentGridObj.transform,false);
            infoText.text = string.Format("{0}exp", m_exp);

            icon.transform.localPosition = new Vector3(icon.transform.position.x, icon.transform.position.y, 0f);
            icon.transform.localScale = new Vector3(2,2);
        }

    }
}
