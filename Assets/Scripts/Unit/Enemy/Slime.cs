using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy,IDamagable
{
    private int hp;

    private void Awake()
    {
        hp = 4;
        detactRange = 5;
        moveMaxCnt = 1;
        moveCnt = 1;
        attackDamage = 1;

        for(int i=0; i<3;i++)
        {
            for(int j=0;j<3;j++)
            {
                if (i == 1 && j == 1) continue;

                rightDirRange.Add(new Vector2(i - 1, j - 1));
            }
        }
    }

    public void Damaged(int amount, Unit causer)
    {
        if (amount <= 0) return;

        Debug.Log($"Slime이 {causer.name}에 의해 {amount}의 피해를 입었습니다.");
        EffectManager.instance.PlayParticle("HitEffect", transform.position);
        SoundManager.instance.PlaySFX("MonsterDamaged");
        hp -= amount;

        if (hp <= 0)
        {
            hp = 0;
            //죽는 애니메이션 또는 파티클
            MonsterSpawner.instance.Die(this);

            GameManager.instance.score += 20;
        }
    }
}
