using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Panel_BattleReady : MonoBehaviour
{
    [SerializeField]
    Text m_textStage;
    [SerializeField]
    Text m_apText = null;
    [SerializeField]
    Button m_btnBattleReady;
    [SerializeField]
    Image[] m_ImgCharacter;
    [SerializeField]
    GameObject m_enemyGridObj = null;
    [SerializeField]
    GameObject m_contentGridObj = null;

    GameObject m_itemIcon;
    GameObject m_enemyIcon;
    GameObject[] m_enemyInfoObj = null;
    GameObject[] m_contentInfoObj = null;
    int m_gold = 0;
    int m_exp = 0;

    private void Start()
    {
        m_btnBattleReady.onClick.AddListener(OnClick_BattleReady);
    }

    public void SetBattleInfo(string stageName, int exp, int gold, int ap, GameObject[] enemyInfo, GameObject[] contentInfo)
    {
        m_textStage.text = stageName;
        m_enemyInfoObj = enemyInfo;
        m_contentInfoObj = contentInfo;
        
        m_exp = exp;
        m_gold = gold;
        m_apText.text = ap.ToString();

        if (LobbyDataManager.Instance == null)
        {
            return;
        }

        for (int i = 0; i < m_ImgCharacter.Length; i++)
        {
            var icon = LobbyDataManager.Instance.GetPartyIcon(i);
            m_ImgCharacter[i].sprite = icon;
        }

        SetStageIcon();
    }
    public void SetStageIcon()
    {
        if (m_enemyInfoObj.Length > 0)
        {
            for (int i = 0; i < m_enemyInfoObj.Length; i++)
            {
                m_enemyIcon = Instantiate(m_enemyInfoObj[i]) as GameObject;
                m_enemyIcon.transform.SetParent(m_enemyGridObj.transform, false);
            }
        }

        if (m_contentInfoObj.Length > 0)
        {
            for (int i = 0; i < m_contentInfoObj.Length; i++)
            {
                m_itemIcon = Instantiate(m_contentInfoObj[i]) as GameObject;
                m_itemIcon.transform.SetParent(m_contentGridObj.transform, false);
                var info = m_itemIcon.transform.GetChild(m_itemIcon.transform.childCount - 1).GetComponent<Text>();

                if (i == 0)   //경험치
                {
                    info.text = string.Format("{0}exp", m_exp);
                }
                else if (i == 1)  //코인
                {
                    info.text = string.Format("{0}G", m_gold);
                }
            }
        }
    }
    private void OnClick_BattleReady()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayUISound(SoundManager.eUISoundAudioClip.EnterDungeon);
        }

        if (CheckAP() == true)
        {
            if (StageManager.Instance != null)
            {
                if (StageManager.Instance.GetNextStageInfo() != string.Empty)
                {
                    LoadSceneManager.Instance.LoadGameScene(StageManager.Instance.GetNextStageInfo());
                }
            }
        }
        else
        {
            if (PopupManager.Instance != null)
            {
                PopupManager.Instance.CreateOKPopup("알림", "행동력이 부족합니다!", null);
            }
        }
    }

    bool CheckAP()
    {
        if (PlayerDataManager.Instance != null)
        {
            if (PlayerDataManager.Instance.OwnedAP - StageDataManager.Instance.GetStageAP() >= 0)
                return true;
        }
        return false;
    }
}
