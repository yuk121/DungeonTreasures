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
    Button m_stageInfoBtn = null;

    [SerializeField]
    GameObject[] m_enemyInfoObj = null;
    [SerializeField]
    GameObject[] m_contentInfoObj = null;

    [SerializeField]
    GameObject m_clearCheckObj = null;
    [SerializeField]
    GameObject m_stageLock = null;

    [SerializeField]
    bool m_isClear;

    [SerializeField]
    Text m_apText = null;
    [SerializeField]
    Text m_stageName = null;

    public void OnPressStageButton()
    {
        StageManager.Instance.PlayButtonSfx();
        if (m_nextGameScene != null)
        {
            if (StageDataManager.Instance != null)
                StageDataManager.Instance.SetStageInfo(m_stageIndex, m_activePoint, m_exp, m_gold, m_isClear);
        }

        if (StageManager.Instance != null)
        { 
            StageManager.Instance.SetNextScene(m_nextGameScene);

            Panel_BattleReady popup = StageManager.Instance.OpenPanel<Panel_BattleReady>("Panel_BattleReady");
            popup.SetBattleInfo(m_stageName.text, m_exp, m_gold, m_activePoint, m_enemyInfoObj, m_contentInfoObj);
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

    void CheckLock()
    {
        bool isLock = PlayerDataManager.Instance.GetDungeonLock(m_stageIndex);
        m_stageLock.gameObject.SetActive(isLock);
        m_stageInfoBtn.interactable = !isLock;
    }

    void ShowStageAP()
    {
        m_apText.text = string.Format("{0}", m_activePoint);
    }

    private void Start()
    {
        CheckLock();
        ShowClearMark();
        ShowStageAP();
        m_stageInfoBtn.onClick.AddListener(OnPressStageButton);
    }

}
