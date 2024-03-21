using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum eMonsterDamageType
{
    None,
    Normal,
    Critical,
    Miss,
    Block
}
public enum eMonsterTpye
{
    None,
    Normal,
    Boss
}

public class MonsterController : MonoBehaviour
{
    public enum eMoveState
    {
        Idle,
        Move,
        Skill1,
        Skill2,
        Skill3,
        Die
    }

    [SerializeField]
    EffectUnit m_monsterTile = null;
    [SerializeField]
    bool m_attackReady = false;
    [SerializeField]
    float m_stopDistance = 0;
    [SerializeField]
    float m_moveSpeedTime = 1f;
    [SerializeField]
    GameObject m_monsterHpBar = null;
    [SerializeField]
    GameObject m_monsterSkillCoolTimeBar = null;
    [SerializeField]
    FloatText m_hudText = null;
    [SerializeField]
    eMonsterTpye m_monsterType = eMonsterTpye.None;

    [SerializeField]
    Renderer m_renderer = null;
    MaterialPropertyBlock m_materialProperty;

    public bool m_isTileOn = false;

    AnimationController m_animController;
    Animator m_animator;
    BattleManager m_battleManager;
    CharacterController m_characterController;
    CharacterStatus m_monsterStatus;
    [SerializeField]
    eMoveState m_state;
    TweenMove m_tweenMove;
    string m_skillTargetType;
    string m_nowAttackType;
    Quaternion m_initRotation;
    Vector3 m_initPos;
    GameObject m_selectPlayer;
    CameraPath m_cameraPath;
    [SerializeField]
    GameObject m_turnIcon =null;
    
    float m_waitTime = 2f;
    float m_startTime = 0f;
    float m_curActGauge = 0;
    float m_maxActGauge = 5;
    float m_damage = 0;

    bool m_isCameraDone;
    public bool m_isDie = false;

    List<int> m_skillCoolTimeList = new List<int>();                                   // 스킬에 관련된 쿨타임만을 저장할 리스트

    public delegate void MonsterSkill();
    public MonsterSkill m_MonsterSkillDel = null;


    public void SetReady()
    {
        m_attackReady = false;
        SetState(eMoveState.Idle);
    }

    public GameObject GetEnemyHpBar()
    {
        if (m_monsterHpBar == null)
            return null;

        return m_monsterHpBar;
    }

    public eMonsterTpye GetMonsterType()
    {
        return m_monsterType;
    }

    public GameObject GetCharacterTurnIcon()
    {
        return m_turnIcon;
    }

    void SetSkillCoolTimeInfo()
    {
        for(int i = 0; i <m_monsterStatus.GetSkillCount(); i++)
        {
            m_skillCoolTimeList.Add(m_monsterStatus.GetSkillCoolTime(i));
        }
    }

    public void DecreaseCoolTime(int value)
    {
        // 한턴이 지나면 모든 스킬쿨타임이 줄어든다.
        for (int i = 0; i < m_skillCoolTimeList.Count; i++)
        {
            // 쿨타임을 감소시키고자 하는 스킬이 감소할 필요가 없다면 다음 스킬로
            if (m_skillCoolTimeList[i] == 0)
                continue;

            m_skillCoolTimeList[i] = m_skillCoolTimeList[i] - value;
        }
    }
    
    // 스킬 초기화
    public void InitSkillCoolTime(int index)
    {
        var coolTimeBar = m_monsterSkillCoolTimeBar.GetComponentInChildren<Slider>();

        m_skillCoolTimeList[index] = m_monsterStatus.GetSkillCoolTime(index);
        coolTimeBar.value = 0;       
    }

    public void OnTile()
    {
        if (m_monsterTile != null)
        {
            //타일을 켜준다.
            m_isTileOn = true;
            m_monsterTile.gameObject.SetActive(true);
            m_monsterTile.Play();
        }
    }

    public void OffTile()
    {
        if (m_monsterTile != null)
        {
            m_isTileOn = false;
            m_monsterTile.gameObject.SetActive(false);
        }
    }

