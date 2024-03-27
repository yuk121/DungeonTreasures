using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Storage;
using UnityEngine.Networking;

public class GameDataLoadManager : DontDestory<GameDataLoadManager>
{
    FirebaseStorage storage;
    AssetBundleRequest bundleRequest;
    
    TextAsset m_characterTable;
    TextAsset m_effectTable;
    TextAsset m_ItemTable;
    TextAsset m_skillTable;

    public TextAsset ChracterTable { get { return m_characterTable; } }
    public TextAsset EffectTable { get { return m_effectTable; } }
    public TextAsset ItemTable { get { return m_ItemTable; } }
    public TextAsset SkillTable { get { return m_skillTable; } }

    bool m_isLoadDone = false;
    string m_url = string.Empty;

    // 데이터 테이블 에셋번들을 Storage로 부터 불러오는 구간

    public void LoadData(Action callback)
    {
#if UNITY_EDITOR
        LoadFromLocal(callback);
#elif UNITY_ANDROID
        LoadFromLocal(callback);
        //LoadFromFBStroage(callback);
#endif
    }

    private void LoadFromLocal(Action callback)
    {
        m_characterTable = Resources.Load<TextAsset>($"Data/CharacterDataTable");
        m_effectTable = Resources.Load<TextAsset>($"Data/EffectDataTable");
        m_ItemTable = Resources.Load<TextAsset>($"Data/ItemDataTable");
        m_skillTable = Resources.Load<TextAsset>($"Data/SkillDataTable");

        TableCharacter.Instance.CharacterDataLoad();
        TableItem.Instance.ItemDataLoad();
        TableSkill.Instance.SkillDataLoad();
        TableEffect.Instance.EffectDataLoad();

        callback?.Invoke();
    }

    private IEnumerator LoadFromFBStroage(Action callback)
    {
        yield return null;
    }


    //IEnumerator LoadDataFromFirebase()
    //{
    //    m_isLoadDone = false;

    //    FirebaseStorage storage = FirebaseStorage.DefaultInstance;
    //    StorageReference reference = storage.GetReferenceFromUrl("gs://dungeon-treasures.appspot.com/Android/datatable");

    //    var downloadTask = reference.GetDownloadUrlAsync();

    //    yield return new WaitUntil(() => downloadTask.IsCompleted);

    //    if (!downloadTask.IsCanceled && !downloadTask.IsFaulted)
    //    {
    //        Debug.Log("#로그 downloadTask.Result : " + downloadTask.Result);

    //        m_url = (downloadTask.Result).ToString();

    //        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(m_url);
    //        yield return request.SendWebRequest();

    //        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
    //        {
    //            Debug.Log("#로그 : 불러오기 실패");
    //            m_isLoadDone = false;
    //        }
    //        else
    //        {
    //            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(request);

    //            m_characterTable = bundle.LoadAsset<TextAsset>("CharacterDataTable");
    //            m_effectTable = bundle.LoadAsset<TextAsset>("EffectDataTable");
    //            m_ItemTable = bundle.LoadAsset<TextAsset>("ItemDataTable");
    //            m_skillTable = bundle.LoadAsset<TextAsset>("SkillDataTable");

    //            if (CheckTableLoad() == true)
    //            {                  
    //                m_isLoadDone = true;

    //                if (FirebaseManager.Instance != null)
    //                {
    //                    Debug.Log("#로그 유저 인증시작");
    //                    //FirebaseManager.Instance.AuthStart();
    //                }
    //            }
    //            else
    //                Debug.Log("#로그 : 데이터를 불러오지 못했습니다.");
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("#로그 Error : " + downloadTask.Exception);
    //    }
    //}

    //bool CheckTableLoad()
    //{
    //    if(m_characterTable != null && m_effectTable != null && m_ItemTable != null && m_skillTable != null)
    //    {
    //        if (TableCharacter.Instance != null && TableEffect.Instance != null && TableItem.Instance != null && TableSkill.Instance != null)
    //        {
    //            TableCharacter.Instance.CharacterDataLoad();
    //            TableItem.Instance.ItemDataLoad();
    //            TableSkill.Instance.SkillDataLoad();
    //            TableEffect.Instance.EffectDataLoad();

    //            return true;
    //        }
    //        else
    //        {
    //            return false;
    //        }
    //    }
    //    return false;
    //}

    #region WhenAPIQuit
    // 어플이내려지는 순간 저장
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            //종료
            if (ServerTimeManager.Instance != null && PlayerDataManager.Instance != null)
            {
                ServerTimeManager.Instance.DateTimeCheck();
                Debug.Log("#로그 LastTimestamp : " + ServerTimeManager.Instance.GetDate());
                PlayerDataManager.Instance.SetLastAccessTime(ServerTimeManager.Instance.GetDate());
            }
            //SaveUserInfo(m_auth.CurrentUser.UserId);
        }
    }

    // 어플이 꺼지는 순간 저장
    private void OnApplicationQuit()
    {
        //종료
        if (ServerTimeManager.Instance != null && PlayerDataManager.Instance != null)
        {
            ServerTimeManager.Instance.DateTimeCheck();
            Debug.Log("#로그 LastTimestamp : " + ServerTimeManager.Instance.GetDate());
            PlayerDataManager.Instance.SetLastAccessTime(ServerTimeManager.Instance.GetDate());
        }
        //SaveUserInfo(m_auth.CurrentUser.UserId);

        // Addressable 메모리 해제
        //if (GameDataManager.Instance != null)
        //    GameDataManager.Instance.ReleaseAddressableMem();
    }

    private void OnDestroy()
    {
       // FirebaseAuth.DefaultInstance.StateChanged -= AuthStateChanged;
    }
    #endregion
}
