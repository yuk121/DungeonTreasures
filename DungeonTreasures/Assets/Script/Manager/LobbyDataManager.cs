using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyDataManager : DontDestory<LobbyDataManager>
{
    [SerializeField]
    Sprite[] m_characterIcon;
    [SerializeField]
    string[] m_characterName;

    private const float APRefillTime = 180; 

    GameObject m_lobbyCharactersObj;
    GameObject[] m_partyCharacter;

    float m_time = 0f;

    #region PartyInfo
    public int GetPartyNum()
    {
        return LobbyManager.Instance.GetPartyNum();
    }

    public void SetPartyInfo()
    {
        // 캐릭터 아이콘 및 이름을 가져오는 부분
        int num = LobbyManager.Instance.GetPartyNum();
        m_characterIcon = new Sprite[num];
        m_characterName = new string[num];

        if (LobbyManager.Instance != null)
        {
            for(int i=0; i < num; i++)
            {
                var stat = LobbyManager.Instance.GetCharacterStat(i);
                m_characterIcon[i] = stat.GetCharacterImage();
                m_characterName[i] = stat.GetCharacterName();
            }
        }      
    }

    public string GetCharacterName(int index)
    {
        return m_characterName[index];
    }

    public Sprite GetPartyIcon(int index)
    {
        return m_characterIcon[index];
    }
    #endregion

    void FillAP()
    {
        // 경과 시간에따른 접속 행동력 채워줌
        if (ServerTimeManager.Instance != null && PlayerDataManager.Instance != null)
        {
            ServerTimeManager.Instance.CheckElapsedUnixTime();

            double elaspedTime = ServerTimeManager.Instance.GetElaspedTime();
            int ApFill = (int)elaspedTime / 180;
            Debug.Log("#로그 ApFill : " + ApFill);

            PlayerDataManager.Instance.IncreaseAtcivePoint(ApFill, true);
            //if(FirebaseManager.Instance != null)
            //{
            //    FirebaseManager.Instance.SaveUserInfo(FirebaseManager.Instance.GetCurrentUserUID());
            //}
        }
    }

    protected override void OnAwake()
    {
        // 접속 시간 저장
        if(ServerTimeManager.Instance != null && PlayerDataManager.Instance != null && FirebaseManager.Instance != null)
        {
            PlayerDataManager.Instance.SetFirstAccessTime(ServerTimeManager.Instance.GetDate());
            //FirebaseManager.Instance.SaveUserInfo(FirebaseManager.Instance.GetCurrentUserUID());
        }
        FillAP();
    }


    private void Update()
    {
        if (PlayerDataManager.Instance != null)
        {
            if (PlayerDataManager.Instance.OwnedAP < 150)
            {
                m_time += Time.deltaTime;

                if (m_time >= APRefillTime)
                {
                    m_time = 0f;
                    PlayerDataManager.Instance.IncreaseAtcivePoint(1);

                    if (LobbyManager.Instance != null)
                        LobbyManager.Instance.ShowOwnActivePoint();

                    if (FirebaseManager.Instance != null)
                    {
                        //FirebaseManager.Instance.SaveUserInfo(FirebaseManager.Instance.GetCurrentUserUID());
                    }
                }
            }
        }
    }

}