    public void SetDamaged(float damage, ePlayerDamageType type)
    {
        if (m_isDie)
            return;
        
        if(m_hudText != null)
            m_hudText.SetEnemyHudText(gameObject, type, (int)damage);

        m_monsterStatus.DecreaseHp((int)damage);

        if (m_monsterHpBar != null)
        {
            var hpbar = m_monsterHpBar.GetComponentInChildren<Slider>();
            hpbar.value = (float)m_monsterStatus.GetHp() / m_monsterStatus.GetMaxHp();

            if (m_monsterType == eMonsterTpye.Boss)
            {
                var hpText = m_monsterHpBar.GetComponentInChildren<Text>();
                hpText.text = string.Format("{0} / {1}", m_monsterStatus.GetHp(), m_monsterStatus.GetMaxHp());
            }
            //Debug.Log(string.Format("{0}의 남은 Hp : {1}", gameObject.transform.parent.name, m_monsterStatus.GetHp()));
        }

        SetDamagedColor();
        Invoke("SetDefaultColor",0.2f);

        if (m_monsterStatus.GetHp() <= 0)
        {
            m_isDie = true;
            SetDie();
            return;
        }

        PlayHitAnim();
    }
    
    public int SkillSelect()
    {
        // 디폴트는 기본공격
        int selectSkill = 0;
        int skillCnt = m_skillCoolTimeList.Count;

        for (int i = 1; i < skillCnt; i++)
        {
            // 스킬이 두개밖에 없고
            if(skillCnt == 1)
            {
                // 해당 스킬이 쿨타임이 0이라면 해당 스킬을 사용할 수 있다.
                if (m_skillCoolTimeList[1] == 0)
                {
                    selectSkill = 1;
                }
                else  // 아니면 빠져나온다.
                    break;
            }
            else   // 스킬이 3개 이상인 경우
            {
                // 쿨타임이 0이라면 현재 선택한 스킬
                if (m_skillCoolTimeList[i] == 0)
                {
                    selectSkill = i;
                }
                else  // 쿨타임이 0이 아니라면 다음 스킬 쿨타임을 살펴본다.
                {
                    continue;
                }
            }
        }
        return selectSkill;
    }

    public GameObject RandomAttackCharacter()
    {
        List<GameObject> players = new List<GameObject>();
        players = m_battleManager.GetPlayers();

        var randomPlayer = Random.Range(0,players.Count);

        switch(randomPlayer)
        {
            case 0:
                if(players[0] != null)
                {                  
                    m_selectPlayer = players[0];
                }
                break;
            case 1:
                if (players[1] != null)
                {
                    m_selectPlayer = players[1];
                }
                break;
            case 2:
                if (players[2] != null)
                {
                    m_selectPlayer = players[2];
                }
                break;
            case 3:
                if (players[3] != null)
                {
                    m_selectPlayer = players[3];
                }
                break;
        }
        return m_selectPlayer;
    }

    public void MoveToTarget(GameObject target)
    {
        float dist = Vector3.Distance(transform.position, target.transform.position);
        var dir = target.transform.position - transform.position;
        int skillIndex = m_battleManager.GetEnemySkillIndex();

        m_skillTargetType = m_monsterStatus.GetTargetType(skillIndex);               // 현재 사용하는 스킬의 공격범위를 가져온다.
        m_stopDistance = m_monsterStatus.GetAttackRange(skillIndex);                // 스킬별로 공격거리를 얻어온다.
        m_initPos = transform.parent.position;                                                          // 캐릭터의 원래 위치를 저장한다.

        transform.forward = dir;
        m_animator.speed = 1.5f;
        m_tweenMove.m_playAnimation = aEvent_Foward;

        // 다중타겟인경우 캐릭터의 포인터 지점에 가서 때린다.
        if (m_skillTargetType == "MultiTarget")
        {
            var point = m_battleManager.GetPlayerPoint();
 
            m_tweenMove.MoveToTween(transform.position, new Vector3(point.position.x, transform.position.y, point.position.z) - new Vector3(dir.x, 0f, dir.z).normalized * m_stopDistance,
                                                            m_moveSpeedTime - (m_moveSpeedTime - (dist / 10f)));
        }
        else if (m_skillTargetType == "SingleTarget" && target != null)
        {
            m_tweenMove.MoveToTween(transform.position, new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z) - new Vector3(dir.x, 0f, dir.z).normalized * m_stopDistance,
                                                            m_moveSpeedTime - (m_moveSpeedTime - (dist / 10f)));
        }

