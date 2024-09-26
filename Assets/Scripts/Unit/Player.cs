using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit, IDamagable
{
    [HideInInspector] public Weapon weapon;

    [Header("Health")]
    private int maxHP;  //2의 배수
    private int currentHP;

    private void Awake()
    {
        maxHP = 6;
        currentHP = maxHP;
    }

    /// <summary>
    /// 공격 범위 안에 있는 IDamagable 유닛에게 피해를 입히는 함수
    /// </summary>
    /// <param name="inRange">공격범위 안에 있는 IDamagable 유닛</param>
    public void Attack(List<IDamagable> inRange, float damageRate)
    {
        if(weapon == null)
        {
            //공격할 수 없음
            return;
        }

        foreach(var damagableUnit in inRange)
        {
            damagableUnit.Damaged((int)(weapon.damage * damageRate), this);
            if((int)(weapon.damage * damageRate) > 0)
            {
                StartCoroutine(CameraShake.Shake(0.1f, 0.1f, transform.position));
            }
        }
    }

    public void Damaged(int amount, Unit causer)
    {
        if (amount <= 0) return;

        Debug.Log($"player가 {causer.name}에 의해 {amount}의 피해를 입었습니다.");

        StartCoroutine(CameraShake.Shake(0.1f, 0.1f, transform.position));

        currentHP -= amount;

        if (currentHP < 0)
        {
            currentHP = 0;
        }

        if (InGameUIManager.Instance != null)
        {
            InGameUIManager.Instance.SetHealthBar(maxHP, currentHP);
        }

        EffectManager.instance.HitEffect(transform.position);
    }
}
