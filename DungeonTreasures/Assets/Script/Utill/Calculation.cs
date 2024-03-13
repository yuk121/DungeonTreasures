using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    None = -1,
    Normal,
    Critical,
    Miss
}
static public class Calculation
{
    //공격 명중 처리
    static public bool AttackDecision (float attacker_Hit, float defence_Dodge)
    {
        //명중률이 100이면 무조건 트루로 반환
        if (Util.IsEqual(attacker_Hit, 100.0f) || attacker_Hit > 100.0f)
            return true;

        float checkHitSum = attacker_Hit + defence_Dodge;
        float checkHitRand = Random.Range(0, checkHitSum + 1);

        if(checkHitRand < attacker_Hit)
        {
            //공격 성공
            return true; 
        }
        return false;
    }
   
    //일반 데미지 처리
    static public float NormalDamage(float attacker_atk, float action_atk, float defence_def)
    {
        float attack = attacker_atk + (attacker_atk * action_atk / 100);
        float _damage = attack - defence_def;
        return _damage;
    }
   
    // 치명타 판정
    static public bool CriticalDecision(float critical)
    {
        int randValue = Random.Range(0, 100 + 1);
        if(randValue <= critical) // 치명타
        {
            return true;
        }
        return false;
    }

    //크리티컬 데미지 처리
    static public float CriticalDamage(float damage , float critical_atk)
    {
        return damage = damage + (damage * (critical_atk / 100f));
    }
}
