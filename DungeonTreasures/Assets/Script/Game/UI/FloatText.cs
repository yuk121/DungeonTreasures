using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatText : MonoBehaviour
{
    Animator m_animator;
    Text m_text;
    float m_time = 0f;
    [SerializeField]
    float m_checkTime = 2f;
    bool m_isHit = false;
    
    public void SetEnemyHudText(GameObject user, ePlayerDamageType type,int damage)
    {
        gameObject.SetActive(true);
        gameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);

        if (type == ePlayerDamageType.Normal)
        {
            m_text.text = string.Format("-{0}", damage);
            m_animator.SetTrigger("Normal");
            
        }
        else if(type == ePlayerDamageType.Critical)
        {
            m_text.text = string.Format("-{0}", damage);
            m_animator.SetTrigger("Critical");
        }
        else
        {
            m_text.text = string.Format("Miss");
            m_animator.SetTrigger("Miss");
        }
        m_time = 0f;
        m_isHit = true;
    }

    public void SetPlayerText(GameObject user, eMonsterDamageType type, int damage)
    {
        gameObject.SetActive(true);
        transform.LookAt(GameObject.Find("Main Camera").transform);
        gameObject.transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y -180, transform.localEulerAngles.z);

        if (type == eMonsterDamageType.Normal)
        {
            m_text.text = string.Format("-{0}", damage);
            m_animator.SetTrigger("Normal");

        }
        else if (type == eMonsterDamageType.Critical)
        {
            m_text.text = string.Format("-{0}", damage);
            m_animator.SetTrigger("Critical");
        }
        else
        {
            m_text.text = string.Format("Miss", Color.yellow);
            m_animator.SetTrigger("Normal");
        }
        m_time = 0f;
        m_isHit = true;
    }

    // Start is called before the first frame update

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_text = GetComponent<Text>();
        gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (m_isHit)
        {
            m_time = m_time + Time.deltaTime;

            if (m_time >= m_checkTime)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
