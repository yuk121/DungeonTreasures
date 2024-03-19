using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BuffInfo
{
    public string m_buffName;
    public int m_duration;

    public BuffInfo(string buffName, int duration)
    {
        m_buffName = buffName;
        m_duration = duration;
    }
}

public enum ePlayerDamageType
{
    None,
    Normal,
    Critical,
    Miss
}

public class CharacterAttack : MonoBehaviour
{

    public enum eMoveState
    {
        None = -1,
        Idle,
        Move,
        Skill1,
        Skill2,
        Skill3,
        Die
    }

    [SerializeField]
    EffectUnit m_characterTile = null;
    [SerializeField]
    FloatText m_hudText = null;
    [SerializeField]
    eMoveState m_state = eMoveState.None;
    [SerializeField]
    Renderer m_renderer = null;
    Material m_baseMat;

    public TweenMove m_tweenMove;
    protected AnimationController m_animController;

    Animator m_animator;
    CharacterController m_characterController;
    BattleManager m_battleManager;
    CharacterStatus m_charatcterStatus;
    CameraPath m_cameraPath;
    Vector3 m_initPos;
    string m_nowAttackType;
    string m_skillTargetType;
    GameObject m_effect;
    [SerializeField]
    GameObject m_turnIcon= null;
    

    [SerializeField]
    bool m_attackReady = false;
    [SerializeField]
    float m_stopDistance = 0;
    [SerializeField]
    float m_moveSpeedTime = 1f;

    int m_prevSkillIndex = 0;

    float m_prevMyDef = 0;
    float m_damage = 0;
    float m_curActGauge = 0;
    float m_maxActGauge = 5;
    float m_waitTime = 2f;
    float m_startTime = 0f;
    bool m_isCameraDone = false;
    bool m_isDefBuffOn = false;
    public bool m_isDie = false;
    public bool m_isFirstBattle = true;

    string m_prevSkillName;

    List<BuffInfo> m_buffInfoList = new List<BuffInfo>();
    List<int> m_skillCoolTimeList = new List<int>();                                   // 스킬에 관련된 쿨타임만을 저장할 리스트

    public delegate void CharacterSkill();
    public CharacterSkill m_characterSkillDel = null;

    public void SetReady()
    {
        m_attackReady = false;
        m_isCameraDone = false;
        m_startTime = 0;
        SetState(eMoveState.Idle);
    }
    
    public GameObject GetCharacterTurnIcon()
    {
        return m_turnIcon;
    }

    public List<int> GetSkillCoolTimeList()
    {
        List<int> tempList = m_skillCoolTimeList;
        return tempList;
    }

    void SetSkillCoolTimeInfo()
    {
        var skillCnt = m_charatcterStatus.GetSkillCount();
        for (int i = 0; i < skillCnt; i++)
        {
            m_skillCoolTimeList.Add(m_charatcterStatus.GetSkillCoolTime(i));
        }
    }

    public int GetSkillCoomTime(int index)
    {
        return m_skillCoolTimeList[index];
    }

    // 쿨타임 줄여주는 메소드
    public void DecreaseCoolTime(int value)
    {
        // 첫번째는 기본스킬이므로 무조건 계속 쓸 수 있게 쿨타임이 0
        m_skillCoolTimeList[0] = 0;
        // 한턴이 지나면 모든 스킬쿨타임이 줄어든다.
        for (int i = 1; i < m_skillCoolTimeList.Count; i++)
        {
            if (m_skillCoolTimeList[i] == 0)
                continue;

            m_skillCoolTimeList[i] = m_skillCoolTimeList[i] - value;
        }
    }

    #region PlayerPartyBuff

    public void InitCoolTime()
    {
        // 첫번째는 기본스킬이므로 무조건 계속 쓸 수 있게 쿨타임이 0
        m_skillCoolTimeList[0] = 0;

        for (int i = 1; i < m_skillCoolTimeList.Count; i++)
        {
            m_skillCoolTimeList[i] = 0;
        }
    }
    public void PercentHeal(int percent)
    {
        m_charatcterStatus.IncreasePercentHp(percent);
        Debug.Log(string.Format("{0}이 {1}% 만큼 회복", gameObject.name, percent));
    }

