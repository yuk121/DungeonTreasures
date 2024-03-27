using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Google;
using UnityEngine.UI;
using Assets.SimpleGoogleSignIn.Scripts;

public class TitleManager : SingleTonMonoBehaviour<TitleManager>
{
    [SerializeField]
    TitleAnimationController m_titleAnim = null;
    [SerializeField]
    Button m_signUpBtn =null;

    GoogleAuth m_googleAuth;

    bool m_isLogin = false;

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
        m_signUpBtn.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    protected override void OnStart()
    {    
        m_titleAnim.ShowLogo();

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM(LoadSceneManager.eScene.Title);
        }

        if (GameDataLoadManager.Instance != null)
        {
             Debug.Log("데이터 로드 시작");
            // 타이틀 시작시 테이블 데이터 로드
            GameDataLoadManager.Instance.LoadData( () => 
            {
                Debug.Log("데이터 테이블 로드 끝");
                // 로드 끝난 경우 로그인 확인
                CheckLogin();
            });
        }
    }

    private void CheckLogin()
    {
        Debug.Log("로그인 체크 시작");
        PlayerDataManager.Instance.LoadPlayerData();
        PlayerData playerData = PlayerDataManager.Instance.PlayerData;

        if (playerData == null)
        {
            playerData = new PlayerData();
            playerData.BasicSetting();
            PlayerDataManager.Instance.SavePlayerData(playerData);
        }

#if UNITY_EDITOR
        m_isLogin = true;
#elif UNITY_ANDROID
        // AccessToken 존재 확인
        if(playerData.m_googleAccessToken == null)
        {
            Debug.Log("AccessToken가 유효하지 않음");
            m_signUpBtn.gameObject.SetActive(true);
        }
        else
        {
            // 파이어베이스 인증시작
            Debug.Log("AccessToken 유효, Firebase Auth 시작");            
            FirebaseManager.Instance.FirebaseAuthStart(playerData.m_googleAccessToken, AuthSuccess, AuthFailed);
        }
#endif
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

            if (PopupManager.Instance.PopupCount == 0 && m_titleAnim.TouchStart.activeSelf == true)
            {
                if (UnityEngine.Input.GetMouseButtonDown(0))
                {
                    if (LoadSceneManager.Instance != null)
                        LoadSceneManager.Instance.LoadLobbyScene();
                }
            }
        }
    }

    private void OnClick_SignUp()
    {
        // 구글 로그인
        m_googleAuth = new GoogleAuth();
        //m_googleAuth.TryResume(OnSignIn, OnGetAccessToken);

        m_googleAuth.GetAccessToken(OnGetAccessToken);
    }

    public void GetAccessToken()
    {
        m_googleAuth.GetAccessToken(OnGetAccessToken);
    }

    private void OnSignIn(bool success, string error, UserInfo userInfo)
    {
        if (success == false)
            return;
    }

    private void OnGetAccessToken(bool success, string error, TokenResponse tokenResponse)
    {
        if (success == false)
        {
            PopupManager.Instance.CreateOKCancelPopup("로그인 실패", "", "다시 시작합니다.", () => 
            {
                // 재시작 
                LoadSceneManager.Instance.LoadTitleScene();
            }, 
            () => 
            {
                // 어플 종료
                PlayerDataManager.Instance.SavePlayerData();
                Application.Quit();
            });
        }

        Debug.Log("구글 인증 완료");
        PlayerData playerData = PlayerDataManager.Instance.PlayerData;
        playerData.m_googleAccessToken = tokenResponse.AccessToken;
        playerData.m_googleIdToken = tokenResponse.IdToken;

        // 파이어베이스 연동 시작
        FirebaseManager.Instance.FirebaseAuthStart(tokenResponse.AccessToken, AuthSuccess, AuthFailed);
    }

    private void AuthSuccess()
    {
        m_isLogin = true;
    }

    private void AuthFailed()
    {
        PopupManager.Instance.CreateOKCancelPopup("유저 인증 실패", "", "다시 시작합니다.", () =>
        {
            // 재시작 
            LoadSceneManager.Instance.LoadTitleScene();
        },
                  () =>
                  {
                      // 어플 종료
                      PlayerDataManager.Instance.SavePlayerData();
                      Application.Quit();
                  });
    }

}
