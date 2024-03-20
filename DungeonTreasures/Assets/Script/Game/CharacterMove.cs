using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMove : MonoBehaviour
{
    [SerializeField]
    int m_speed = 5;
    [SerializeField]    
    bool m_portalEnterCheck;

    GameObject m_enemySpawnPoint;
    AnimationController m_animController;
    CharacterController m_characterController;
    CharacterStatus m_characterStat;
   // BattleManager m_battleManager = null;
    MapManager m_mapManager;
    Vector3 m_dir;

    int m_characterId;
    string m_state;


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 중복 감지 방지
        if (m_portalEnterCheck)
            return;

        if (hit.gameObject.tag.Equals("Portal"))
        {
            m_portalEnterCheck = true;
            GameObject spawnPoint = hit.gameObject;
            var portalInfo = spawnPoint.GetComponent<Portal>();
            MapManager.Instance.SetEnemySpawnPoint(spawnPoint);                            // 맵 매니저에게 현재 닿은 포탈이 무엇인지 알려준다.

            if(portalInfo.GetPortalType() == ePortalType.Enemy || portalInfo.GetPortalType() == ePortalType.Boss)
            {
                if(SoundManager.Instance != null)
                {
                    SoundManager.Instance.PlayUISound(SoundManager.eUISoundAudioClip.Transport);
                }
                Debug.Log("CharacterMove");
                MapManager.Instance.PanelSwitch.m_callEvent = BattleManager.Instance.SetBattleManager;
                MapManager.Instance.PanelSwitch.m_endCallEvent = BattleManager.Instance.AfterAnimEndProcess;
                MapManager.Instance.PanelSwitch.SetSwitchPanel();
            }
            else if(portalInfo.GetPortalType() == ePortalType.Buff)
            {
                
                // 버프 포탈이므로 꺼준다.
                spawnPoint.SetActive(false);
                // 버프 구현
                int buff = MapManager.Instance.RandomBuff();
                List<GameObject> tempPlayers = BattleManager.Instance.GetPlayers();

                // 버프를 적용하는 부분
                if(buff == 0)           // 쿨타임 초기화
                {
                    for(int i = 0; i < tempPlayers.Count; i++)
                    {
                        tempPlayers[i].transform.GetChild(1).GetComponent<CharacterAttack>().InitCoolTime();
                    }
                }
                else if (buff == 1)    // 공격력 증가
                {
                    for (int i = 0; i < tempPlayers.Count; i++)
                    {
                        tempPlayers[i].transform.GetChild(1).GetComponent<CharacterAttack>().SetPowerUp(10f);
                    }
                }
                else if(buff == 2)   // 방어력 증가
                {
                    for (int i = 0; i < tempPlayers.Count; i++)
                    {
                        tempPlayers[i].transform.GetChild(1).GetComponent<CharacterAttack>().SetDefenceUp(10f);
                    }
                }
                else if(buff == 3)  // 힐
                {
                    for (int i = 0; i < tempPlayers.Count; i++)
                    {
                        tempPlayers[i].transform.GetChild(1).GetComponent<CharacterAttack>().PercentHeal(10);
                    }
                }

                switch (buff)
                {
                    case 0:                                                     // ReduceCoolTime     팝업창
                        MapManager.Instance.ShowInfo(string.Format("모든 캐릭터의 스킬 쿨타임 초기화."));
                        break;
                    case 1:                                                     // PowerUp                  아이콘
                        //m_mapManager.ShowBuffIcon(MapManager.ePartyBuff.PowerUp);
                        MapManager.Instance.ShowInfo(string.Format("모든 캐릭터의 공격력 10% 증가"));
                        break;
                    case 2:                                                     // DefenceUp               아이콘
                                                                                // m_mapManager.ShowBuffIcon(MapManager.ePartyBuff.DefenceUp);
                        MapManager.Instance.ShowInfo(string.Format("모든 캐릭터의 방어력 10% 증가"));
                        break;
                    case 3:                                                     // Heal
                        MapManager.Instance.ShowInfo(string.Format("모든 캐릭터의 Hp 10% 회복"));
                        break;
                }
                                 
                spawnPoint.GetComponent<Portal>().SetClear();
                MapManager.Instance.ShowPortal();
            }
        }
    }

    #region Animation

    void PlayFowardAnimation()
    {
        m_characterId = m_characterStat.GetCharacterId();

        if (m_characterId == 0)
            return;

        switch(m_characterId)
        {
            case 1:  //나이트
                ((KnightAnimController)m_animController).Play(KnightAnimController.eAnimState.Foward);
                m_state = ((KnightAnimController)m_animController).GetAniState().ToString();
                break;
            case 2: // 파이터
                ((FighterAnimController)m_animController).Play(FighterAnimController.eAnimState.Foward);
                m_state = ((FighterAnimController)m_animController).GetAniState().ToString();
                break;
            case 3: // 아쳐
                ((ArcherAnimController)m_animController).Play(ArcherAnimController.eAnimState.Foward);
                m_state = ((ArcherAnimController)m_animController).GetAniState().ToString();
                break;
            case 4: // 매지션
                ((MagicianAnimController)m_animController).Play(MagicianAnimController.eAnimState.Foward);
                m_state = ((MagicianAnimController)m_animController).GetAniState().ToString();
                break;
        }

    }

    void PlayIdleAnim()
    {
        m_characterId = m_characterStat.GetCharacterId();

        if (m_characterId == 0)
            return;

        switch (m_characterId)
        {
            case 1:  //나이트
                ((KnightAnimController)m_animController).Play(KnightAnimController.eAnimState.Idle);
                m_state = ((KnightAnimController)m_animController).GetAniState().ToString();
                break;
            case 2: // 파이터
                ((FighterAnimController)m_animController).Play(FighterAnimController.eAnimState.Idle);
                m_state = ((FighterAnimController)m_animController).GetAniState().ToString();
                break;
            case 3: // 아쳐
                ((ArcherAnimController)m_animController).Play(ArcherAnimController.eAnimState.Idle);
                m_state = ((ArcherAnimController)m_animController).GetAniState().ToString();
                break;
            case 4: // 매지션
                ((MagicianAnimController)m_animController).Play(MagicianAnimController.eAnimState.Idle);
                m_state = ((MagicianAnimController)m_animController).GetAniState().ToString();
                break;
        }
    }
    #endregion
    
    public void OutPortal()
    {
        m_portalEnterCheck = false;
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (LoadSceneManager.Instance != null)
        {
            if (LoadSceneManager.Instance.NowScene() == "Lobby")
                this.enabled = false;
            else
            {
                //m_battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();                                          // 자식의 게임 오브젝트를 가져온다
                //m_mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
                m_animController = gameObject.GetComponent<AnimationController>();
                m_characterController = gameObject.GetComponent<CharacterController>();
                m_characterStat = gameObject.GetComponent<CharacterStatus>();
            }
        }
    }
           
    private void Start()
    {
    }

    public void Set()
    {
        m_portalEnterCheck = false;

        gameObject.transform.position = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf == true)
        {
            GamePad gamePad = MapManager.Instance.GamePad;
            m_dir = new Vector3(gamePad.GetAxis().x, 0f, gamePad.GetAxis().y);
            float dist = gamePad.GetDist();

            if (m_dir != Vector3.zero)
            {
                if(m_state != "Foward")
                {
                    PlayFowardAnimation();
                }

                transform.forward = m_dir;
                m_characterController.SimpleMove(m_dir * dist * m_speed * Time.deltaTime);            
            }
            else
            {
                if (m_state != "Idle")
                {
                    PlayIdleAnim();
                }
            }        

        }
    }
}
