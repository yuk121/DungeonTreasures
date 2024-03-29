using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField]
    GameObject m_bgmOptionPanel = null;
    [SerializeField]
    GameObject m_bgmOptionImage = null;
    [SerializeField]
    GameObject m_sfxOptionPanel = null;
    [SerializeField]
    GameObject m_sfxOptionImage = null;

    AudioSource m_bgmAduio;
    AudioSource m_sfxAudio;

    Slider m_bgmSlider;
    Slider m_sfxSlider;

    void BGMSlider()
    {
        m_bgmAduio.volume = m_bgmSlider.value;

        if (m_bgmSlider.value <= 0)
        {
            m_bgmOptionImage.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            m_bgmOptionImage.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (PlayerDataManager.Instance != null)
        {
            int volume = (int)(m_bgmAduio.volume * 100);
            PlayerDataManager.Instance.SetBGMVolume(volume);
            PlayerDataManager.Instance.SavePlayerData();
        }

        //if (PlayerDataManager.Instance != null)
        //{
        //    int volume = (int)(m_bgmAduio.volume * 100);
        //    PlayerDataManager.Instance.SetBGMVolume(volume);
        //    //FirebaseManager.Instance.SaveUserInfo(FirebaseManager.Instance.GetCurrentUserUID());
        //}
    }

    void SFXSlider()
    {
        m_sfxAudio.volume = m_sfxSlider.value;

        if (m_sfxSlider.value <= 0)
        {
            m_sfxOptionImage.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            m_sfxOptionImage.transform.GetChild(0).gameObject.SetActive(false);
        }

        // 상태 저장
        if (PlayerDataManager.Instance != null)
        {
            int volume = (int)(m_sfxAudio.volume * 100);
            PlayerDataManager.Instance.SetSFXVolume(volume);
            PlayerDataManager.Instance.SavePlayerData();
        }

        //if (PlayerDataManager.Instance != null)
        //{
        //    int volume = (int)(m_sfxAudio.volume * 100);
        //    PlayerDataManager.Instance.SetSFXVolume(volume);
        //    //FirebaseManager.Instance.SaveUserInfo(FirebaseManager.Instance.GetCurrentUserUID());
        //}

    }

    private void Awake()
    {
        m_bgmSlider = m_bgmOptionPanel.GetComponentInChildren<Slider>();
        m_bgmOptionImage.transform.GetChild(0).gameObject.SetActive(false);

        m_sfxSlider = m_sfxOptionPanel.GetComponentInChildren<Slider>();
        m_sfxOptionImage.transform.GetChild(0).gameObject.SetActive(false);

        if (SoundManager.Instance != null)
        {
            m_bgmAduio = SoundManager.Instance.GetBGMAudio();
            m_sfxAudio = SoundManager.Instance.GetSFXAudio();

            m_bgmSlider.value = m_bgmAduio.volume;
            m_sfxSlider.value = m_sfxAudio.volume;
        }

        if(PlayerDataManager.Instance != null)
        {
            if(PlayerDataManager.Instance.GetBGMUnmute())
            {
                m_bgmOptionImage.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                m_bgmOptionImage.transform.GetChild(0).gameObject.SetActive(true);
            }

            if (PlayerDataManager.Instance.GetSFXUnmute())
            {
                m_sfxOptionImage.transform.GetChild(0).gameObject.SetActive(false);
            }
            else
            {
                m_sfxOptionImage.transform.GetChild(0).gameObject.SetActive(true);
            }
        }

        transform.parent.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bgmSlider != null)
        {
            BGMSlider();
        }

        if (m_sfxSlider != null)
        { 
            SFXSlider();
        }

    }
}
