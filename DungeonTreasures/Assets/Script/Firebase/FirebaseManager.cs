using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using Google;
using System.IO;
using Firebase.Extensions;

public class FirebaseManager : DontDestory<FirebaseManager>
{
    [SerializeField]
    string webClientId = "<클라이언트 아이디 입력>";

    FirebaseAuth m_auth;
    Credential m_credential;

    /// <summary>
    /// Sign In
    /// </summary>
    /// 

    #region FireBase Auth
    public void FirebaseAuth(string authCode, Action callbackSuccess, Action callbackFailed)
    {
        m_auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        m_credential = PlayGamesAuthProvider.GetCredential(authCode);
        m_auth.SignInWithCredentialAsync(m_credential).ContinueWithOnMainThread((task) =>
        {           
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync was canceled.");
                callbackFailed.Invoke();
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " + task.Exception);
                callbackFailed.Invoke();
                return;
            }

            Debug.Log("Firebase 인증 완료");
            callbackSuccess.Invoke();
        });
    }

    public void FirebaseAuth(string googleIdToken, string googleAccessToken, Action callbackSuccess, Action callbackFailed)
    {
        m_auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        m_credential = GoogleAuthProvider.GetCredential(googleIdToken, googleAccessToken);
        m_auth.SignInWithCredentialAsync(m_credential).ContinueWithOnMainThread((task) =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync was canceled.");
                callbackFailed.Invoke();
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInAndRetrieveDataWithCredentialAsync encountered an error: " + task.Exception);
                callbackFailed.Invoke();
                return;
            }

            Debug.Log("Firebase 인증 완료");
            callbackSuccess.Invoke();
        });
    }

    public void FirebaseSignOut()
    {
        m_auth.SignOut();
    }
    #endregion

 

    //    public void BeforeCheckUser()
    //    {
    //        FirebaseAuth.DefaultInstance.StateChanged += AuthStateChanged;
    //        //CheckUser();
    //    }

    //    void AuthStateChanged(object sender, EventArgs args)
    //    {
    //        CheckUser();
    //    }

    //    public string GetCurrentUserUID()                                                             
    //    {
    //        return m_auth.CurrentUser.UserId;
    //    }

    //     void CheckUser()
    //    {
    //        if (FirebaseAuth.DefaultInstance.CurrentUser != null)
    //        {
    //            if (TitleManager.Instance != null)
    //            {
    //                LoadUserInfo();

    //                Debug.Log("#로그 LoadUserInfo() 이후");
    //                TitleManager.Instance.SetLoginState(true);
    //            }
    //        }
    //        else
    //        {
    //            if (TitleManager.Instance != null)
    //            {
    //                TitleManager.Instance.ActiveSignUp();
    //            }
    //        }
    //    }


    //    #region WhenAPIQuit
    //    // 어플이내려지는 순간 저장
    //    private void OnApplicationPause(bool pause)
    //    {
    //        if(pause)
    //        {
    //            //종료
    //            if (ServerTimeManager.Instance != null && PlayerDataManager.Instance != null)
    //            {
    //                ServerTimeManager.Instance.DateTimeCheck();
    //                Debug.Log("#로그 LastTimestamp : " + ServerTimeManager.Instance.GetDate());
    //                PlayerDataManager.Instance.SetLastAccessTime(ServerTimeManager.Instance.GetDate());
    //            }
    //            SaveUserInfo(m_auth.CurrentUser.UserId);
    //        }
    //    }

    //    // 어플이 꺼지는 순간 저장
    //    private void OnApplicationQuit()
    //    {
    //        //종료
    //        if (ServerTimeManager.Instance != null && PlayerDataManager.Instance != null)
    //        {
    //            ServerTimeManager.Instance.DateTimeCheck();
    //            Debug.Log("#로그 LastTimestamp : " + ServerTimeManager.Instance.GetDate());
    //            PlayerDataManager.Instance.SetLastAccessTime(ServerTimeManager.Instance.GetDate());
    //        }
    //        SaveUserInfo(m_auth.CurrentUser.UserId);

    //        // Addressable 메모리 해제
    //        //if (GameDataManager.Instance != null)
    //        //    GameDataManager.Instance.ReleaseAddressableMem();
    //    }

    //    private void OnDestroy()
    //    {
    //        FirebaseAuth.DefaultInstance.StateChanged -= AuthStateChanged;
    //    }
    //    #endregion

    //    #region Database

    //    public void SaveUserInfo(string userId)
    //    {
    //        PlayerData userData = null;
    //        string json = null;
    //        bool isNew = false;

    //        if (PlayerDataManager.Instance != null)
    //        {
    //            //기존 정보가 있다면 불러옴
    //            userData = PlayerDataManager.Instance.GetPlayerData();

    //            //신규 가입시 세팅된 정보로 저장
    //            if(userData == null)
    //            {
    //                userData = new PlayerData();
    //                isNew = true;
    //            }
    //        }

    //        if (userData != null)
    //        {
    //            json = JsonUtility.ToJson(userData);
    //            m_reference.Child("UserInfo").Child(userId).SetRawJsonValueAsync(json);

    //            //신규 가입시 유저 정보 세팅후 로드를 위한 것. 신규가입이 아니라면 들어올 일이 없음
    //            if(isNew)
    //            {
    //                LoadUserInfo();
    //            }
    //        }
    //    }

    //    public void LoadUserInfo()
    //    {   
    //        if (m_auth.CurrentUser != null)
    //        {  
    //            FirebaseDatabase.DefaultInstance.GetReference("UserInfo").Child(m_auth.CurrentUser.UserId).GetValueAsync().ContinueWith(task =>
    //            {
    //                if (task.IsFaulted)
    //                {
    //                    Debug.Log("#Error 로그 : " + task.Exception + "\n");
    //                    // Handle the error...
    //                }
    //                else if (task.IsCompleted)
    //                {
    //                    DataSnapshot snapshot = task.Result;

    //                    // Do something with snapshot...
    //                    string jsonString = snapshot.GetRawJsonValue();

    //                    if (!string.IsNullOrEmpty(jsonString))
    //                    {
    //                        if (PlayerDataManager.Instance != null)
    //                        {
    //                            Debug.Log("#로그 :  " + jsonString + "\n");
    //                            PlayerDataManager.Instance.SetPlayerData(jsonString);
    //                        }
    //                    }
    //                }
    //            });
    //        }
    //    }

    //#endregion
}
