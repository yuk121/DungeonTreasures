using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameButton : MonoBehaviour
{
    LoadSceneManager m_loadSceneManager;
    bool m_isPause;

    public void OnPressRestart()
    {
        if(m_loadSceneManager != null)
            m_loadSceneManager.LoadRestart();
    }

    public void OnPressGoStage()
    {
        if(StageDataManager.Instance != null)
        {
            StageDataManager.Instance.ClearStageInfo();
        }

        if(SoundManager.Instance !=null)
        {
            SoundManager.Instance.StopBGM();
            SoundManager.Instance.StopUISound();
        }

        if (m_loadSceneManager != null)
            m_loadSceneManager.LoadStageScene();
    }

    public void OnPressPause()
    {
        PopupManager.Instance.SetPopupManager();

        if (m_isPause == false)
        {        
            PopupManager.Instance.CreateOKCancelPopup("게임 일시정지", "확인", "해당 스테이지를 다시 시작하겠습니까?", () =>
            {
                OnPressRestart();
            }
            , null , "예", "아니오");
        }
    }

   

    void Resume()
    {
        if(m_isPause == true)
        {
            Time.timeScale = 1;
            m_isPause = false;
            gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(GameObject.Find("LoadSceneManager") != null)
            m_loadSceneManager = GameObject.Find("LoadSceneManager").GetComponent<LoadSceneManager>();
        m_isPause = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
