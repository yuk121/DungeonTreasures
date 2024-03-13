using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : DontDestory<SoundManager>
{
    enum eBackgroundSoundAudioClips
    {
        BackgroundSound_01 = 0,
        BackgroundSound_02,
        BackgroundSound_03,
        BackgroundSound_04,
        BackgroundSound_05,
        Max
    }

    public enum eEffectSoundAudioClip
    {
        Knight_Hit = 0,
        Knight_Skill_01,
        Knight_Skill_02,
        Knight_Skill_03_1,
        Knight_Skill_03_2,
        Fighter_Hit,
        Fighter_Skill_01,
        Fighter_Skill_02,
        Fighter_Skill_03,
        Arhcer_Hit,
        Arhcer_Skill_01,
        Arhcer_Skill_02,
        Arhcer_Skill_03,
        Magician_Hit,
        Magician_Skill_01,
        Magician_Skill_02,
        Magician_Skill_03,
        Creature_Hit,
        Creature_Skill_01,
        Creature_Skill_02,
        KingTroll_Hit,
        KingTroll_Skill_01,
        KingTroll_Skill_02,
        Damaged,
        Die,
        Walk,
        Walk2,
        Walk3,
        Max
    }

    public  enum eUISoundAudioClip
    {
        BuffWindow =0,
        StageWindow,
        Click,
        Defeat,
        Victory,
        Button,
        EnterDungeon,
        Transport,
        Max
    }

    [SerializeField]
    AudioSource m_audioBGMPlayers = null;
    [SerializeField]
    AudioSource m_audioSFXPlayers = null;


    AudioClip[] m_audioBGMClips;
    AudioClip[] m_audioSFXClips;
    AudioClip[] m_audioUIClips;

    void LoadSound()
    {
        m_audioBGMClips = new AudioClip[(int)eBackgroundSoundAudioClips.Max];
        m_audioSFXClips = new AudioClip[(int)eEffectSoundAudioClip.Max];
        m_audioUIClips = new AudioClip[(int)eUISoundAudioClip.Max];

        for (int i = 0; i < (int)eBackgroundSoundAudioClips.Max; i++)
        {
            var path = string.Format("Sound/BackgroundSound_{0:00}",i + 1);
            var clip = Resources.Load(path) as AudioClip;
            m_audioBGMClips[i] = clip;
        }

        for(int j= 0; j <(int)eEffectSoundAudioClip.Max; j++)
        {
            var path = string.Format("Sound/{0}",(eEffectSoundAudioClip)j);
            var clip = Resources.Load(path) as AudioClip;
            m_audioSFXClips[j] = clip;
        }

        for (int k= 0; k < (int)eUISoundAudioClip.Max; k++)
        {
            var path = string.Format("Sound/{0}", (eUISoundAudioClip)k);
            var clip = Resources.Load(path) as AudioClip;
            m_audioUIClips[k] = clip;
        }
        
    }
    
    public AudioSource GetBGMAudio()
    {
        return m_audioBGMPlayers;
    }

    public AudioSource GetSFXAudio()
    {
        return m_audioSFXPlayers;
    }
    
    #region PlayBackgroundMusic
    public void PlayBGM(LoadSceneManager.eScene scene)
    {
        switch(scene)
        {        
            case LoadSceneManager.eScene.Title:
                if (m_audioBGMPlayers.clip != null || m_audioBGMPlayers.isPlaying == true)
                    m_audioBGMPlayers.Stop();

                m_audioBGMPlayers = GetComponent<AudioSource>();
                m_audioBGMPlayers.clip = m_audioBGMClips[0];
                m_audioBGMPlayers.loop = true;
                m_audioBGMPlayers.rolloffMode = AudioRolloffMode.Linear;

                m_audioBGMPlayers.Play();
                break;

            case LoadSceneManager.eScene.Lobby:
                if (m_audioBGMPlayers.clip != null || m_audioBGMPlayers.isPlaying == true)
                    m_audioBGMPlayers.Stop();

                m_audioBGMPlayers = GetComponent<AudioSource>();
                m_audioBGMPlayers.clip = m_audioBGMClips[1];
                m_audioBGMPlayers.loop = true;
                m_audioBGMPlayers.rolloffMode = AudioRolloffMode.Linear;

                if (m_audioBGMPlayers.clip != null || m_audioBGMPlayers.isPlaying == true)
                    m_audioBGMPlayers.Stop();

                m_audioBGMPlayers.Play();
                break;

            case LoadSceneManager.eScene.Stage:
                if (m_audioBGMPlayers.clip != null || m_audioBGMPlayers.isPlaying == true)
                    m_audioBGMPlayers.Stop();

                m_audioBGMPlayers = GetComponent<AudioSource>();
                m_audioBGMPlayers.clip = m_audioBGMClips[2];
                m_audioBGMPlayers.loop = true;
                m_audioBGMPlayers.rolloffMode = AudioRolloffMode.Linear;

                if (m_audioBGMPlayers.clip != null || m_audioBGMPlayers.isPlaying == true)
                    m_audioBGMPlayers.Stop();

                m_audioBGMPlayers.Play();
                break;

            case LoadSceneManager.eScene.Game_Castle_01:
                if (m_audioBGMPlayers.clip != null || m_audioBGMPlayers.isPlaying == true)
                    m_audioBGMPlayers.Stop();

                m_audioBGMPlayers = GetComponent<AudioSource>();
                m_audioBGMPlayers.clip = m_audioBGMClips[3];
                m_audioBGMPlayers.loop = true;
                m_audioBGMPlayers.rolloffMode = AudioRolloffMode.Linear;

                if (m_audioBGMPlayers.clip != null || m_audioBGMPlayers.isPlaying == true)
                    m_audioBGMPlayers.Stop();

                m_audioBGMPlayers.Play();
                break;
        }
    }

    public void PlayBossBGM()
    {
        if (m_audioBGMPlayers.clip != null || m_audioBGMPlayers.isPlaying == true)
            m_audioBGMPlayers.Stop();

        m_audioBGMPlayers = GetComponent<AudioSource>();
        m_audioBGMPlayers.clip = m_audioBGMClips[4];
        m_audioBGMPlayers.loop = true;
        //m_audioBGMPlayers.volume = 0.3f;
        m_audioBGMPlayers.rolloffMode = AudioRolloffMode.Linear;

        if (m_audioBGMPlayers.clip != null || m_audioBGMPlayers.isPlaying == true)
            m_audioBGMPlayers.Stop();

        m_audioBGMPlayers.Play();
    }

    public void StopBGM()
    {
        if (m_audioBGMPlayers.clip != null || m_audioBGMPlayers.isPlaying == true)
            m_audioBGMPlayers.Stop();
    }

    public void MuteBGM()
    {
        if (m_audioBGMPlayers.clip != null || m_audioBGMPlayers.isPlaying == true)
        {
            m_audioBGMPlayers.volume = 0f;
        }
    }

    public void UnmuteBGM()
    {
        if (m_audioBGMPlayers.clip != null || m_audioBGMPlayers.isPlaying == false)
        {
            m_audioBGMPlayers.volume = 1f;
        }
    }

    #endregion

    #region PlaySFX

    public void PlaySfx(eEffectSoundAudioClip effect)
    {
        m_audioSFXPlayers.volume = 1f;
        switch(effect)
        {
           
            case eEffectSoundAudioClip.Knight_Hit:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Knight_Hit]);
                break;
            case eEffectSoundAudioClip.Knight_Skill_01:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Knight_Skill_01]);
                break;
            case eEffectSoundAudioClip.Knight_Skill_02:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Knight_Skill_02]);
                break;
            case eEffectSoundAudioClip.Knight_Skill_03_1:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Knight_Skill_03_1]);
                break;
            case eEffectSoundAudioClip.Knight_Skill_03_2:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Knight_Skill_03_2]);
                break;

            case eEffectSoundAudioClip.Fighter_Hit:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Fighter_Hit]);
                break;
            case eEffectSoundAudioClip.Fighter_Skill_01:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Fighter_Skill_01]);
                break;
            case eEffectSoundAudioClip.Fighter_Skill_02:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Fighter_Skill_02]);
                break;
            case eEffectSoundAudioClip.Fighter_Skill_03:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Fighter_Skill_03]);
                break;

            case eEffectSoundAudioClip.Arhcer_Hit:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Arhcer_Hit]);
                break;
            case eEffectSoundAudioClip.Arhcer_Skill_01:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Arhcer_Skill_01]);
                break;
            case eEffectSoundAudioClip.Arhcer_Skill_02:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Arhcer_Skill_02]);
                break;
            case eEffectSoundAudioClip.Arhcer_Skill_03:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Arhcer_Skill_03]);
                break;

            case eEffectSoundAudioClip.Magician_Hit:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Magician_Hit]);
                break;
            case eEffectSoundAudioClip.Magician_Skill_01:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Magician_Skill_01]);
                break;
            case eEffectSoundAudioClip.Magician_Skill_02:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Magician_Skill_02]);
                break;
            case eEffectSoundAudioClip.Magician_Skill_03:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Magician_Skill_03]);
                break;

            case eEffectSoundAudioClip.Creature_Hit:              
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Creature_Hit]);
                break;
            case eEffectSoundAudioClip.Creature_Skill_01:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Creature_Skill_01]);
                break;
            case eEffectSoundAudioClip.Creature_Skill_02:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Creature_Skill_02]);
                break;         

            case eEffectSoundAudioClip.KingTroll_Hit:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.KingTroll_Hit]);
                break;
            case eEffectSoundAudioClip.KingTroll_Skill_01:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.KingTroll_Skill_01]);
                break;
            case eEffectSoundAudioClip.KingTroll_Skill_02:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.KingTroll_Skill_02]);
                break;

            case eEffectSoundAudioClip.Damaged:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Damaged]);
                break;
            case eEffectSoundAudioClip.Die:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Die]);
                break;
            case eEffectSoundAudioClip.Walk:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Walk]);
                break;
            case eEffectSoundAudioClip.Walk2:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Walk2]);
                break;
            case eEffectSoundAudioClip.Walk3:
                m_audioSFXPlayers.PlayOneShot(m_audioSFXClips[(int)eEffectSoundAudioClip.Walk3]);
                break;
        }
    }

    public void StopSFX()
    {
        if (m_audioSFXPlayers.clip != null || m_audioSFXPlayers.isPlaying == true)
            m_audioSFXPlayers.Stop();
    }

    public void MuteSFX()
    {
        if (m_audioSFXPlayers.clip != null || m_audioSFXPlayers.isPlaying == true)
        {
            m_audioSFXPlayers.volume = 0f;
        }
    }

    public void UnmuteSFX()
    {
        if (m_audioSFXPlayers.clip != null || m_audioSFXPlayers.isPlaying == false)
        {
            m_audioSFXPlayers.volume = 1f;
        }
    }

    #endregion

    #region PlayUISound
    public void PlayUISound(eUISoundAudioClip ui)
    {
        switch(ui)
        {
            case eUISoundAudioClip.BuffWindow:
                //m_audioSFXPlayers.volume = 0.3f;
                m_audioSFXPlayers.PlayOneShot(m_audioUIClips[0]);
                break;
            case eUISoundAudioClip.StageWindow:
               // m_audioSFXPlayers.volume = 0.3f;
                m_audioSFXPlayers.PlayOneShot(m_audioUIClips[1]);
                break;
            case eUISoundAudioClip.Click:
              //  m_audioSFXPlayers.volume = 0.3f;
                m_audioSFXPlayers.PlayOneShot(m_audioUIClips[2]);
                break;
            case eUISoundAudioClip.Defeat:
              //  m_audioSFXPlayers.volume = 0.3f;
                m_audioSFXPlayers.PlayOneShot(m_audioUIClips[3]);
                break;
            case eUISoundAudioClip.Victory:
              //  m_audioSFXPlayers.volume = 0.3f;
                m_audioSFXPlayers.PlayOneShot(m_audioUIClips[4]);
                break;
            case eUISoundAudioClip.Button:
                //m_audioSFXPlayers.volume = 0.3f;
                m_audioSFXPlayers.PlayOneShot(m_audioUIClips[5]);
                break;
            case eUISoundAudioClip.EnterDungeon:
                //m_audioSFXPlayers.volume = 1f;
                m_audioSFXPlayers.PlayOneShot(m_audioUIClips[6]);
                break;
            case eUISoundAudioClip.Transport:
               // m_audioSFXPlayers.volume = 1f;
                m_audioSFXPlayers.PlayOneShot(m_audioUIClips[7]);
                break;
        }
    }

    public void StopUISound()
    {
        if (m_audioSFXPlayers.clip != null || m_audioSFXPlayers.isPlaying == true)
            m_audioSFXPlayers.Stop();
    }

    public void SetSoundVolume()
    {
        m_audioBGMPlayers.volume = PlayerDataManager.Instance.GetBGMVolume();
        m_audioSFXPlayers.volume = PlayerDataManager.Instance.GetSFXVolume();
    }

    public void StartDefaultSound()
    {
        m_audioBGMPlayers.volume = 1f;
        m_audioSFXPlayers.volume = 1f;
    }


    #endregion
    protected override void OnAwake()
    {
        LoadSound();
    }

    protected override void OnStart()
    {
       StartDefaultSound();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            PlayUISound(eUISoundAudioClip.Click);
        }
    }
}