        m_tweenMove.m_onFinish = RegisteredSkill;
    }

    void RegisteredSkill()
    {
        m_animator.speed = 1f;
        // 상대방쪽으로 움직인 후 다음에 쓸 스킬
        m_MonsterSkillDel();
    }


    IEnumerator DelayBackMoveTime()
    {
        yield return new WaitForSeconds(.5f);
        BackMyPos();
    }

    public void BackMyPos()
    {
        m_tweenMove.m_playAnimation = null;
        float dist = Vector3.Distance(transform.position, m_initPos);
        m_tweenMove.MoveToTween(transform.position, m_initPos, m_moveSpeedTime - (m_moveSpeedTime - (dist / 10f)));
        m_tweenMove.m_onFinish = PlayIdleAnim;

        //마지막 스킬만 스킬바에 반영
        if (m_monsterSkillCoolTimeBar != null)
        {
            var coolTimeBar = m_monsterSkillCoolTimeBar.GetComponentInChildren<Slider>();
            coolTimeBar.value = 1.0f - ((float)m_skillCoolTimeList[m_skillCoolTimeList.Count - 1] / m_monsterStatus.GetSkillCoolTime(m_skillCoolTimeList.Count - 1));
        }

        if(m_monsterHpBar != null)
            m_monsterHpBar.transform.localRotation = Quaternion.identity;

        if (m_monsterSkillCoolTimeBar != null)
            m_monsterSkillCoolTimeBar.transform.localRotation = Quaternion.identity;

        transform.localRotation = m_initRotation;      
    }

    public void SetState(eMoveState state)
    {
        m_state = state;
    }

     eMonsterDamageType AttackPlayer(GameObject player)
    {
        CharacterAttack target = player.GetComponent<CharacterAttack>();
        eMonsterDamageType damageType = eMonsterDamageType.None;
        int skillIndex = m_battleManager.GetEnemySkillIndex();
        m_damage = 0f;
        var targetStat = player.GetComponent<CharacterStatus>();

        if (Calculation.AttackDecision(m_monsterStatus.GetHitRate() + m_monsterStatus.GetSkillHitRate(skillIndex) > m_monsterStatus.GetHitRate() ? 100f : m_monsterStatus.GetHitRate(), targetStat.GetDodgeRate()))
        {
            m_damage = Calculation.NormalDamage(m_monsterStatus.GetAtk(), m_monsterStatus.GetSkillValue(skillIndex), targetStat.GetDef());
            damageType = eMonsterDamageType.Normal;

            if (Calculation.CriticalDecision(m_monsterStatus.GetCriRate()))
            {
                Debug.Log("크리티컬!");
                m_damage = Calculation.CriticalDamage(m_damage, 50);
                damageType = eMonsterDamageType.Critical;
            }

            // 데미지 최소 ,최대 대미지
            m_damage = Mathf.Max(0, m_damage + Random.Range(0, 31));

            if (m_damage <= 0)
                damageType = eMonsterDamageType.Block;
        }
        else
        {
            damageType = eMonsterDamageType.Miss;
        }
       
        //Debug.Log(string.Format("{0}이 준 대미지 : {1}", gameObject.name, m_damage));

        return damageType;
    }
    
    // 죽었을때 발생할 메소드
    public void SetDie()
    {
        m_battleManager.RemoveTurnIcon(gameObject);
        aEvent_PlayDamagedSfx();
    
        int id = m_monsterStatus.GetCharacterId();

        switch (id)
        {
            case 1001:
                aEvent_PlayHItSfx(1001);
                break;
            case 1002:
                aEvent_PlayHItSfx(1002);
                break;
        }

        if(m_monsterType == eMonsterTpye.Boss)
        {
            List<GameObject> temp = m_battleManager.GetMonsters();

            // 보스를 죽이면 남은 몬스터도 모두 죽이기
            for (int i = 0; i < temp.Count; i++)
            {
                var mon = temp[i].transform.GetChild(0).GetComponent<MonsterController>();

                if (mon.GetMonsterType() == eMonsterTpye.Boss)
                    continue;
                else
                {
                    mon.m_isDie = true;
                    mon.SetDie();
                }
            }
        }
     
        m_monsterStatus.SetHp(0);
        m_battleManager.RemoveinList(gameObject);
        if (m_monsterTile.enabled == true)
        {
            OffTile();
        }

        PlayDieAnim();    
    }

    void aEvent_RemoveMonster()
    {
        m_battleManager.ScreenSwitch(m_isDie, gameObject,m_monsterType);

        if (m_monsterType == eMonsterTpye.Normal)
            transform.parent.gameObject.SetActive(false);      
    }
    
    void ActiveGaugeProcess()
    {
        int speed = m_monsterStatus.GetCharacterSpeed();

        m_curActGauge = m_curActGauge + (Time.deltaTime * (speed / m_maxActGauge));

        if (m_curActGauge >= m_maxActGauge)
        {
            m_curActGauge = 0;
            m_attackReady = true;
            SetState(eMoveState.Move);
            m_battleManager.GetAttackReady(m_attackReady, gameObject);        
        }
    }

    #region PlaySkill
    public void PlaySkill_01()
    {
        switch (m_monsterStatus.GetCharacterId())
        {
            case 1001:  // 뮤턴트
                ((CreatureAnimController)m_animController).Play(CreatureAnimController.eAnimState.Skill_01);
                break;
            case 1002: // 보스 트롤 
                ((BossAnimController)m_animController).Play(BossAnimController.eAnimState.Skill_01);
                break;
        }
    }

    public void PlaySkill_02()
    {
        switch (m_monsterStatus.GetCharacterId())
        {
            case 1001:  //뮤턴트
                ((CreatureAnimController)m_animController).Play(CreatureAnimController.eAnimState.Skill_02);
                break;
            case 1002: // 보스 트롤 
                ((BossAnimController)m_animController).Play(BossAnimController.eAnimState.Skill_02);
                break;
        }
    }

    public void PlaySkill_03()
    {
        switch (m_monsterStatus.GetCharacterId())
        {
            case 1001:  //뮤턴트
                ((CreatureAnimController)m_animController).Play(CreatureAnimController.eAnimState.Skill_03);
                break;
            case 1002: // 보스 트롤 
                //((BossAnimController)m_animController).Play(BossAnimController.eAnimSate.Skill_02, false);
                break;
        }
    }

    void PlayIdleAnim()
    {
        switch (m_monsterStatus.GetCharacterId())
        {
            case 1001:  //뮤턴트
                ((CreatureAnimController)m_animController).Play(CreatureAnimController.eAnimState.Idle);
                break;
            case 1002: // 보스 트롤 
                ((BossAnimController)m_animController).Play(BossAnimController.eAnimState.Idle);
                break;
        }
        SetState(eMoveState.Idle);
    }

    void PlayHitAnim()
    {
        switch (m_monsterStatus.GetCharacterId())
        {
            case 1001:
                ((CreatureAnimController)m_animController).Play(CreatureAnimController.eAnimState.Hit, false);
                aEvent_PlayDamagedSfx();
                aEvent_PlayHItSfx(1001);
                break;
            case 1002:
                ((BossAnimController)m_animController).Play(BossAnimController.eAnimState.Hit);
                aEvent_PlayDamagedSfx();
                aEvent_PlayHItSfx(1002);
                break;
        }
    }

    void PlayDieAnim()
    {
        switch (m_monsterStatus.GetCharacterId())
        {
            case 1001:  //뮤턴트
                ((CreatureAnimController)m_animController).Play(CreatureAnimController.eAnimState.Die, false);
                break;
            case 1002: // 보스 트롤 
                ((BossAnimController)m_animController).Play(BossAnimController.eAnimState.Die, false);
                break;
        }
    }

    void aEvent_Foward()
    {
        switch (m_monsterStatus.GetCharacterId())
        {
            case 1001:  //나이트
                if (((CreatureAnimController)m_animController).GetAniState() != CreatureAnimController.eAnimState.Foward)
                    ((CreatureAnimController)m_animController).Play(CreatureAnimController.eAnimState.Foward);
                break;
            case 1002: // 파이터
                if (((BossAnimController)m_animController).GetAniState() != BossAnimController.eAnimState.run)
                    ((BossAnimController)m_animController).Play(BossAnimController.eAnimState.run);
                break;
        }
    }

    #endregion
    #region Animation

    public void aEvent_Attack()
    {
        eMonsterDamageType type = eMonsterDamageType.None;
        int skillIndex = m_battleManager.GetEnemySkillIndex();
        m_nowAttackType = m_monsterStatus.GetAttackType(skillIndex);
        m_skillTargetType = m_monsterStatus.GetTargetType(skillIndex);

        // 스킬 타입별로 몬스터 전체에게 이펙트를 넣을지 말지 선택하는 구간
        if (m_skillTargetType == "SingleTarget")
        {
            var target = m_battleManager.GetSelectPlayer();
            var pos = Util.FindChildObject(target, "Dummy_Hit").transform.position;

            if (target != null && pos != null)
            {
                var effectId = m_monsterStatus.GetEffectId(m_battleManager.GetEnemySkillIndex());
                var effectData = TableEffect.Instance.GetData(effectId);
                var effect = EffectPool.Instance.Create(effectData.Prefab[0]);

                effect.transform.position = pos;
                effect.transform.LookAt(transform.position);

                type = AttackPlayer(target);
                target.GetComponent<CharacterAttack>().SetDamagaed(m_damage, type);
            }
        }
        else if (m_skillTargetType == "MultiTarget")
        {
            List<GameObject> players = m_battleManager.GetPlayers();
            Vector3[] pos = new Vector3[players.Count];

            for (int i = 0; i < players.Count; i++)
            {
                pos[i] = Util.FindChildObject(players[i], "Dummy_Hit").transform.position;
            }

            if (players != null && pos != null)
            {
                for (int i = 0; i < players.Count; i++)
                {
                    var effectId = m_monsterStatus.GetEffectId(m_battleManager.GetEnemySkillIndex());
                    var effectData = TableEffect.Instance.GetData(effectId);
                    var effect = EffectPool.Instance.Create(effectData.Prefab[0]);

                    effect.transform.position = pos[i];
                    effect.transform.rotation = Quaternion.Euler(players[i].transform.rotation.x + 140, 0f, 0f);

                    var target = players[i].transform.GetChild(1).gameObject;
                    type = AttackPlayer(target);
                    target.GetComponent<CharacterAttack>().SetDamagaed(m_damage, type);
                }
            }
        }

        // 근접 공격이 끝나면 다시 돌아옴
        switch (m_nowAttackType)
        {
            case "Melee":
                StartCoroutine("DelayBackMoveTime");
                break;
            case "Ranged":
                transform.rotation = Quaternion.identity;
                break;
        }

        // 일반공격이 끝나면 쿨타임이 있는경우 스킬쿨타임을 줄여준다. 그러나 스킬을 쓴경우는 줄여주지 않는다.
        if(GetMonsterType() == eMonsterTpye.Normal)
        {
            if (m_state != eMoveState.Skill2)
            {
                DecreaseCoolTime(1);
            }
        }
        else if(GetMonsterType() == eMonsterTpye.Boss)
        {
            if(m_state != eMoveState.Skill2)
            {
                DecreaseCoolTime(1);
            }
        }
        SetState(eMoveState.Idle);
        m_attackReady = false;
    }

    void aEvent_Hit()
    {
        if (m_isDie)
            return;

        PlayIdleAnim();
    }

    void aEvent_StopAnimaiton()
    {
        if (m_battleManager.GetSelectPlayer() != null)
        {
            m_animController.Pause();
        }
    }

    void aEvent_ResumeAnimation()
    {
        switch (m_monsterStatus.GetCharacterId())
        {
            case 1001:  // 뮤턴트(크리쳐)
                ((CreatureAnimController)m_animController).Resume();
                break;
            case 1002: // 보스트롤
                ((BossAnimController)m_animController).Resume();
                break;
        }
    }
    #endregion

    #region PlaySfx
    public void aEvent_PlayHItSfx(int id)
    {
        if (SoundManager.Instance == null)
        {
            return;
        }

        switch (id)
        {
            case 1001:  // 크리쳐
                SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Creature_Hit);
                break;
            case 1002: // 트롤킹
                SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.KingTroll_Hit);
                break;
        }
    }

    public void aEvent_PlayDamagedSfx()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Damaged);
        }
    }

    public void aEvent_DieSfx()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Die);
        }
    }

    public void aEvent_SFX_CreatureSkill_01()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Creature_Skill_01);
        }
    }

    public void aEvent_SFX_CreatureSkill_02()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Creature_Skill_02);
        }
    }

    public void aEvent_SFX_KingTrollSkill_01()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.KingTroll_Skill_01);
        }
    }

    public void aEvent_SFX_KingTrollSkill_02()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.KingTroll_Skill_02);
        }
    }
    #endregion
    //void SetCharacterTurnIcon()
    //{
    //    var path = string.Format("Characters/TurnIcon/TurnIcon_{0}", m_monsterStatus.GetCharacterName());
    //    m_turnIcon = Resources.Load(path) as GameObject;
    //}

    void SetDamagedColor()
    {
        if(m_monsterType == eMonsterTpye.Boss)
        {
            if(m_materialProperty != null)
            {
                m_materialProperty.SetColor("_Color", Color.red);
                m_renderer.SetPropertyBlock(m_materialProperty);
            }
        }
        else if(m_monsterType == eMonsterTpye.Normal)
        {
            if (m_materialProperty != null)
            {
                m_materialProperty.SetColor("_Color", Color.red);
                m_renderer.SetPropertyBlock(m_materialProperty);
            }
        }
    }

    void SetDefaultColor()
    {
        if (m_monsterType == eMonsterTpye.Boss)
        {
            if (m_materialProperty != null)
            {
                m_materialProperty.SetColor("_Color", new Color (0.82f, 0.82f, 0.82f, 1f));
                m_renderer.SetPropertyBlock(m_materialProperty);
            }
        }
        else if (m_monsterType == eMonsterTpye.Normal)
        {
            if (m_materialProperty != null)
            {
                m_materialProperty.SetColor("_Color", new Color(0.82f, 0.82f, 0.82f, 1f));
                m_renderer.SetPropertyBlock(m_materialProperty);
            }
        }
    }

    public void InitProcess()
    {
        m_characterController = GetComponent<CharacterController>();
        m_monsterStatus = GetComponent<CharacterStatus>();
        m_animController = GetComponent<AnimationController>();
        m_battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        m_cameraPath = GameObject.Find("Main Camera").GetComponent<CameraPath>();
        m_tweenMove = GetComponent<TweenMove>();
        m_animator = GetComponent<Animator>();

        m_attackReady = false;
        m_initPos = transform.position;
        m_initRotation = transform.localRotation;
        m_materialProperty = new MaterialPropertyBlock();
        m_materialProperty.SetColor("_Color", new Color(0.82f, 0.82f, 0.82f, 1f));
        m_renderer.SetPropertyBlock(m_materialProperty);

        SetState(eMoveState.Idle);

    }

    private void Awake()
    {
        InitProcess();
       // SetCharacterTurnIcon();
    }

    // Start is called before the first frame update
    void Start()
    { 
        if(!gameObject.transform.parent.parent.name.Equals("SpawnPoint_01"))
            gameObject.SetActive(false);
       
        SetSkillCoolTimeInfo();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isDie)
            return;

        if (m_cameraPath.m_fistPathDone)
        {
            m_startTime += Time.deltaTime;

            if (m_startTime > m_waitTime)
            {
                m_isCameraDone = true;
            }
        }
        if (m_isCameraDone)
        {        
            switch (m_state)
            {
                case eMoveState.Idle:
                    if (m_attackReady == false)
                        ActiveGaugeProcess();
                    break;
                case eMoveState.Move:
                    break;
                case eMoveState.Skill1:

                    break;
                case eMoveState.Skill2:

                    break;
                case eMoveState.Skill3:
                 
                    break;
                case eMoveState.Die:
                    break;
            }
        }
    }
}
