using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Firebase.Auth;
using Google;
using UnityEngine.UI;

public class TitleManager : SingleTonMonoBehaviour<TitleManager>
{
    [SerializeField]
    TitleAnimationController m_titleAnim = null;
    [SerializeField]
    Button m_signUpBtn =null;
    

    FirebaseAuth m_auth;

    bool m_isLogin = false;
    bool m_isPopOn = false;

    public void PopFalse()
    {
        m_isPopOn = false;
    }

    public void ActiveSignUp()
    {
        m_signUpBtn.gameObject.SetActive(true);
    }

    public void SetLoginState(bool state)
    {
        if (FirebaseManager.Instance != null)
        {
            m_isLogin = state;

            if (m_isLogin)
            {
                m_signUpBtn.gameObject.SetActive(false);
                m_titleAnim.ShowStartToTuch();
            }
        }
    }

    protected override void OnAwake()
    {
        m_isLogin = false;

        m_signUpBtn.onClick.AddListener(OnClick_SignUp);
    }
    // Start is called before the first frame update
    protected override void OnStart()
    {    
        m_titleAnim.ShowLogo();

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM(LoadSceneManager.eScene.Title);
        } 

        if(GameDataLoadManager.Instance != null)
        {
            // 타이틀 시작시 테이블 데이터 로드
            GameDataLoadManager.Instance.LoadData( () => 
            {
                // 로드 끝난 경우 로그인 확인
                CheckLogin();
            });
        }
    }

    private void CheckLogin()
    {
#if UNITY_EDITOR
        PlayerDataManager.Instance.LoadPlayerData();
        
        if (PlayerDataManager.Instance.PlayerData == null)
        {
            PlayerDataManager.Instance.BasicSettingPlayer();
            PlayerDataManager.Instance.SavePlayerData();
        }
        else
        {
            PlayerDataManager.Instance.LoadPlayerData();
        }

#elif UNITY_ANDROID
        // 구글 로그인
#endif

        m_isLogin = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isLogin)
        {
            if(m_titleAnim.isLogoFadeEnd == true)
            {
                m_titleAnim.ShowStartToTuch();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_isPopOn = true;
            }

            if (m_isPopOn == false && m_titleAnim.TouchStart.activeSelf == true)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (LoadSceneManager.Instance != null)
                        LoadSceneManager.Instance.LoadLobbyScene();
                }
            }
        }
    }

    private void OnClick_SignUp()
    {

    }
}
