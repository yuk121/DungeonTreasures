using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static ImageManager;

public class BattleManager : SingleTonMonoBehaviour<BattleManager>
{
    public enum eState
    {
        Action,
        Waiting
    }

    const float WAIT_TIME = 1f;
    const int MAX_TURNGAUGE = 430;

    [SerializeField]
    GameObject m_playerTeamObj = null;
    [SerializeField]
    GameObject m_enemyTeamObj = null;
    [SerializeField]
    GameObject m_battlePanelObj = null;
    [SerializeField]
    Display m_gameoverPanel = null;
    [SerializeField]
    Display m_gameClearPanel = null;
    [SerializeField]
    Display m_switchPanel = null;
    [SerializeField]
    GameObject[] m_skillHighLights = null;
    [SerializeField]
    GameObject[] m_skillCoolTimeObj = null;
    [SerializeField]
    GameObject m_characterUIObj = null;
    [SerializeField]
    GameObject m_bossUIObj = null;
    [SerializeField]
    GameObject m_bossCharacter = null;                              // 보스 캐릭터를 저장할 변수
    [SerializeField]
    GameObject m_turnUI = null;
    [SerializeField]
    GameObject m_preventTouchObj = null;

    public bool m_canAction;

    eState m_state;
    CameraPath m_cameraPath;
    GameObject m_nowCharacter;                              // 현재 선택된 캐릭터
    GameObject m_prevCharacter;                              // 이전 턴의 캐릭터
    GameObject m_prevOnHighLight;
    GameObject m_moveCharacterObj;                      // 맵을 움직이는 캐릭터
    GameObject m_nowSpawnPoint;
    CharacterAttack m_prevTileOnPlayer;
    CharacterAttack m_nowTargetPlayer;
    MonsterController m_prevTileOnMonster;            // 적 개체중 발판이 켜진개체를 저장
    MonsterController m_nowEnemy;                        // 현재 선택된 적
    string m_nowMyTargetType;
    string m_nowCharacterType;                               // 현재 캐릭터의 타입
    MapManager m_mapManager;
    Portal m_enterPortal;
    MiniMap m_miniMap;
    int m_targgetClickCount = 0;
    int m_skillClickCount = 0;
    int m_skillBtnNumber = 0;
    int m_enemySkillIndex = 0;
    int m_prevBtnNumber = 0;
    int m_encounterMonsterCount = 0;

    bool m_ListFull;
    bool m_isEnemyAttackProcess = false;
    bool m_isAttackProcess = false;
    bool m_isEnemySelectAll = false;
    bool m_isPlayerSelectAll = false;
    bool m_isFisrtBattle = true;
    bool m_isFirstTurn = true;

    bool m_nowSkillUsing = false;
    bool m_isSettingEnd = false;
    public bool m_isBattle = false;

    float m_checkTime = 0f;
    List<GameObject> m_readyCharacterList = new List<GameObject>();
    List<GameObject> m_deadMonsterList = new List<GameObject>();
    List<string> m_characterOrderList = new List<string>();                                                                        // 현재 전투에서 캐릭터들간 순서를 넣어줄 리스트
    Dictionary<string, GameObject> m_turnIconDic = new Dictionary<string, GameObject>();                   // 캐릭터 아이콘 정보를 저장할 Dic

    // 전투 시작을 알리는 메소드
    public void SetBattleManager()
    {
        // 카메라가 플레이어 따라가는 것을 꺼준다.
        m_cameraPath.enabled = true;
        // 미니맵도 꺼준다.
        m_miniMap.OffMiniMap();

        GameObject characterObj = m_mapManager.GetMoveCharacterObj();
        m_mapManager.TurnOffMoveUI();

        m_enterPortal = m_mapManager.GetPortalInfo();
        m_nowSpawnPoint = m_enterPortal.GetEnemySpwanPoint();

        if (characterObj != null && characterObj.activeSelf)
        {
            characterObj.SetActive(false);
        }

        // 몬스터 스폰위치에 따른 캐릭터팀 위치 변경
        // 보스방
        if (m_nowSpawnPoint.transform.position.x == 0 && m_nowSpawnPoint.transform.position.z == 0)                           // 보스방
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayBossBGM();

            m_playerTeamObj.transform.position = new Vector3(0, 0, m_nowSpawnPoint.transform.position.z - 10);
        }
        else if (m_nowSpawnPoint.transform.position.x != 0)                                                                                                  // 좌/ 우방
        {
            if (m_nowSpawnPoint.transform.position.x < 0)
            {
                m_playerTeamObj.transform.position = new Vector3(m_nowSpawnPoint.transform.position.x + 10, 0, 0);
            }
            else if (m_nowSpawnPoint.transform.position.x > 0)
            {
                m_playerTeamObj.transform.position = new Vector3(m_nowSpawnPoint.transform.position.x - 10, 0, 0);
            }
        }
        else if (m_nowSpawnPoint.transform.position.z != 0)                                                                                                  // 일직선 상에 있는 방
        {
            if (m_nowSpawnPoint.transform.position.z > 0)
            {
                m_playerTeamObj.transform.position = new Vector3(0, 0, m_nowSpawnPoint.transform.position.z - 10);
            }
            else if (m_nowSpawnPoint.transform.position.z <= 0)
            {
                // 
            }
        }
        m_playerTeamObj.transform.rotation = m_nowSpawnPoint.transform.rotation;