    public void SetPowerUp(float percent)
    {
        m_charatcterStatus.IncreasePercentAtk(percent);
        Debug.Log(string.Format("{0}이 {1}% 만큼 공격력 증가", gameObject.name, percent));
    }

    public void SetDefenceUp(float percent)
    {
        m_charatcterStatus.IncreasePercentDef(percent);
        Debug.Log(string.Format("{0}이 {1}% 만큼 방어력 증가", gameObject.name, percent));
    }

    #endregion
    public void InitSkillCoolTime(int skillindex)
    {
        // 스킬을 사용하고나서 쿨타임을 다시 채워준다.
        if (m_skillCoolTimeList[skillindex] <= 0)
             m_skillCoolTimeList[skillindex] = m_charatcterStatus.GetSkillCoolTime(skillindex);
    }
    
    public void SetNoCoolTime()
    {
        for(int i = 0; i < m_skillCoolTimeList.Count; i++)
        {
            m_skillCoolTimeList[i] = 0;
        }
    }


    // 캐릭터 발판 켜주는 함수
    public void OnTile()
    {
        m_characterTile.gameObject.SetActive(true);
        m_characterTile.Play();
    }

    public void OffTile()
    {
        m_characterTile.gameObject.SetActive(false);
    }

    public void ActiveGaugeReset()
    {
        m_curActGauge = 0;
        m_attackReady = false;
        SetState(eMoveState.Idle);
    }

    public List<BuffInfo> GetBuffInfoList()
    {
        return m_buffInfoList;
    }

    public GameObject GetBuffEffect()
    {
        return m_effect;
    }

    public void SetDamagaed(float damage, eMonsterDamageType type)
    {
        if (m_isDie)
            return;

        if (m_hudText != null)
            m_hudText.SetPlayerText(gameObject, type, (int)damage);

        m_charatcterStatus.DecreaseHp((int)damage);

        SetDamagedColor();
        Invoke("SetDefaultColor", 0.2f);

       // Debug.Log(string.Format("{0}의 남은 Hp : {1}", gameObject.name, m_charatcterStatus.GetHp()));

        if (m_charatcterStatus.GetHp() <= 0)
        {
            m_isDie = true;
            SetDie();
            return;
        }

        PlayHitAnim();
    }
    #region CharacterSkill

    public void MoveToTarget(GameObject enemy)
    {
        float dist = Vector3.Distance(transform.position, enemy.transform.position);
        var dir = enemy.transform.position - transform.position;
        int skillIndex = m_battleManager.GetSkillIndex();

        m_skillTargetType = m_charatcterStatus.GetTargetType(skillIndex);               // 현재 사용하는 스킬의 공격범위를 가져온다.
        m_stopDistance = m_charatcterStatus.GetAttackRange(skillIndex);                // 스킬별로 공격거리를 얻어온다.

        m_initPos = transform.parent.position;                                                          // 캐릭터의 원래 위치를 저장한다.

        transform.forward = dir;

        m_tweenMove.m_playAnimation = aEvent_Foward;
        m_animator.speed = 1.5f;

        if (m_skillTargetType == "MultiTarget")
        {
            var point = m_battleManager.GetSpawnPoint();
           
            m_tweenMove.MoveToTween(transform.position, new Vector3(point.position.x, transform.position.y, point.position.z) - new Vector3(dir.x, 0f, dir.z).normalized * m_stopDistance,
                                                            m_moveSpeedTime - (m_moveSpeedTime - (dist / 10f)));
        }
        else if (m_skillTargetType == "SingleTarget" && enemy != null)
        {
            m_tweenMove.MoveToTween(transform.position, new Vector3(enemy.transform.position.x, transform.position.y, enemy.transform.position.z) - new Vector3(dir.x, 0f, dir.z).normalized * m_stopDistance,
                                                            m_moveSpeedTime - (m_moveSpeedTime - (dist / 10f)));

        }
      
        m_tweenMove.m_onFinish = RegisteredSkill;
       
    }

    void RegisteredSkill()
    {
        // 상대방쪽으로 움직인 후 다음에 쓸 스킬
        m_animator.speed = 1f;
        m_characterSkillDel();
    }

