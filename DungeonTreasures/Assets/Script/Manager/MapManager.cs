using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 전투가 끝나면 캐릭터를 움직이게 만들고 몬스터랑 조우 하게 만들 매니저 , 배틀매니저와 상호작용이 가능해야한다.
public class MapManager : SingleTonMonoBehaviour<MapManager>
{
    public enum ePartyBuff
    {
        ReduceCoolTime,
        PowerUp,
        DefenceUp,
        Heal,
        Max
    }
    
    [SerializeField]
    GameObject[] m_stopPointsObj = null;
    [SerializeField]
    GameObject m_playerTeamObj = null;
    [SerializeField]
    GameObject m_moveCharacterObj = null;
    [SerializeField]
    GameObject m_panelMoveUIObj = null;
    [SerializeField]
    GameObject m_buffObj = null;
    [SerializeField]
    GameObject m_panelInfomationObj = null;
    [SerializeField]
    Display m_panelSwitch = null;
    public Display PanelSwitch { get { return m_panelSwitch; } }
    [SerializeField]
    GamePad m_gamePad = null;
    public GamePad GamePad { get { return m_gamePad; } }

    MiniMap m_miniMap;
    GameObject m_portal = null;
    CameraMove m_cameraMove;

    int m_clearCount = 0;                                // 한 전투가 끝날때마다 오르며 3이상인경우 보스방이 열린다.
    bool m_isFirstMove = true;

    // 전투가 끝나고 탐색 시작시 들어올 메소드
    public void SetMapManager()
    {
        // 미니맵 키기
        m_miniMap.OnMinimap();

        // 터치 방지 꺼주기
        BattleManager.Instance.SetPreventTouchFalse();

        // 플레이이어 캐릭터 상태를 초기화
        for (int i = 0; i < m_playerTeamObj.transform.childCount - 1; i++)   // 마지막 자식은 카메라
        {
            var character = m_playerTeamObj.transform.GetChild(i).GetChild(1).GetComponent<CharacterAttack>();
            character.SetReady();
        }

        m_playerTeamObj.SetActive(false);

        // 움직일 캐릭터의 위치 선정
        if (m_isFirstMove)
        {
            // 캐릭터를 가지고 있는 오브젝트의 위치
            m_moveCharacterObj.transform.position = Vector3.zero;
            // 캐릭터 위치
            m_moveCharacterObj.transform.GetChild(0).transform.localPosition = m_stopPointsObj[0].transform.position;
            m_isFirstMove = false;
        }
        else
        {
            if (m_portal != null)
                m_moveCharacterObj.transform.GetChild(0).transform.position = m_portal.transform.position;
        }

        m_moveCharacterObj.SetActive(true);
        m_moveCharacterObj.GetComponentInChildren<CharacterMove>().OutPortal();

        ShowPortal();

        //Debug.Log(m_clearCount);

        m_panelMoveUIObj.SetActive(true);
        // 카메라가 움직이게 하는것
        m_cameraMove.enabled = !m_cameraMove.enabled;
        m_cameraMove.SetCameraMove();
    }

    public void ShowPortal()
    {
        for (int i = 1; i < m_stopPointsObj.Length - 1; i++)   // 마지막 보스방 제외하고 켜주기
        {
            if (m_stopPointsObj != null)
            {
                if (m_stopPointsObj[i].GetComponent<Portal>().CheckClerar() == false)
                {
                    m_stopPointsObj[i].SetActive(true);
                }
            }
        }

        // 방 클리어 수가 3이상인 경우 보스방을 켜준다.
        if (m_clearCount >= 3)
        {
            m_stopPointsObj[m_stopPointsObj.Length - 1].SetActive(true);
        }
    }

    // 모든 포탈을 꺼주는 메소드
    public void HidePortal()
    {
        for (int i = 1; i < m_stopPointsObj.Length; i++)  
        {
            if (m_stopPointsObj != null && m_stopPointsObj[i].activeSelf)
            {
                m_stopPointsObj[i].SetActive(false);
            }
        }
    }

    public void SetEnemySpawnPoint(GameObject spawnPoint)
    {
        m_portal = spawnPoint;
    }

    public Portal GetPortalInfo()
    {
        var portal = m_portal.GetComponent<Portal>();
        return portal;
    }

    public void TurnOffMoveUI()
    {
        m_panelMoveUIObj.SetActive(false);
    }

    public GameObject GetMoveCharacterObj()
    {
        return m_moveCharacterObj;
    }
    
    public void ClearCountPlus()
    {
        m_clearCount = m_clearCount + 1;
    }

    public int RandomBuff()
    {
        int buff = Random.Range((int)ePartyBuff.ReduceCoolTime, (int)ePartyBuff.Max);

        return buff;
    }

    public void ShowInfo(string text)
    {
        m_panelInfomationObj.SetActive(true);
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayUISound(SoundManager.eUISoundAudioClip.BuffWindow);
        }
        var window = m_panelInfomationObj.GetComponent<Display>();
        window.SetInfomation(text);

        m_moveCharacterObj.GetComponentInChildren<CharacterMove>().OutPortal();
        ClearCountPlus();
    }

    public void ShowBuffIcon(ePartyBuff partyBuff)
    {
        var icon = m_buffObj.GetComponentInChildren<Image>();

        if (partyBuff == ePartyBuff.PowerUp)
        {
            icon.sprite = ImageManager.Instance.GetBuffImage(ImageManager.eBuffIcon.ActionIconAtlas_5);
        }
        else if(partyBuff == ePartyBuff.DefenceUp)
        {
            icon.sprite = ImageManager.Instance.GetBuffImage(ImageManager.eBuffIcon.ActionIconAtlas_4);
        }

        m_buffObj.SetActive(true);
    }

    void SelectBuffPortal()
    {
        int buffPortal = Random.Range(1, m_stopPointsObj.Length - 1);                               // 첫번째방 보스방 제외하고 선택

        for(int i = 1; i < m_stopPointsObj.Length -1; i++)
        {
            if (i == buffPortal)
            {
                m_stopPointsObj[buffPortal].GetComponent<Portal>().SetPortalType(ePortalType.Buff);                // 버프방
            }
            else
            {
                m_stopPointsObj[i].GetComponent<Portal>().SetPortalType(ePortalType.Enemy);
            }              
        }     
    }

    void SetMoveCharacter()
    {
        var path = string.Format("Character/MoveCharacter/Move_{0}", LobbyDataManager.Instance.GetCharacterName(0));
        var prefab = Resources.Load(path);
        var moveCharacter = Instantiate(prefab) as GameObject;

        moveCharacter.transform.SetParent(m_moveCharacterObj.transform);
        moveCharacter.transform.localPosition = Vector3.zero;
        moveCharacter.transform.localScale = Vector3.one;
        moveCharacter.transform.localRotation = Quaternion.identity;
        moveCharacter.GetComponent<CharacterMove>().Set();

        m_moveCharacterObj.SetActive(false);
    }

    protected override void OnAwake()
    { 
        m_cameraMove = GameObject.Find("Main Camera").GetComponent<CameraMove>();
        m_miniMap = GameObject.Find("MiniMapCamera").GetComponent<MiniMap>();

        SetMoveCharacter();
        m_panelMoveUIObj.SetActive(false);
        m_buffObj.SetActive(false);
        m_panelInfomationObj.SetActive(false);

        for (int i = 0; i < m_stopPointsObj.Length; i++)
        {
            m_stopPointsObj[i].SetActive(false);
        }
        m_isFirstMove = true;
    }

    protected override void OnStart()
    {
        SelectBuffPortal();
    }
}
