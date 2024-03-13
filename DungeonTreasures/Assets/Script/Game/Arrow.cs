using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : SingleTonMonoBehaviour<Arrow>
{
    TrailRenderer m_trailRender;
    CharacterAttack m_player;
    Vector3 m_foward;
    Vector3 m_prevPos;
    Vector3 m_dir;

    const float m_radius = 2f;

    public void SetArrow(Vector3 foward, CharacterAttack player)
    {
        m_foward = foward;
        if(m_trailRender != null)
            m_trailRender.Clear();

        m_player = player;
        m_prevPos = transform.position;
    }

    public void GetTargetDir(GameObject target)
    {
        m_dir = target.transform.position - transform.position;   
    }

    void CheckCollision()
    {
        float dist = Vector3.Distance(transform.position, m_prevPos);
        RaycastHit hit = new RaycastHit();

        if (m_dir != Vector3.zero)
        {
            if (Physics.SphereCast(transform.position,m_radius ,(m_foward + m_dir + Vector3.up).normalized , out hit, dist, 1 << LayerMask.NameToLayer("Enemy")))
            {  
                transform.position = hit.point;           
                m_dir = Vector3.zero;
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (Physics.SphereCast(transform.position, m_radius, m_foward, out hit, dist, 1 << LayerMask.NameToLayer("Enemy")))
            {
                transform.position = hit.point;
                m_player.aEvent_Attack();
                m_dir = Vector3.zero;
                gameObject.SetActive(false);
            }
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        m_trailRender = GetComponentInChildren<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // 멀티타겟인 경우
        if (m_dir != Vector3.zero)
        {
            transform.position += (m_foward +Vector3.up+ m_dir).normalized * 20f * Time.deltaTime;  
        }
        else   // 싱글타겟인 경우
        {
            transform.position += new Vector3(m_foward.x, -0.01f, m_foward.z).normalized * 20f * Time.deltaTime;           
        }

        CheckCollision();
        m_prevPos = transform.position;
    }
}
