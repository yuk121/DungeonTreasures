using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPath : SingleTonMonoBehaviour<CameraPath>
{
    [SerializeField]
    Transform m_cameraPathObj = null;
    Transform m_targetPoint;

    Camera m_camera;

    [SerializeField]
    int m_speed = 5;
    int m_index = 0;

    public bool m_fistPathDone = false;

    public void SetCameraPath()
    {
        var camerMove = gameObject.GetComponent<CameraMove>();
        camerMove.enabled = false;
        camerMove.SetCameraMoveFalse();

        m_index = 0;
        m_targetPoint = m_cameraPathObj.GetChild(m_index);
    }

    public void ChanageCameraPos(GameObject closeUpObj)
    {
        var cameraPos = closeUpObj.gameObject.GetComponent<Transform>();
       
        transform.position = new Vector3(cameraPos.position.x, cameraPos.position.y, cameraPos.position.z);  
        transform.eulerAngles = new Vector3(cameraPos.eulerAngles.x, cameraPos.eulerAngles.y, 0f);
        transform.localScale = Vector3.one;
    }
    // Start is called before the first frame update
    void Awake()
    {
        m_camera = GetComponent<Camera>();
        m_index = 0;
        m_camera.enabled = false;
        m_targetPoint = m_cameraPathObj.GetChild(m_index);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_index < m_cameraPathObj.childCount && m_fistPathDone == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_targetPoint.position, m_speed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, m_targetPoint.rotation, m_speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, m_targetPoint.position) < 0.1)
            {
                 if(m_index == 1)
                {
                    m_camera.enabled = true;
                }

                m_index++;

                if (m_index < m_cameraPathObj.childCount)
                    m_targetPoint = m_cameraPathObj.GetChild(m_index);
                else
                    m_fistPathDone = true;
            }
        }
    }
}