        m_playerTeamObj.SetActive(true);

        // 몬스터를 보여주는 for문
        for (int i = 1; i < m_nowSpawnPoint.transform.childCount; i++)
        {
            var enemy = m_nowSpawnPoint.transform.GetChild(i).GetChild(0).gameObject;
            enemy.SetActive(true);
        }

        m_enterPortal.gameObject.SetActive(false);
        m_checkTime = 0f;
        m_isFirstTurn = true;
        m_mapManager.HidePortal();
        m_turnUI.SetActive(true);
        SetTurnIcon();
    }

    public void AfterAnimEndProcess()
    {
        m_skillClickCount = 0;
        m_cameraPath.SetCameraPath();
        m_isBattle = true;
    }

    // 몬스터가 죽는 모션이 끝난 경우 화면 전환 해줄 메소드 + 보스 몬스터 잡으면 클리어
    public void ScreenSwitch(bool isDie, GameObject monster, eMonsterTpye monType)
    {
        if (isDie)
        {
            if (monType == eMonsterTpye.Boss)
            {
                m_isBattle = false;

                m_gameClearPanel.gameObject.SetActive(true);

                if (m_battlePanelObj.activeSelf == true)
                {
                    m_battlePanelObj.SetActive(false);
                }

                EndProcess();

                if (SoundManager.Instance != null)
                {
                    SoundManager.Instance.StopBGM();
                    SoundManager.Instance.PlayUISound(SoundManager.eUISoundAudioClip.Victory);
                }
                m_gameClearPanel.SetClearPanel();
                var clear = m_gameClearPanel.GetComponent<StageClear>();
                SetPreventTouchFalse();
                clear.SetContentInfo();
                clear.ShowContentList();

#if UNITY_EDITOR
                //// 데이터 저장부분
                //if (StageDataManager.Instance != null && PlayerDataManager.Instance != null)
                //{
                //    if (StageDataManager.Instance.GetStageClear() == false)
                //    {
                //        PlayerDataManager.Instance.SetDungeonClear(StageDataManager.Instance.GetStageIndex());
                //    }
                //    PlayerDataManager.Instance.IncreaseGold(StageDataManager.Instance.GetStageGoldInfo());

                //    PlayerDataManager.Instance.SavePlayerData();
                //}
#endif

                if (StageDataManager.Instance != null && PlayerDataManager.Instance != null)
                {
                    if (StageDataManager.Instance.GetStageClear() == false)
                    {
                        PlayerDataManager.Instance.SetDungeonClear(StageDataManager.Instance.GetStageIndex());
                    }

                    // 골드 증가 및 행동력 감소
                    PlayerDataManager.Instance.IncreaseGold(StageDataManager.Instance.GetStageGoldInfo());
                    PlayerDataManager.Instance.DecreaseActivePoint(StageDataManager.Instance.GetStageAP());

                    //if(FirebaseManager.Instance !=  null)
                    //{
                    //    FirebaseManager.Instance.SaveUserInfo(FirebaseManager.Instance.GetCurrentUserUID());
                    //}

                    //PlayerDataManager.Instance.APKSavePlayerData();
                }

            }

            else
            {
                m_deadMonsterList.Add(monster);
            }

            if (m_deadMonsterList.Count >= m_encounterMonsterCount)
            {
                m_isBattle = false;

                if (m_battlePanelObj.activeSelf == true)
                {
                    m_battlePanelObj.SetActive(false);
                }

                EndProcess();

                m_switchPanel.m_callEvent = m_mapManager.SetMapManager;
                m_switchPanel.SetSwitchPanel();
            }
        }
    }

    // 턴 제어하는 메소드
    public void GetAttackReady(bool ready, GameObject character)
    {
        // 전투 종료시 해당 메소드에 들어오는것을 방지
        if (CheckBattleEnd() == false)                                                                          
        {
            return;
        }

        // 준비된 캐릭터 부터 리스트에 넣어준다.
        if (ready)
        {
            m_readyCharacterList.Add(character);
            m_characterOrderList.Add(character.transform.parent.name);
        }

        int maxCount = GetMonsters().Count + GetPlayers().Count; 
        EncounterMonsterCount();                                                                                                                                 // 만난 몬스터 수 
          
        if (m_readyCharacterList.Count >= maxCount)                                                                                                   // 현재 전투에 살아있는 개체 수 
        {
            SetPreventTouchFalse();                                                                                                                                  // 다음턴의 캐릭터 순서시 터치 방지 해제
            SetSkillNotUse();                                                                   
            m_ListFull = true;
            m_nowCharacter = m_readyCharacterList[0];                                                                                                  // 캐릭터를 리스트에서 꺼낸다.
            ShowCharacterOrder();                                                                                                                                   // 캐릭터들의 행동 순서를 보여주는 메소드
            m_nowCharacterType = m_nowCharacter.GetComponent<CharacterStatus>().GetCharacterType();              // 현재 캐릭터 정보 
        }
    }

    // 리스트에 있는것을 기반으로 넣어준다.
    void ShowCharacterOrder()
    {
        int rootIndex = m_turnUI.transform.childCount - 1;
        GameObject root = m_turnUI.transform.GetChild(rootIndex).gameObject;

        if (!m_isFirstTurn)
        {
            if (m_prevCharacter != null)
            {
                if (m_turnIconDic.TryGetValue(m_characterOrderList[m_characterOrderList.Count - 1], out GameObject icon))
                {
                    icon.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 1f);
                    icon.GetComponent<RectTransform>().SetAsLastSibling();
                    var Frame = icon.transform.GetChild(1).GetComponent<Image>();
                    Frame.color = new Color(255, 255, 255);
                }
                if (m_turnIconDic.TryGetValue(m_characterOrderList[0], out GameObject NowIcon))  //현재 턴 캐릭
                {
                    Debug.Log("#로그 : 현재 턴 아이콘 : " + m_characterOrderList[0]);
                    NowIcon.GetComponent<RectTransform>().localScale = new Vector3(0.65f, 0.65f, 0.65f);
                    var Frame = NowIcon.transform.GetChild(1).GetComponent<Image>();
                    Frame.color = new Color(255, 255, 0);
                }
            }
        }
        else
        {
            for (int i = 0; i < m_characterOrderList.Count; i++)
            {
                if (m_turnIconDic.TryGetValue(m_characterOrderList[i], out GameObject icon))
                {
                    icon.GetComponent<RectTransform>().transform.SetParent(root.transform);
                    icon.GetComponent<RectTransform>().transform.localPosition = new Vector3(0, 0, 0);
                    icon.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, -180f);

                    if (i == 0)
                    {
                        if (m_isBattle)
                        {
                            icon.GetComponent<RectTransform>().localScale = new Vector3(0.65f, 0.65f, 0.65f);
                            var Frame = icon.transform.GetChild(1).GetComponent<Image>();
                            Frame.color = new Color(255, 255, 0);
                        }
                    }
                    else
                    {
                        icon.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 1f);
                    }

                    m_isFirstTurn = false;
                }
            }
        }
    }
    // 턴 아이콘을 세팅하는 메소드
    void SetTurnIcon()
    {
        List<GameObject> tempPlayer = GetPlayers();
        List<GameObject> tempEnemy = GetMonsters();

        int rootIndex = m_turnUI.transform.childCount - 1;
        GameObject root = m_turnUI.transform.GetChild(rootIndex).gameObject;

        // 플레이어 아이콘
        for (int i = 0; i < tempPlayer.Count; i++)
        {
            var player = tempPlayer[i].transform.GetChild(1).GetComponent<CharacterAttack>();
            var icon = Instantiate(player.GetCharacterTurnIcon()) as GameObject;
            icon.GetComponent<RectTransform>().transform.SetParent(GameObject.Find("InstatiateIcon").transform);
            m_turnIconDic.Add(player.transform.parent.name, icon);

            icon.transform.localPosition = new Vector3(icon.transform.localPosition.x, icon.transform.localPosition.y, 0);
            icon.transform.localScale = Vector3.one;
        }

        // 몬스터 아이콘
        for (int j = 0; j < tempEnemy.Count; j++)
        {
            var enemy = tempEnemy[j].transform.GetChild(0).GetComponent<MonsterController>();

            var icon = Instantiate(enemy.GetCharacterTurnIcon()) as GameObject;
            icon.GetComponent<RectTransform>().transform.SetParent(GameObject.Find("InstatiateIcon").transform);
            m_turnIconDic.Add(enemy.transform.parent.name, icon);

            icon.transform.localPosition = new Vector3(icon.transform.localPosition.x, icon.transform.localPosition.y, 0);
            icon.transform.localScale = Vector3.one;
        }
    }

    public void RemoveTurnIcon(GameObject character)
    {
        string deadCharacter = character.transform.parent.name;

        if (m_turnIconDic.TryGetValue(deadCharacter, out var icon))
        {
            Destroy(icon);
            m_characterOrderList.Remove(deadCharacter);
        }
        m_turnIconDic.Remove(deadCharacter);
    }

    void ClearTurnIcon()
    {
        int rootIndex = m_turnUI.transform.childCount - 1;
        GameObject root = m_turnUI.transform.GetChild(rootIndex).gameObject;

        if (root.transform.childCount != 0)
        {
            for (int i = 0; i < root.transform.childCount; i++)
            {
                var icon = root.transform.GetChild(i).gameObject;
                Destroy(icon);
            }
        }
    }

    public void RemoveinList(GameObject character)
    {
        // 만일 리스트에서 캐릭터를 꺼내려할때 해당 캐릭터가 죽어있다면(hp <= 0이라면) 리스트에서 찾아서 제거

        if (m_readyCharacterList.Find(temp => temp.name == character.name))
        {
            m_readyCharacterList.Remove(character);
        }
    }

    public GameObject GetRoot()
    {
        int rootIndex = m_turnUI.transform.childCount - 1;
        return m_turnUI.transform.GetChild(rootIndex).gameObject;
    }

    public GameObject GetPlayerHpObj()
    {
        GameObject hpObj = m_characterUIObj.transform.GetChild(1).gameObject;
        return hpObj;
    }

    public Transform GetSpawnPoint()
    {
        return m_nowSpawnPoint.transform;
    }

    public Transform GetPlayerPoint()
    {
        return m_playerTeamObj.transform;
    }

    public GameObject GetSelectMonster()
    {
        return m_nowEnemy.gameObject;
    }

    public GameObject GetSelectPlayer()
    {
        return m_nowTargetPlayer.gameObject;
    }

    public List<GameObject> GetMonsters()
    {
        List<GameObject> m_monsterList = new List<GameObject>();

        // 문제점 - 첫번째 적 스폰 포인트만 갖고옴
        if (m_isFisrtBattle)
        {
            m_nowSpawnPoint = m_enemyTeamObj.transform.GetChild(0).gameObject;
        }

        var enemyCount = m_nowSpawnPoint.transform.childCount;

        for (int i = 1; i < enemyCount; i++)                                         // 첫번째 자식은 카메라 위치이다.
        {
            // 죽은 개체는 리스트 항목에서 넘겨짐
            if (m_nowSpawnPoint.transform.GetChild(i).GetChild(0).GetComponentInChildren<MonsterController>().m_isDie)
            {
                continue;
            }

            m_monsterList.Add(m_nowSpawnPoint.transform.GetChild(i).gameObject);
        }

        return m_monsterList;
    }

    void EncounterMonsterCount()
    {
        m_encounterMonsterCount = m_nowSpawnPoint.transform.childCount - 1;   // 카메라 위치제외
    }

    public List<GameObject> GetPlayers()
    {
        List<GameObject> playerList = new List<GameObject>();

        for (int i = 0; i < m_playerTeamObj.transform.childCount - 1; i++)
        {
            // 죽은 개체는 리스트 항목에서 넘겨짐
            if (m_playerTeamObj.transform.GetChild(i).GetComponentInChildren<CharacterAttack>().m_isDie)
            {
                continue;
            }

            playerList.Add(m_playerTeamObj.transform.GetChild(i).gameObject);
        }

        return playerList;
    }

    public int GetSkillIndex()
    {
        return m_skillBtnNumber - 1;
    }

    public int GetEnemySkillIndex()
    {
        return m_enemySkillIndex;
    }

    GameObject GetFirstEnemy()
    {
        List<GameObject> tempList = GetMonsters();

        if (tempList[0].transform.GetChild(0).gameObject.activeSelf == false)
            return null;

        return tempList[0].transform.GetChild(0).gameObject;
    }

    public void SetPlayerHp(GameObject player)
    {
        var unit = player.GetComponent<CharacterStatus>();
        var hpObj = GetPlayerHpObj();
        var slider = hpObj.GetComponentInChildren<Slider>();
        var hpText = hpObj.GetComponentInChildren<Text>();

        slider.value = (float)unit.GetHp() / unit.GetMaxHp();
        hpText.text = string.Format("{0} / {1}", unit.GetHp(), unit.GetMaxHp());
    }

    void CheckHighLight()
    {
        if (m_prevOnHighLight != null)
        {
            m_prevOnHighLight.SetActive(false);
        }
    }

    // 스킬 하이라이트
    void OnHighLight(int index)
    {
        m_skillHighLights[index].SetActive(true);

        var highlight = m_skillHighLights[index].transform.GetChild(0).GetComponent<EffectUnit>();
        highlight.gameObject.SetActive(true);
        highlight.Play();

        m_prevOnHighLight = m_skillHighLights[index];
    }

    bool CheckChangeButton(int btnNum)
    {
        // 그 전에 캐릭터가 같은지 체크 -> 캐릭터가 같다면 계속 아니면 false;
        //이전버튼과 같다면 스킬카운트 = 1
        if (m_nowCharacter != m_prevCharacter)
            return false;

        if (m_prevBtnNumber == btnNum)
        {
            m_skillClickCount = 1;
            return true;
        }

        return false;
    }

    void ResetSkillCount()
    {
        m_skillClickCount = 0;
    }


    public void SetPreventTouchFalse()
    {
        m_preventTouchObj.SetActive(false);
    }

    public void SetSkillNotUse()
    {
        m_nowSkillUsing = false;
    }

    public void OnPressSkill_01()
    {
        if (m_nowSkillUsing)
            return;

        m_skillBtnNumber = 1;

        CheckHighLight();
        OnHighLight(m_skillBtnNumber - 1);

        // 스킬의 공격대상 타입을 가져와서 모든적을 선택할지 말지 결정한다.
        m_nowMyTargetType = m_nowCharacter.GetComponent<CharacterStatus>().GetTargetType(GetSkillIndex());
        var character = m_nowCharacter.GetComponent<CharacterAttack>();

        if (character.GetSkillCoomTime(m_skillBtnNumber - 1) != 0)
            return;

        if (m_nowMyTargetType == "MultiTarget")
        {
            TargetSelectAll();
        }
        else if (m_nowMyTargetType == "SingleTarget")
        {
            TargetDeselectAll();
            m_prevTileOnMonster.OnTile();
        }

        // 한번더 눌렀을때 이전에 누른 버튼과 같을때에만 카운트 아닐시 초기화
        // 그러나 타겟 타입이 전체 공격인 경우는 초기화 X0
        if (CheckChangeButton(m_skillBtnNumber))
            m_skillClickCount++;
        else
            m_skillClickCount = 1;

        //스킬 애니메이션이 재생되는 조건
        if (m_skillClickCount > 1)
        {
            m_preventTouchObj.SetActive(true);
            m_nowSkillUsing = true;
            PlayerAttackProcess(m_skillBtnNumber);
        }

        m_prevBtnNumber = m_skillBtnNumber;

    }

    public void OnPressSkill_02()
    {
        if (m_nowSkillUsing)
            return;
        m_skillBtnNumber = 2;

        CheckHighLight();
        OnHighLight(m_skillBtnNumber - 1);

        m_nowMyTargetType = m_nowCharacter.GetComponent<CharacterStatus>().GetTargetType(GetSkillIndex());
        var character = m_nowCharacter.GetComponent<CharacterAttack>();

        if (character.GetSkillCoomTime(m_skillBtnNumber - 1) != 0)
            return;

        if (m_nowMyTargetType == "MultiTarget")
        {
            TargetSelectAll();
        }
        else if (m_nowMyTargetType == "SingleTarget")
        {
            TargetDeselectAll();
            m_prevTileOnMonster.OnTile();
        }

        if (CheckChangeButton(m_skillBtnNumber))
        {
            m_skillClickCount++;
        }
        else
            m_skillClickCount = 1;

        if (m_skillClickCount > 1)
        {
            m_preventTouchObj.SetActive(true);
            m_nowSkillUsing = true;
            PlayerAttackProcess(m_skillBtnNumber);
        }

        m_prevBtnNumber = m_skillBtnNumber;
    }

    public void OnPressSkill_03()
    {
        if (m_nowSkillUsing)
            return;

        m_skillBtnNumber = 3;

        CheckHighLight();
        OnHighLight(m_skillBtnNumber - 1);

        m_nowMyTargetType = m_nowCharacter.GetComponent<CharacterStatus>().GetTargetType(GetSkillIndex());
        var character = m_nowCharacter.GetComponent<CharacterAttack>();

        if (character.GetSkillCoomTime(m_skillBtnNumber - 1) != 0)
            return;

        if (m_nowMyTargetType == "MultiTarget")
        {
            TargetDeselectAll();

            TargetSelectAll();
        }
        else if (m_nowMyTargetType == "SingleTarget")
        {
            if (m_isEnemySelectAll)
            {
                m_preventTouchObj.SetActive(true);
                TargetDeselectAll();
                m_prevTileOnMonster.OnTile();
            }
        }

        if (CheckChangeButton(m_skillBtnNumber))
            m_skillClickCount++;
        else
            m_skillClickCount = 1;

        if (m_skillClickCount > 1)
        {
            m_nowSkillUsing = true;
            PlayerAttackProcess(m_skillBtnNumber);
        }

        m_prevBtnNumber = m_skillBtnNumber;
    }

    // 누른 스킬버튼 별로 캐릭터의 스킬 모션을 재생
    void PlaySkill()
    {
        if (m_nowCharacterType == "Character")
        {
            var character = m_nowCharacter.GetComponent<CharacterAttack>();

            switch (m_skillBtnNumber)
            {
                case 1:
                    character.PlaySkill_01();
                    character.SetState(CharacterAttack.eMoveState.Skill1);
                    break;
                case 2:
                    character.PlaySkill_02();
                    character.SetState(CharacterAttack.eMoveState.Skill2);
                    break;
                case 3:
                    character.PlaySkill_03();
                    character.SetState(CharacterAttack.eMoveState.Skill3);
                    break;
            }
        }
        else if (m_nowCharacterType == "Enemy")
        {
            var enemy = m_nowCharacter.GetComponent<MonsterController>();

            switch (m_enemySkillIndex)
            {
                case 0:
                    enemy.PlaySkill_01();
                    enemy.SetState(MonsterController.eMoveState.Skill1);
                    break;
                case 1:
                    enemy.PlaySkill_02();
                    enemy.SetState(MonsterController.eMoveState.Skill2);
                    break;
            }
        }
    }

    // 공격을 위한 준비 과정
    void BeforeAttackProcess()
    {
        if (m_enterPortal != null)
        {
            var point = m_enterPortal.GetComponent<Portal>();
            if (point != null && point.GetPortalType() == ePortalType.Boss)
            {
                m_bossUIObj.SetActive(true);
                GetBossUI();
            }
        }

        MyTeamDeselectAll();
        if (m_nowCharacterType == "Character")
        {
            SelectFirstEnemy();

            if (m_prevTileOnPlayer != null)
                m_prevTileOnPlayer.OffTile();

            // 캐릭터 선택 표시를 위한 발판을 생성시켜준다.
            var unit = m_nowCharacter.gameObject.GetComponent<CharacterAttack>();
            unit.OnTile();

            m_cameraPath.ChanageCameraPos(m_nowCharacter.transform.parent.GetChild(0).gameObject);

            // 스킬 패널 켜줌
            m_battlePanelObj.SetActive(true);

            for (int i = 0; i < m_battlePanelObj.transform.childCount - 1; i++)
            {
                if (m_battlePanelObj.transform.GetChild(i).gameObject.activeSelf == false)
                {
                    if (i == 2)
                        continue;

                    m_battlePanelObj.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            GetSkillImg();
            ResetSkillCount();
            // 스킬 쿨타임 
            CheckSkillCoolTime();
            // 캐릭터 UI 켜줌
            GetCharacterUI();
            SetPlayerHp(m_nowCharacter);

            OnPressSkill_01();

            if (m_isEnemyAttackProcess)
                m_isEnemyAttackProcess = false;
            m_isAttackProcess = true;

            m_prevTileOnPlayer = unit;
        }
        else if (m_nowCharacterType == "Enemy")
        {
            var unit = m_nowCharacter.gameObject.GetComponent<MonsterController>();

            // 몬스터인 경우 스킬 패널 보여줄 필요 X
            // m_battlePanelObj.SetActive(false);
            for (int i = 0; i < m_battlePanelObj.transform.childCount - 1; i++)
            {
                if (m_battlePanelObj.transform.GetChild(i).gameObject.activeSelf == true)
                {
                    m_battlePanelObj.transform.GetChild(i).gameObject.SetActive(false);
                }
            }

            if (m_prevTileOnMonster != null)
                m_prevTileOnMonster.OffTile();

            unit.OnTile();
            m_cameraPath.ChanageCameraPos(m_nowCharacter.transform.parent.parent.GetChild(0).gameObject);

            if (m_isAttackProcess)
                m_isAttackProcess = false;
            m_isEnemyAttackProcess = true;

            m_prevTileOnMonster = unit;
            EnemyAttackProcess();
        }
    }

    // 적이 공격을 위한 과정
    void EnemyAttackProcess()
    {
        // 어떤 대상을 공격할지 랜덤을 돌린다.
        var enemy = m_nowCharacter.GetComponent<MonsterController>();
        var enemyStat = enemy.GetComponent<CharacterStatus>();
        GameObject target = enemy.RandomAttackCharacter();                                // 공격할 캐릭터를 선택한다.

        m_nowTargetPlayer = target.transform.GetChild(1).GetComponent<CharacterAttack>();

        if (m_prevTileOnPlayer != null)
            m_prevTileOnPlayer.OffTile();

        // 스킬을 선택하는 구간
        m_enemySkillIndex = enemy.SkillSelect();

        // 만일 기본공격이 아닌 스킬을 썼다면 해당 스킬은 당분간 쓰지못한다. 
        if (m_enemySkillIndex != 0)
            enemy.InitSkillCoolTime(m_enemySkillIndex);

        var monsterStat = enemy.GetComponent<CharacterStatus>();
        string attackType = monsterStat.GetAttackType(m_enemySkillIndex);
        string targetType = monsterStat.GetTargetType(m_enemySkillIndex);

        var targetStat = m_nowTargetPlayer.gameObject.GetComponent<CharacterStatus>();

        //Debug.Log(string.Format("상대 체력 : {0}", targetStat.GetHp()));

        if (targetType == "MultiTarget")
        {
            TargetSelectAll();
        }
        else if (targetType == "SingleTarget")
        {
            if (m_isPlayerSelectAll)
            {
                TargetDeselectAll();
            }
            m_nowTargetPlayer.OnTile();
        }

        if (attackType == "Melee")
        {
            enemy.MoveToTarget(m_nowTargetPlayer.gameObject);
            m_nowCharacter.GetComponent<MonsterController>().m_MonsterSkillDel = PlaySkill;
        }
        else
        {
            PlaySkill();
        }

        m_prevTileOnPlayer = m_nowTargetPlayer;
        AfterAttackProcess();
    }

    // 공격을 위한 과정
    void PlayerAttackProcess(int skillBtnNumber)
    {
        int skillIndex = skillBtnNumber - 1;
        CharacterStatus characterStat = m_nowCharacter.GetComponent<CharacterStatus>();
        CharacterAttack characterAttack = m_nowCharacter.GetComponent<CharacterAttack>();
        string skillType = characterStat.GetSkillType(skillIndex);                                                   // 스킬이 공격인지 버프인지 알아온다.
        string attackType = characterStat.GetAttackType(skillIndex);                                      // 스킬이 근접인지 원거리인지 알아온다.

        var monsterStat = m_nowEnemy.gameObject.GetComponent<CharacterStatus>();

        //Debug.Log(string.Format("상대 체력 : {0}",monsterStat.GetHp()));

        if (skillType == "Attack")
        {
            //  스킬 공격 타입이 근접인 경우 선택된 몬스터에게 다가가야한다. 
            if (attackType == "Melee")
            {
                characterAttack.MoveToTarget(m_nowEnemy.gameObject);
                m_nowCharacter.GetComponent<CharacterAttack>().m_characterSkillDel = PlaySkill;
            }
            else if (attackType == "Ranged")
            {
                // 몬스터 방향으로 돌아야한다.
                characterAttack.LookAtTarget(m_nowEnemy.gameObject);
                PlaySkill();
            }
        }
        else if (skillType == "Buff")
        {
            TargetDeselectAll();
            PlaySkill();
        }

        // 턴이 지나면 모든 스킬 -1
        characterAttack.DecreaseCoolTime(1);

        // 스킬이 사용됬다면 쿨타임을 채워준다.
        characterAttack.InitSkillCoolTime(skillIndex);
        AfterAttackProcess();

        characterAttack.SetState(CharacterAttack.eMoveState.Idle);
    }

    // 공격이 끝나고 해줄것
    void AfterAttackProcess()
    {
        m_readyCharacterList.RemoveAt(0);                                           // 꺼낸 캐릭터는 리스트에서 없애준다.
        m_characterOrderList.RemoveAt(0);                                           // 공격한 캐릭터를 리스트에서 없애준다.
    }

    // 캐릭별로 스킬이미지를 바꿔주기 위한 함수
    void GetSkillImg()
    {
        var characterStat = m_nowCharacter.GetComponent<CharacterStatus>();
        var skill1 = m_battlePanelObj.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        var skill2 = m_battlePanelObj.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Image>();
        var skill3 = m_battlePanelObj.transform.GetChild(0).GetChild(0).GetChild(2).GetComponent<Image>();

        skill1.sprite = characterStat.GetCharacterSkillImg(0);
        skill2.sprite = characterStat.GetCharacterSkillImg(1);
        skill3.sprite = characterStat.GetCharacterSkillImg(2);
    }

    void GetCharacterUI()
    {
        var characterStat = m_nowCharacter.GetComponent<CharacterStatus>();
        var characterFace = m_characterUIObj.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        var characterName = m_characterUIObj.transform.GetChild(2).GetChild(0).GetComponent<Text>();

        characterFace.sprite = characterStat.GetCharacterImage();
        characterName.text = characterStat.GetCharacterName();
    }

    void GetBossUI()
    {
        var bossStat = m_bossCharacter.GetComponent<CharacterStatus>();
        var bossFace = m_bossUIObj.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        var bossName = m_bossUIObj.transform.GetChild(3).GetChild(0).GetComponent<Text>();

        bossFace.sprite = bossStat.GetCharacterImage();
        bossName.text = string.Format("Boss : {0}", bossStat.GetCharacterName());
    }

    // 캐릭별로 스킬 쿨타임 체크해서 행동할 메소드
    void CheckSkillCoolTime()
    {
        if (m_nowCharacterType == "Character")
        {
            var character = m_nowCharacter.GetComponent<CharacterAttack>();

            if (character.m_isFirstBattle)
            {
                character.SetNoCoolTime();
                character.m_isFirstBattle = false;
            }

            List<int> coolTimeList = character.GetSkillCoolTimeList();

            // 이전 캐릭터가 아닌 다른 캐릭터가 들어온다면 이전캐릭터의 쿨타임 창은 꺼준다.
            if (m_nowCharacter != m_prevCharacter)
            {
                for (int i = 0; i < coolTimeList.Count; i++)
                    m_skillCoolTimeObj[i].SetActive(false);
            }


            for (int i = 0; i < coolTimeList.Count; i++)
            {
                // 스킬 쿨타임이 0이 아니라면 못쓰게 쿨타임 창을 켜준다.
                if (coolTimeList[i] != 0)
                {
                    m_skillCoolTimeObj[i].SetActive(true);
                    var coolTimeText = m_skillCoolTimeObj[i].GetComponentInChildren<Text>();
                    coolTimeText.text = (coolTimeList[i]).ToString();
                }
            }
        }
    }
    //첫번째 몬스터 선택하는 함수
    void SelectFirstEnemy()
    {
        if (GetFirstEnemy() != null)
        {
            m_nowEnemy = GetFirstEnemy().GetComponent<MonsterController>();
            m_nowEnemy.OnTile();
            m_prevTileOnMonster = m_nowEnemy;
        }
    }

    void TargetSelectAll()
    {
        if (m_nowCharacterType == "Character")
        {
            //m_nowEnemy = null;
            List<GameObject> monster = GetMonsters();

            if (m_prevTileOnMonster != null)
                m_prevTileOnMonster.OffTile();

            for (int i = 0; i < monster.Count; i++)
            {
                monster[i].transform.GetChild(0).GetComponent<MonsterController>().OnTile();
            }
            m_isEnemySelectAll = true;
        }
        else if (m_nowCharacterType == "Enemy")
        {
            List<GameObject> player = GetPlayers();

            if (m_prevTileOnPlayer != null)
                m_prevTileOnPlayer.OffTile();

            for (int i = 0; i < player.Count; i++)
            {
                player[i].transform.GetChild(1).GetComponent<CharacterAttack>().OnTile();
            }
            m_isPlayerSelectAll = true;
        }
    }

    void TargetDeselectAll()
    {
        if (m_nowCharacterType == "Character")
        {
            List<GameObject> monster = GetMonsters();

            if (m_prevTileOnMonster != null)
                m_prevTileOnMonster.OffTile();

            for (int i = 0; i < monster.Count; i++)
            {
                monster[i].transform.GetChild(0).GetComponent<MonsterController>().OffTile();
            }

            m_isEnemySelectAll = false;
        }
        else if (m_nowCharacterType == "Enemy")
        {
            List<GameObject> player = GetPlayers();

            if (m_prevTileOnPlayer != null)
                m_prevTileOnPlayer.OffTile();

            for (int i = 0; i < player.Count; i++)
            {
                player[i].transform.GetChild(1).GetComponent<CharacterAttack>().OffTile();
            }

            m_isPlayerSelectAll = false;
        }
    }

    void MyTeamDeselectAll()
    {
        if (m_nowCharacterType == "Character")
        {
            List<GameObject> player = GetPlayers();

            if (m_prevTileOnPlayer != null)
                m_prevTileOnPlayer.OffTile();

            for (int i = 0; i < player.Count; i++)
            {
                player[i].transform.GetChild(1).GetComponent<CharacterAttack>().OffTile();
            }
        }
        else if (m_nowCharacterType == "Enemy")
        {
            List<GameObject> monster = GetMonsters();

            if (m_prevTileOnMonster != null)
                m_prevTileOnMonster.OffTile();

            for (int i = 0; i < monster.Count; i++)
            {
                monster[i].transform.GetChild(0).GetComponent<MonsterController>().OffTile();
            }
        }
    }

    // 몬스터나 플레이어 전멸 체크
    bool CheckBattleEnd()
    {
        List<GameObject> tempEnemyList = GetMonsters();         // 죽은 몬스터는 리스트에 없다.
        List<GameObject> tempPlayerList = GetPlayers();              // 죽은 플레이어는 리스트에 없다.

        // 몬스터가 전멸했는지 체크
        if (tempEnemyList.Count == 0)
        {
            m_isBattle = false;
            m_turnUI.SetActive(false);
        }
        else if (tempPlayerList.Count == 0)                                       // 플레이어 전멸 체크 전멸시 게임 오버 화면 띄우기
        {
            m_gameoverPanel.gameObject.SetActive(true);


            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.StopBGM();
                SoundManager.Instance.PlayUISound(SoundManager.eUISoundAudioClip.Defeat);
            }
            m_gameoverPanel.SetDefeatPanel();

            m_isBattle = false;
        }
        else
            m_isBattle = true;


        return m_isBattle;
    }

    // 전투가 끝나고 처리할 과정들
    void EndProcess()
    {
        // 들어간 포탈 클리어 처리 해주고 꺼주기
        if (m_enterPortal != null)
        {
            m_enterPortal.SetClear();
            m_enterPortal.gameObject.SetActive(false);
        }

        ClearTurnIcon();
        m_characterOrderList.Clear();
        m_turnIconDic.Clear();
        m_prevBtnNumber = 0;
        m_mapManager.ClearCountPlus();
        m_deadMonsterList.Clear();
        m_readyCharacterList.Clear();
        m_isFisrtBattle = false;
        m_skillClickCount = 0;
    }

    void SettingCharacters()
    {
        if (LobbyDataManager.Instance == null)
            return;

        int characterNum = LobbyDataManager.Instance.GetPartyNum();

        for (int i = 0; i < characterNum; i++)
        {
            var path = string.Format("Character/BattleCharacter/{0}", LobbyDataManager.Instance.GetCharacterName(i));
            var prefab = Resources.Load(path);
            var character = Instantiate(prefab as GameObject, m_playerTeamObj.transform.GetChild(i).transform, false);

            //character.transform.SetParent(m_playerTeamObj.transform.GetChild(i).transform);
            character.transform.localPosition = Vector3.zero;
            character.transform.localScale = Vector3.one;
            character.transform.localRotation = Quaternion.identity;
        }

        m_isSettingEnd = true;    
    }

    void InitProcess()
    {
        m_mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        m_playerTeamObj.gameObject.SetActive(true);
        m_enemyTeamObj.gameObject.SetActive(true);
        m_isSettingEnd = false;
        m_nowSpawnPoint = m_enemyTeamObj.transform.GetChild(0).gameObject;
        // 스킬창과 관련된 개체들 false로 
        for (int i = 0; i < m_skillHighLights.Length; i++)
        {
            m_skillHighLights[i].SetActive(false);
            m_skillCoolTimeObj[i].SetActive(false);
        }


        m_cameraPath = GameObject.Find("Main Camera").GetComponent<CameraPath>();
        m_miniMap = GameObject.Find("MiniMapCamera").GetComponent<MiniMap>();
        m_bossUIObj.SetActive(false);
        m_battlePanelObj.SetActive(false);
        m_gameoverPanel.gameObject.SetActive(false);
        m_gameClearPanel.gameObject.SetActive(false);
        m_isFisrtBattle = true;
    }

    // Start is called before the first frame update
    protected override void OnAwake()
    {
        base.OnAwake();

        InitProcess();
    }

    protected override void OnStart()
    {
        base.OnStart();

        m_preventTouchObj.SetActive(false);

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayBGM(LoadSceneManager.eScene.Game_Castle_01);

        SettingCharacters();
        SetTurnIcon();

        m_isFirstTurn = true;
    }

    // Update is called once per frame
    void Update()
    {
        // 배틀이 시작되거나 하는중이라면
        if (m_isBattle)
        {
            if (m_ListFull)
            {
                m_checkTime = m_checkTime + Time.deltaTime;

                if (m_checkTime > WAIT_TIME)
                {
                    m_ListFull = false;
                    m_checkTime = 0f;

                    BeforeAttackProcess();
                    m_prevCharacter = m_nowCharacter;
                }
            }

            if (m_isAttackProcess)
            {
                if (m_nowSkillUsing)
                    return;

                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit rayHit;

                    if (Physics.Raycast(ray, out rayHit, 1000f, 1 << LayerMask.NameToLayer("Enemy")))
                    {
                        m_nowEnemy = rayHit.transform.gameObject.GetComponent<MonsterController>();

                        Debug.Log(string.Format("현재 선택된 몬스터 : {0}", m_nowEnemy.transform.parent.name));

                        // 멀티 타겟이 아닌경우 다른 몬스터 선택 시 선택한 몬스터를 제외하고 다른 몬스터의 발판을 꺼준다.
                        if (m_nowMyTargetType != "MultiTarget")
                        {
                            if (m_prevTileOnMonster != null)
                            {
                                var other = m_prevTileOnMonster.GetComponent<MonsterController>();
                                other.OffTile();
                            }

                            m_nowEnemy.OnTile();
                        }

                        // 쿨타임이 0이라면
                        if (m_nowCharacter.GetComponent<CharacterAttack>().GetSkillCoomTime(m_skillBtnNumber - 1) == 0)
                        {
                            // 한번 클릭시에만 공격이 나가게한다.
                            if (m_targgetClickCount <= 1)
                            {
                                // 스킬이 두번 이상 나가게 하는것 방지
                                if (m_isAttackProcess == true)
                                    m_isAttackProcess = false;

                                //선택 시 바로 공격이 나간다.
                                PlayerAttackProcess(m_skillBtnNumber);
                            }
                        }
                        m_prevTileOnMonster = m_nowEnemy;
                    }
                }
            }
        }
    }
}