    public void LookAtTarget(GameObject enemy)
    {
        int skillIndex = m_battleManager.GetSkillIndex();
        m_skillTargetType = m_charatcterStatus.GetTargetType(skillIndex);

        if (m_skillTargetType == "MultiTarget")
        {
            var point = m_battleManager.GetSpawnPoint();
            transform.LookAt(point);

        }
        else if (m_skillTargetType == "SingleTarget" && enemy != null)
        {
            transform.LookAt(enemy.transform);

        }
    }

    public void BackMyPos()
    {     
        float dist = Vector3.Distance(transform.position, m_initPos);
        m_tweenMove.m_playAnimation = null;

        m_tweenMove.MoveToTween(transform.position, m_initPos, m_moveSpeedTime - (m_moveSpeedTime - (dist / 10f)));
        m_tweenMove.m_onFinish = PlayIdleAnim;
    }

    public void CheckRemoveBuff()
    {
        // 버프체크, 내턴이 끝날때 버프 남은 수 -1
        if (m_buffInfoList != null)
        {         
            for (int i = 0; i < m_buffInfoList.Count; i++)
            {
                m_buffInfoList[i].m_duration -= 1;

                // 버프 리스트의 Duration값을 모두 꺼내서 확인한다., 버프가 끝남
                if (m_buffInfoList[i].m_duration == 0)
                {
                    if(m_isDefBuffOn)
                    {
                        m_charatcterStatus.SetDef(m_prevMyDef);
                        m_isDefBuffOn = false;
                        Debug.Log(string.Format("버프 끝 현재 방어력 : {0}", m_charatcterStatus.GetDef()));
                    }
                    m_buffInfoList.RemoveAt(i);
                    var effect = GetBuffEffect();
                    effect.gameObject.SetActive(false);
                    effect.transform.parent = GameObject.Find("EffectPool").transform;
                    continue;
                }
            }
        }
    }

    // 캐릭이 다양해 질수록 case도 늘어나게 된다.?
    public void PlaySkill_01()
    {
        switch (m_charatcterStatus.GetCharacterId())
        {
            case 1:  //나이트
                ((KnightAnimController)m_animController).Play(KnightAnimController.eAnimState.Skill_01);
                break;
            case 2: // 파이터
                ((FighterAnimController)m_animController).Play(FighterAnimController.eAnimState.Skill_01);
                break;
            case 3: // 아쳐
                ((ArcherAnimController)m_animController).Play(ArcherAnimController.eAnimState.Skill_01, false);
                break;
            case 4: // 매지션
                ((MagicianAnimController)m_animController).Play(MagicianAnimController.eAnimState.Skill_01, false);
                break;
        }
    }

    public void PlaySkill_02()
    {
        switch (m_charatcterStatus.GetCharacterId())
        {
            case 1:  //나이트
                ((KnightAnimController)m_animController).Play(KnightAnimController.eAnimState.Skill_02);
                break;
            case 2: // 파이터
                ((FighterAnimController)m_animController).Play(FighterAnimController.eAnimState.Skill_02);
                break;
            case 3: // 아쳐
                ((ArcherAnimController)m_animController).Play(ArcherAnimController.eAnimState.Skill_02);
                break;
            case 4: // 매지션
                ((MagicianAnimController)m_animController).Play(MagicianAnimController.eAnimState.Skill_02,false);
                break;
        }
    }

    public void PlaySkill_03()
    {
        switch (m_charatcterStatus.GetCharacterId())
        {
            case 1:  //나이트
                ((KnightAnimController)m_animController).Play(KnightAnimController.eAnimState.Skill_03);
                break;
            case 2: // 파이터
                ((FighterAnimController)m_animController).Play(FighterAnimController.eAnimState.Skill_03);
                break;
            case 3: // 아쳐
                ((ArcherAnimController)m_animController).Play(ArcherAnimController.eAnimState.Skill_03,false);
                break;
            case 4: // 매지션
                ((MagicianAnimController)m_animController).Play(MagicianAnimController.eAnimState.Skill_03,false);
                break;
        }
    }

