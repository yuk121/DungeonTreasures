using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Google;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Threading.Tasks;

public class TitleManager : SingleTonMonoBehaviour<TitleManager>
{
    enum ePlatform
    {
        None,
        Google,
        Facebook,
        iOS,
    }

    [SerializeField]
    TitleAnimationController m_titleAnim = null;
    [SerializeField]
    Button m_signUpBtn =null;

    //GoogleAuth m_googleAuth;

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

        Debug.Log($"Player Last Access Platform : {playerData.m_lastAccessPlatform}");
        switch(Enum.Parse<ePlatform>(playerData.m_lastAccessPlatform))
        {
            case ePlatform.None:
                // 로그인 버튼 활성화
                m_signUpBtn.gameObject.SetActive(true);
                break;

            case ePlatform.Google:
                Login_Google();
                break;

            case ePlatform.iOS:
                break;

            case ePlatform.Facebook:
                break;
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
        Login_Google();
    }
    
    private void Login_Google()
    {
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder()
            .RequestIdToken()
            .Build());
        PlayGamesPlatform.DebugLogEnabled = true;

        // 구글 로그인
        PlayGamesPlatform.Activate();
        Social.localUser.Authenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(bool success)
    {
        if (success)
        {
            // Continue with Play Games Services
            Debug.Log("구글 인증 완료");
            var authcode = PlayGamesPlatform.Instance.GetServerAuthCode();
            var idToken = PlayGamesPlatform.Instance.GetIdToken();

            Debug.Log("Firebase 인증 시작");
            // 파이어베이스 연동 시작
            FirebaseManager.Instance.FirebaseAuth(idToken, "", AuthSuccess, AuthFailed);
        }
        else
        {
            // Disable your integration with Play Games Services or show a login button
            // to ask users to sign-in. Clicking it should call
            // PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication).
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
    }

    private void AuthSuccess()
    {
        m_isLogin = true;
        m_signUpBtn.gameObject.SetActive(false);
        PlayerDataManager.Instance.PlayerData.m_lastAccessPlatform = ePlatform.Google.ToString();
    }

    private void AuthFailed()
    {
        // 구글 인증 로그아웃
        PlayGamesPlatform.Instance.SignOut();

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
