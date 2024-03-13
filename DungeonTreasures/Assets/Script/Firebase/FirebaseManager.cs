using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Auth;
using Firebase;
using Google;
using System.IO;


public class FirebaseManager : DontDestory<FirebaseManager>
{
    [SerializeField]
    string webClientId = "<클라이언트 아이디 입력>";

    FirebaseAuth m_auth;
    GoogleSignInConfiguration m_configuration;
    UnityEngine.Events.UnityEvent m_Initialized = new UnityEngine.Events.UnityEvent();

    /// <summary>
    /// Sign In
    /// </summary>
    /// 

//    public void AuthStart()
//    {
//        m_configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };

//        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://dungeon-treasures.firebaseio.com/");
//        m_reference = FirebaseDatabase.DefaultInstance.RootReference;
//        CheckFirebaseDependencises();
//    }

//    void CheckFirebaseDependencises()
//    {
//        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
//        {
//            if (task.IsCompleted)
//            {
//                if (task.Result == DependencyStatus.Available)
//                {
//                    m_auth = FirebaseAuth.DefaultInstance;       
//                }
//                else
//                {
//                    Debug.Log("Could not resolve all Firebase dependencies : " + task.Result.ToString());
//                }
//            }
//            else
//            {
//                Debug.Log("Dependency check was not complted. Error : " + task.Exception.Message);
//            }
//        });
//    }

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

    #region Login
//    /// <summary>
//    /// Google Login Sign Up
//    /// </summary>

//    public void OnLogOut()
//    {
//        GoogleSignIn.DefaultInstance.SignOut();
//    }

//    public void OnDisconnect()
//    {
//        GoogleSignIn.DefaultInstance.Disconnect();
//    }

//    void OnSignUp()
//    {
//        GoogleSignIn.Configuration = m_configuration;
//        GoogleSignIn.Configuration.UseGameSignIn = false;
//        GoogleSignIn.Configuration.RequestIdToken = true;

//        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);


//#if UNITY_EDITOR
//        TitleManager.Instance.SetLoginState(true);
//#endif
//    }

//    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
//    {
//        if (task.IsFaulted)
//        {
//            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
//            {
//                if (enumerator.MoveNext())
//                {
//                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
//                    Debug.LogFormat("Got Error : {0} {1}", error.Status, task.Exception);
//                }
//                else
//                {
//                    Debug.LogFormat("Got Unexpected Exception?!? : {0}", task.Exception);
//                }
//            }
//        }
//        else if (task.IsCanceled)
//        {
//            Debug.LogFormat("Canceled");
//        }
//        else
//        {
//            if (task.Result.IdToken != null)
//                SignInWithGoogleOnFirebase(task.Result.IdToken);
//        }
//    }

//    void SignInWithGoogleOnFirebase(string idtoken)
//    {
//        Credential credential = GoogleAuthProvider.GetCredential(idtoken, null);

//        m_auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
//        {
//            AggregateException ex = task.Exception;
//            if (ex != null)
//            {
//                if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
//                    Debug.Log(ex.Message);
//            }
//            else
//            {
//                if (TitleManager.Instance != null)
//                {
//                    SaveUserInfo(task.Result.UserId);
//                    TitleManager.Instance.SetLoginState(true);
//                }
//            }
//        });        
//    }

#endregion
}
