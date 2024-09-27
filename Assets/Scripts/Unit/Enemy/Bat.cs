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
        moveMaxCnt = 2;
        moveCnt = 2;
        attackDamage = 1;
        rightDirRange = new List<Vector2>() { new Vector2(1, 0) };
    }

    public void Damaged(int amount, Unit causer)
    {
        if (amount <= 0) return;

        Debug.Log($"Bat가 {causer.name}에 의해 {amount}의 피해를 입었습니다.");
        EffectManager.instance.PlayParticle("HitEffect", transform.position);
        hp -= amount;
        if(hp <= 0)
        {
            hp = 0;
            //죽는 애니메이션 또는 파티클
            MonsterSpawner.instance.Die(this);
        }
    }
}