    //void PlayReadyAnim()
    //{
    //    switch (m_charatcterStatus.GetCharacterId())
    //    {
    //        case 1:  //나이트
    //            ((KnightAnimController)m_animController).Play(KnightAnimController.eAnimState.Equip, false);
    //            break;
    //        case 2: // 파이터
    //            ((FighterAnimController)m_animController).Play(FighterAnimController.eAnimState.Equip, false);
    //            break;
    //        case 3: // 아쳐
    //            ((ArcherAnimController)m_animController).Play(ArcherAnimController.eAnimState.Equip, false);
    //            break;
    //        case 4: // 매지션
    //            //((MagicianAnimController)m_animController).Play(MagicianAnimController.eAnimSate, false);
    //            break;
    //    }
    //}

    void PlayIdleAnim()
    {
        CheckRemoveBuff();

        switch (m_charatcterStatus.GetCharacterId())
        {
            case 1:  //나이트
                ((KnightAnimController)m_animController).Play(KnightAnimController.eAnimState.Idle);
                break;
            case 2: // 파이터
                ((FighterAnimController)m_animController).Play(FighterAnimController.eAnimState.Idle);
                break;
            case 3: // 아쳐
                ((ArcherAnimController)m_animController).Play(ArcherAnimController.eAnimState.Idle);
                break;
            case 4: // 매지션
                ((MagicianAnimController)m_animController).Play(MagicianAnimController.eAnimState.Idle);
                break;
        }

        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.zero;
        SetState(eMoveState.Idle);
    }

    void aEvent_Foward()
    {
        switch (m_charatcterStatus.GetCharacterId())
        {
            case 1:  //나이트
                if (((KnightAnimController)m_animController).GetAniState() != KnightAnimController.eAnimState.Foward)
                    ((KnightAnimController)m_animController).Play(KnightAnimController.eAnimState.Foward);
                break;
            case 2: // 파이터
                if (((FighterAnimController)m_animController).GetAniState() != FighterAnimController.eAnimState.Foward)
                    ((FighterAnimController)m_animController).Play(FighterAnimController.eAnimState.Foward);
                break;
            case 3: // 아쳐
                if (((ArcherAnimController)m_animController).GetAniState() != ArcherAnimController.eAnimState.Foward)
                    ((ArcherAnimController)m_animController).Play(ArcherAnimController.eAnimState.Foward);
                break;
            case 4: // 매지션
                if (((MagicianAnimController)m_animController).GetAniState() != MagicianAnimController.eAnimState.Foward)
                    ((MagicianAnimController)m_animController).Play(MagicianAnimController.eAnimState.Foward);
                break;
        }
    }


    void PlayHitAnim()
    {
        var id = m_charatcterStatus.GetCharacterId();
        switch (id)
        {
            case 1:  //나이트
                ((KnightAnimController)m_animController).Play(KnightAnimController.eAnimState.Hit, false);
                aEvent_PlayDamagedSfx();
                aEvent_PlayHItSfx(1);
                break;
            case 2: // 파이터
                ((FighterAnimController)m_animController).Play(FighterAnimController.eAnimState.Hit, false);
                aEvent_PlayDamagedSfx();
                aEvent_PlayHItSfx(2);
                break;
            case 3: // 아쳐
                ((ArcherAnimController)m_animController).Play(ArcherAnimController.eAnimState.Hit, false);
                aEvent_PlayDamagedSfx();
                aEvent_PlayHItSfx(3);
                break;
            case 4: // 매지션
                ((MagicianAnimController)m_animController).Play(MagicianAnimController.eAnimState.Hit, false);
                aEvent_PlayDamagedSfx();
                aEvent_PlayHItSfx(4);
                break;
        }
    }

