using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    public static bool IsEqual(float a, float b)
    {
        if(a>=b - Mathf.Epsilon && a <= b+Mathf.Epsilon)
        {
            return true;
        }
        else
            return false;
    }
    public static int GetProbability(int[] table)
    {
        int sum = 0;

        for(int i = 0; i < table.Length; i++)
        {
            sum += table[i];
        }

        //오류가 나는 경우
        if (sum <= 0)
            return -1;

        var num = Random.Range(1, sum);
        sum = 0;

        for (int i = 0; i < table.Length; i++)
        {
            var start = sum;
            if (num > start && num <= start + table[i])
                return i;

            sum += table[i];
        }

        return -1;

    }

    public static GameObject FindChildObject (GameObject go , string objname)
    {
        foreach(Transform tr in go.transform)
        {
            if(tr.name.Equals(objname))
            {
                return tr.gameObject;
            }
            else
            {
                GameObject findObj = FindChildObject(tr.gameObject, objname);
                if (findObj != null)
                    return findObj;
            }
        }
        return null;
    }

    //무조건 적으로 찾아주는게 아니다.
    public static T ToEnum<T> (string str)
    {
        // 받은 Enum을 배열로 반환
        System.Array A = System.Enum.GetValues(typeof(T));
        foreach (T t in A)
        {
            if (t.ToString().Equals(str))
                return t;
        }
        return default(T);
    }

}
