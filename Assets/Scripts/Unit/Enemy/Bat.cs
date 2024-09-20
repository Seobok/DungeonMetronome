using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : Enemy, IDamagable
{
    private int hp;

    private void Awake()
    {
        hp = 1;
        detactRange = 5;
    }

    public void Damaged(int amount, Unit causer)
    {
        Debug.Log($"Bat가 {causer.name}에 의해 {amount}의 피해를 입었습니다.");
        EffectManager.instance.HitEffect(transform.position);
        hp -= amount;
        if(hp <= 0)
        {
            hp = 0;
            //죽는 애니메이션 또는 파티클
            MonsterSpawner.instance.Die(this);
        }
    }
}