    void PlayDieAnim()
    {
        var id = m_charatcterStatus.GetCharacterId();
        switch (id)
        {
            case 1:  //나이트
                ((KnightAnimController)m_animController).Play(KnightAnimController.eAnimState.Die, false);
                break;
            case 2: // 파이터
                ((FighterAnimController)m_animController).Play(FighterAnimController.eAnimState.Die, false);
                break;
            case 3: // 아쳐
                ((ArcherAnimController)m_animController).Play(ArcherAnimController.eAnimState.Die, false);
                break;
            case 4: // 매지션
                ((MagicianAnimController)m_animController).Play(MagicianAnimController.eAnimState.Die, false);
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
            case 1:  //나이트
                SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Knight_Hit);
                break;
            case 2: // 파이터
                SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Fighter_Hit);
                break;
            case 3: // 아쳐
                SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Arhcer_Hit);
                break;
            case 4: // 매지션
                SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Magician_Hit);
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

    public void aEvent_PlayDieSfx()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Die);
        }
    }

    public void aEvent_SFX_KnightSkill_01()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Knight_Skill_01);
        }
    }

    public void aEvent_SFX_KnightSkill_02()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Knight_Skill_02);
        }
    }

    public void aEvent_SFX_KnightSkill_03_1()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Knight_Skill_03_1);
        }
    }

    public void aEvent_SFX_KnightSkill_03_2()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Knight_Skill_03_2);
        }
    }

    public void aEvent_SFX_FighterSkill_01()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Fighter_Skill_01);
        }
    }

    public void aEvent_SFX_FighterSkill_02()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Fighter_Skill_02);
        }
    }

    public void aEvent_SFX_FighterSkill_03()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Fighter_Skill_03);
        }
    }

    public void aEvent_SFX_ArcherSkill_01()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Arhcer_Skill_01);
        }
    }

    public void aEvent_SFX_ArcherSkill_02()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Arhcer_Skill_02);
        }
    }

    public void aEvent_SFX_ArcherSkill_03()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Arhcer_Skill_03);
        }
    }

    public void aEvent_SFX_MagicianSkill_01()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Magician_Skill_01);
        }
    }

    public void aEvent_SFX_MagicianSkill_02()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Magician_Skill_02);
        }
    }

    public void aEvent_SFX_MagicianSkill_03()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlaySfx(SoundManager.eEffectSoundAudioClip.Magician_Skill_03);
        }
    }
    #endregion

    IEnumerator DelayBackMoveTime()
    {
        yield return new WaitForSeconds(.5f);
        BackMyPos();
    }

    IEnumerator WaitSkillTime()
    {
        yield return new WaitForSeconds(1f);
        aEvent_Attack();
    }

    public ePlayerDamageType AttackEnemy(GameObject monster)
    {
        MonsterController enemy = monster.GetComponent<MonsterController>();
        ePlayerDamageType damageType = ePlayerDamageType.None;
        int skillIndex = m_battleManager.GetSkillIndex();
        m_damage = 0f;
        var enemyStat = enemy.GetComponent<CharacterStatus>();

        if (Calculation.AttackDecision(m_charatcterStatus.GetHitRate() + m_charatcterStatus.GetSkillHitRate(skillIndex) > m_charatcterStatus.GetHitRate() ? 100f : m_charatcterStatus.GetHitRate(), enemyStat.GetDodgeRate()))
        {
            m_damage = Calculation.NormalDamage(m_charatcterStatus.GetAtk(), m_charatcterStatus.GetSkillValue(skillIndex), enemyStat.GetDef());
            damageType = ePlayerDamageType.Normal;
            
            if (Calculation.CriticalDecision(m_charatcterStatus.GetCriRate()))
            {
                m_damage = Calculation.CriticalDamage(m_damage, 50);
                damageType = ePlayerDamageType.Critical;
            }

            // 데미지 최소 ,최대 대미지
            m_damage += Random.Range(0, 30);        
        }
        else
        {
            damageType = ePlayerDamageType.Miss;
        }

        if (m_damage <= 0)
        {
            m_damage = 0;
            damageType = ePlayerDamageType.Miss;
        }

        return damageType;
    }

    #region AnimationController
    public void aEvent_Attack()
    {
        ePlayerDamageType type = ePlayerDamageType.None;
        int skillIndex = m_battleManager.GetSkillIndex();
        m_nowAttackType = m_charatcterStatus.GetAttackType(skillIndex);
        m_skillTargetType = m_charatcterStatus.GetTargetType(skillIndex);

        // 스킬 타입별로 몬스터 전체에게 이펙트를 넣을지 말지 선택하는 구간
        if (m_skillTargetType == "SingleTarget")
        {
            var mon = m_battleManager.GetSelectMonster();
            var pos = Util.FindChildObject(mon, "Dummy_Hit").transform.position;

            if (mon != null && pos != null)
            {
                var effectId = m_charatcterStatus.GetEffectId(m_battleManager.GetSkillIndex());
                var effectData = TableEffect.Instance.GetData(effectId);
                var effect = EffectPool.Instance.Create(effectData.Prefab[0]);

                effect.transform.position = pos;
                effect.transform.LookAt(transform.position);

                type = AttackEnemy(mon);
                mon.GetComponent<MonsterController>().SetDamaged(m_damage, type);
            }
        }
        else if (m_skillTargetType == "MultiTarget")
        {
            List<GameObject> monsters = m_battleManager.GetMonsters();
            Vector3[] pos = new Vector3[monsters.Count];

            for (int i = 0; i < monsters.Count; i++)
            {
                pos[i] = Util.FindChildObject(monsters[i], "Dummy_Hit").transform.position;
            }

            if (monsters != null && pos != null)
            {
                for (int i = 0; i < monsters.Count; i++) 
                {
                    var effectId = m_charatcterStatus.GetEffectId(m_battleManager.GetSkillIndex());
                    var effectData = TableEffect.Instance.GetData(effectId);
                    var effect = EffectPool.Instance.Create(effectData.Prefab[0]);
                    var mon = monsters[i].transform.GetChild(0);

                    effect.transform.position = pos[i];
                    effect.transform.rotation = Quaternion.Euler(monsters[i].transform.rotation.x + 140, 0f, 0f);

                    type = AttackEnemy(mon.gameObject);
                    mon.GetComponent<MonsterController>().SetDamaged(m_damage, type);
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
                PlayIdleAnim();
                break;
        }

        m_prevSkillIndex = skillIndex;

        m_attackReady = false;
    }

    void aEvent_Shooting()
    {
        int skillIndex = m_battleManager.GetSkillIndex();
        m_skillTargetType = m_charatcterStatus.GetTargetType(skillIndex);

        var dummy = Util.FindChildObject(gameObject, "Dummy_Arrow");

        if (m_skillTargetType == "SingleTarget")
        {
            var arrow = EffectPool.Instance.Create(TableEffect.Instance.GetData("19").Prefab[0]);

            arrow.transform.SetParent(dummy.transform);
            arrow.transform.localPosition = Vector3.zero;
            arrow.transform.localRotation = Quaternion.identity;
            arrow.GetComponent<Arrow>().SetArrow(dummy.transform.forward, this);
            arrow.transform.SetParent(EffectPool.Instance.transform);
        }
        else if (m_skillTargetType == "MultiTarget")
        {
            List<GameObject> monsters = m_battleManager.GetMonsters();
            var point = m_battleManager.GetSpawnPoint();

            if (monsters != null)
            {
                var arrow = EffectPool.Instance.Create(TableEffect.Instance.GetData("20").Prefab[0]);

                arrow.transform.SetParent(dummy.transform);
                arrow.transform.localPosition = Vector3.zero;
                arrow.transform.localRotation = Quaternion.identity;
                arrow.GetComponent<Arrow>().GetTargetDir(point.gameObject);
                arrow.GetComponent<Arrow>().SetArrow(dummy.transform.forward, this);
                arrow.transform.SetParent(EffectPool.Instance.transform);

                var arrowRain = EffectPool.Instance.Create(TableEffect.Instance.GetData("21").Prefab[0]);
                arrowRain.transform.position = point.transform.position;
                arrowRain.transform.rotation = Quaternion.Euler(-90, 0, 0);
                if(arrowRain != null)
                {
                    aEvent_SFX_ArcherSkill_03();
                }

                StartCoroutine("WaitSkillTime");
            }
        }
    }

    void aEvent_MagicAttack()
    {
        int skillIndex = m_battleManager.GetSkillIndex();
        m_skillTargetType = m_charatcterStatus.GetTargetType(skillIndex);
        var enemy = m_battleManager.GetSelectMonster();
        var skillID = m_charatcterStatus.GetSkillId(skillIndex);
        GameObject magic = null;
        Transform point = null;
        Transform target = null;

        if (m_skillTargetType == "MultiTarget")
        {
            point = m_battleManager.GetSpawnPoint();
        }
        else if (m_skillTargetType == "SingleTarget")
        {
            target = m_battleManager.GetSelectMonster().transform;
        }

        switch (skillID)
        {
            case 10: // 마력탄
                magic = EffectPool.Instance.Create(TableEffect.Instance.GetData("22").Prefab[0]);
                var dummy = Util.FindChildObject(gameObject, "Dummy_FirePos");

                magic.transform.SetParent(dummy.transform);
                magic.transform.localPosition = Vector3.zero;
                magic.transform.localRotation = Quaternion.identity;
                magic.GetComponent<Arrow>().SetArrow(dummy.transform.forward, this);
                magic.transform.SetParent(EffectPool.Instance.transform);

                aEvent_SFX_MagicianSkill_01();
                break;
            case 11: // 서릿발
                magic = EffectPool.Instance.Create(TableEffect.Instance.GetData("23").Prefab[0]);

                magic.transform.position = target.position;
                magic.transform.rotation = Quaternion.Euler(-90, 0, 0);
                StartCoroutine("WaitSkillTime");

                aEvent_SFX_MagicianSkill_02();
                break;
            case 12: // 낙뢰
                magic = EffectPool.Instance.Create(TableEffect.Instance.GetData("24").Prefab[0]);

                magic.transform.position = point.transform.position;
                magic.transform.rotation = Quaternion.identity;
                StartCoroutine("WaitSkillTime");

                aEvent_SFX_MagicianSkill_03();
                break;
        }
    }


    void aEvent_StopAnimaiton()
    {
        if (m_battleManager.GetSelectMonster() != null)
        {
            m_animController.Pause();
        }
    }

    void aEvent_Buff()
    {
        int skillIndex = m_battleManager.GetSkillIndex();
        m_skillTargetType = m_charatcterStatus.GetTargetType(skillIndex);
        int buffDuration = m_charatcterStatus.GetSkillDuration(skillIndex);
        string skillName = m_charatcterStatus.GetSkillName(skillIndex);

        // 버프 이펙트 넣는 구간
        if (m_skillTargetType == "SingleTarget")
        {
            Transform transform = Util.FindChildObject(gameObject, "Dummy_Tile").transform;
            var pos = transform.position;

            if (pos != null)
            {
                var effectId = m_charatcterStatus.GetEffectId(m_battleManager.GetSkillIndex());
                var effectData = TableEffect.Instance.GetData(effectId);
                m_effect = EffectPool.Instance.Create(effectData.Prefab[1]);

                m_effect.transform.parent = transform;
                m_effect.transform.position = pos;
            }
        }
        else if (m_skillTargetType == "MultiTarget")
        {
            //미구현
        }

        m_prevSkillName = skillName;
        m_prevSkillIndex = skillIndex;

        // 이전에 들어온 버프와 현재 쓴 버프가 같은지 체크해서 같다면 duration값을 초기화 아니면 버프 추가
        if (m_buffInfoList.Exists(temp => temp.m_buffName.Equals(m_prevSkillName)))
        {
            BuffInfo mybuff = m_buffInfoList.Find(temp => temp.m_buffName == string.Format("{0}", skillName));
            mybuff.m_duration = buffDuration;
        }
        else
            m_buffInfoList.Add(new BuffInfo(skillName, buffDuration));


        // 버프 효과 넣어줌

        if (skillName == "쉴드 업")
        {
            m_prevMyDef = m_charatcterStatus.GetDef();               // 버프 받기 이전 수치를 기억

            m_charatcterStatus.IncreaseDef(m_charatcterStatus.GetSkillValue(skillIndex));
            Debug.Log(string.Format("버프 후 현재 방어력 : {0}", m_charatcterStatus.GetDef()));
            m_isDefBuffOn = true;
        }

        m_attackReady = false;
    }

    void aEvent_ResumeAnimation()
    {
        switch (m_charatcterStatus.GetCharacterId())
        {
            case 1:  //나이트
                ((KnightAnimController)m_animController).Resume();
                break;
            case 2: // 파이터
                ((FighterAnimController)m_animController).Resume();
                break;
            case 3: // 아쳐
                ((ArcherAnimController)m_animController).Resume();
                break;
        }
    }

    void aEvent_Hit()
    {
        if (m_isDie)
            return;

        PlayIdleAnim();
    }

    void aEvent_RemovePlayer()
    {
        transform.parent.gameObject.SetActive(false);
    }

    #endregion
    public void SetState(eMoveState state)
    {
        m_state = state;
    }

    public eMoveState GetState()
    {
        return m_state;
    }

    void SetDie()
    {
        aEvent_PlayDamagedSfx();
        m_battleManager.RemoveTurnIcon(gameObject);
        switch (m_charatcterStatus.GetCharacterId())
        {
            case 1:
                aEvent_PlayHItSfx(1);
                break;
            case 2:
                aEvent_PlayHItSfx(2);
                break;
            case 3:
                aEvent_PlayHItSfx(3);
                break;
            case 4:
                aEvent_PlayHItSfx(4);
                break;
        }

        m_charatcterStatus.SetHp(0);
        m_battleManager.RemoveinList(gameObject);
        m_battleManager.RemoveTurnIcon(gameObject);

        if (m_characterTile == enabled)
        {
            OffTile();
        }

        PlayDieAnim();        
    }

    void ActiveGaugeProcess()
    {      
        int speed = m_charatcterStatus.GetCharacterSpeed();

        m_curActGauge = m_curActGauge + (Time.deltaTime * (speed / m_maxActGauge));

        if (m_curActGauge >= m_maxActGauge)
        {
            m_curActGauge = 0;
            m_attackReady = true;
            SetState(eMoveState.Move);
            m_battleManager.GetAttackReady(m_attackReady, gameObject);
        }
    }

    // 캐릭터 셰이더를 통해 피격시 잠깐 빨갛게 해주는 메소드
    void SetDamagedColor()
    {
        if(m_baseMat != null)
        {
            m_baseMat.SetColor("_Color", Color.red);
        }
    }

    void SetDefaultColor()
    {
        if (m_baseMat != null)
        {
            m_baseMat.SetColor("_Color", new Color(0.82f, 0.82f, 0.82f, 1f));
        }
    }

    //// 턴 아이콘을 갖게해주는 메소드
    //void SetCharacterTurnIcon()
    //{
    //    var path = string.Format("Image/Characters/TurnIcon/TurnIcon_{0}", m_charatcterStatus.GetCharacterName());
    //    m_turnIcon = Resources.Load(path) as GameObject;
    //}

    private void Awake()
    {
        m_charatcterStatus = GetComponent<CharacterStatus>();
        m_animator = GetComponent<Animator>();
        m_characterController = GetComponent<CharacterController>();
        m_tweenMove = GetComponent<TweenMove>();
        m_animController = GetComponent<AnimationController>();
        m_cameraPath = GameObject.Find("Main Camera").GetComponent<CameraPath>();

        if (GameObject.Find("BattleManager"))
        {
            m_battleManager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
        }

        m_baseMat = m_renderer.material;
    }

    // Start is called before the first frame update
    void Start()
    { 
        SetSkillCoolTimeInfo();
        m_attackReady = false;

        Debug.Log(string.Format("#로그 캐릭터 : {0} , ID : {1}, Hp : {2} , Atk : {3} , Def : {4} , Speed : {5} , CriRate : {6} , HitRate : {7} , DodgeRate : {8} , 타입 : {9}\n"
                                                                  , m_charatcterStatus.GetCharacterName(), m_charatcterStatus.GetCharacterId(), m_charatcterStatus.GetHp(), m_charatcterStatus.GetAtk(), 
                                                                  m_charatcterStatus.GetDef(), m_charatcterStatus.GetCharacterSpeed(), m_charatcterStatus.GetCriRate(), 
                                                                  m_charatcterStatus.GetHitRate(), m_charatcterStatus.GetDodgeRate(), m_charatcterStatus.GetType()));
        SetState(eMoveState.Idle);
    }

    // Update is called once per frame
    void Update()
    {
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
            if (m_isDie)
                return;
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

