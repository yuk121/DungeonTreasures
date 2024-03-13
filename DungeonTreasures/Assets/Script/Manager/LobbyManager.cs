using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : SingleTonMonoBehaviour<LobbyManager>
{
    [SerializeField]
    GameObject[] m_partyCharacter =null;

    GameObject m_goldText;
    GameObject m_activePontText;
    GameObject m_lobbyCharactersObj;

    void SetPartyCharacter()
    {
        m_lobbyCharactersObj = GameObject.Find("Lobby_Characters").gameObject;
        int Max = m_lobbyCharactersObj.transform.childCount;
        m_partyCharacter = new GameObject[Max];

        for (int i = 0; i < m_lobbyCharactersObj.transform.childCount; i++)
        {
            m_partyCharacter[i] = m_lobbyCharactersObj.transform.GetChild(i).gameObject;
        }
    }

    public int GetPartyNum()
    {
        return m_partyCharacter.Length;
    }

    public CharacterStatus GetCharacterStat(int index)
    {
        var stat = m_partyCharacter[index].transform.GetChild(0).GetComponent<CharacterStatus>();
        return stat;
    }

    // 모험버튼
    public void OnPress_Adventure()
    {
        if (LoadSceneManager.Instance != null)
        {
            LoadSceneManager.Instance.LoadStageScene();
        }
    }

    public void PlayButtonSfx()
    {
        if(SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayUISound(SoundManager.eUISoundAudioClip.Button);
        }
    }

    #region PlayerGoods

    public void ShowOwnedGold()
    {
        if (PlayerDataManager.Instance != null)
        {
            m_goldText.GetComponent<Text>().text = string.Format("{0:#,###}", PlayerDataManager.Instance.OwnedGold);
        }
    }

    public void ShowOwnActivePoint()
    {
        if (PlayerDataManager.Instance != null)
        {
            Debug.Log("#로그  m_activePontText : " + m_activePontText);
            m_activePontText.GetComponent<Text>().text = string.Format("{0}", PlayerDataManager.Instance.OwnedAP);
        }
    }

    #endregion
    protected override void OnAwake()
    {
        if(PopupManager.Instance != null)
        {
            PopupManager.Instance.SetPopupManager();
        }
        m_goldText = Util.FindChildObject(GameObject.Find("Canvas"), "Text_Gold");
        m_activePontText = Util.FindChildObject(GameObject.Find("Canvas"), "Text_AP");
        SetPartyCharacter();
    }

    protected override void OnStart()
    {
        if(LobbyDataManager.Instance != null)
        {
            LobbyDataManager.Instance.SetPartyInfo();
            ShowOwnActivePoint();
            ShowOwnedGold();
        }
    }
}
