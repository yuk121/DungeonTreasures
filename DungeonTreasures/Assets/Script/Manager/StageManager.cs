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

    // 캐릭터 아이콘을 적용 시켜주는 구간

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
    }

    public T OpenPanel<T>(string name) where T : UnityEngine.Object
    {
        for(int i = 0; i < m_stageUis.Length; i++)
        {
            if (m_stageUis[i].name == name)
            {
                if (m_stageUis[i].activeSelf == false)
                {
                    m_stageUis[i].SetActive(true);
                }
                
                T component = m_stageUis[i].GetComponent<T>();
                return component;
            }
        }

        return null;
    }
    // Start is called before the first frame update
    protected override void OnStart()
    {
        initailize();
    }

}
