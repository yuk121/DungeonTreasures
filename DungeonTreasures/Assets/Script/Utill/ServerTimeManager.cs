using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.Networking;

public class ServerTimeManager : DontDestory<ServerTimeManager>
{
    [SerializeField]
    string m_serverUrl = "";

    string m_date;
    double m_elapsedTime;

    public string GetDate()
    {
        return m_date;
    }
    public void DateTimeCheck()
    {
        Debug.Log("#로그 : 체크 코루틴 들어옴");
        StartCoroutine("WebCheck");
    }

    protected override void OnAwake()
    {
        StartCoroutine("WebCheck");
    }
    IEnumerator WebCheck()
    {
        UnityWebRequest request = new UnityWebRequest();

        using (request = UnityWebRequest.Get(m_serverUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("#로그 Error : " + request.result);
            }
            else
            {
                m_date = request.GetResponseHeader("date");
                DateTime dateTime = DateTime.Parse(m_date).ToUniversalTime().AddHours(9);
                m_date = dateTime.ToString();

                Debug.Log("#로그 WebCheck / m_date " + m_date);
            }
        }
    }
    //마지막 접속일로 부터 경과시간 초단위로 체크
    public void CheckElapsedUnixTime()
    {
        if (PlayerDataManager.Instance != null)
        {
            string firstAccessDate = PlayerDataManager.Instance.GetFirstAccessTime();
            string lastAccessDate = PlayerDataManager.Instance.GetLastAccessTime();

            if (string.IsNullOrEmpty(firstAccessDate) == false && string.IsNullOrEmpty(lastAccessDate) == false)
            {
                DateTime firstDateTime = Convert.ToDateTime(firstAccessDate);
                DateTime lastDateTime = Convert.ToDateTime(lastAccessDate);

                TimeSpan timespan = firstDateTime - lastDateTime;

                m_elapsedTime = timespan.TotalSeconds;

                Debug.Log("#로그 m_elapsedTime: " + m_elapsedTime);
            }
            else
            {
                m_elapsedTime = 0;
            }
        }
    }

    public double GetElaspedTime()
    {
        return m_elapsedTime;
    }

}
