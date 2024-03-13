using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : SingleTonMonoBehaviour<StageManager>
{ 
    public enum eStageType
    {
        None = -1,
        Forest,
        Swamp,
        Desert,
        Volcano,
        SnowMountain,
        BattleReady,
        Max
    }

    [SerializeField]
    GameObject[] m_stageUis = new GameObject[(int)eStageType.Max];
    [SerializeField]
    GameObject[] m_characterIcon = new GameObject[4];
    [SerializeField]
    Text m_battleReadyAPText = null;

    string m_nextSceneInfo;

    public void SetNextScene(string nextScene)
    {
        m_nextSceneInfo = nextScene;
    }

    public string GetNextStageInfo()
    {
        return m_nextSceneInfo;
    }

    public void OnPressBackLobby()
    {
        LoadSceneManager.Instance.LoadLobbyScene();
    }

    public void PlayStageUISFX()
    {
        if(SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayUISound(SoundManager.eUISoundAudioClip.StageWindow);
        }
    }

    public void PlayButtonSfx()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayUISound(SoundManager.eUISoundAudioClip.Button);
        }
    }

    public void SetAP(int ap)
    {
        m_battleReadyAPText.text = string.Format("{0}", ap);
    }

    // 캐릭터 아이콘을 적용 시켜주는 구간
    public void SetCharacterIcon()
    {
        if(LobbyDataManager.Instance == null)
        {
            return;
        }

        for(int i = 0; i < m_characterIcon.Length; i++)
        {
            var icon = LobbyDataManager.Instance.GetPartyIcon(i);
            var Image = m_characterIcon[i].GetComponent<Image>();

            if(icon == null)
            {
                Image.sprite = null; 
            }
            else
            {
                Image.sprite = icon;
            }
        }
    }

    public void initailize()
    {
        for (int i = 0; i < m_stageUis.Length; i++)
        {
            if(i < m_stageUis.Length -1)
                m_stageUis[i] = Util.FindChildObject(GameObject.Find("Canvas"), string.Format("Panel_Stage{0}", (eStageType)i));
            else
                m_stageUis[i] = Util.FindChildObject(GameObject.Find("Canvas"), string.Format("Panel_{0}", (eStageType)i));

            m_stageUis[i].gameObject.SetActive(false);
        }
        for (int i = 0; i <m_characterIcon.Length; i++)
        {
            m_characterIcon[i] = Util.FindChildObject(GameObject.Find("Canvas"), string.Format("Image_Character_{0:00}", i +1));
        }
    }

    // Start is called before the first frame update
    protected override void OnStart()
    {
        initailize();
    }

}
