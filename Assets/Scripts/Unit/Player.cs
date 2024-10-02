using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Player : Unit, IDamagable
{
    private Weapon _weapon;
    public Weapon weapon
    {
        get { return _weapon; }
        set 
        {
            _weapon = value;
            if (_weapon == null)
                InGameUIManager.Instance.SetInventorySlot(0, null);
            else
            {
                InGameUIManager.Instance.SetInventorySlot(0, _weapon.spriteRenderer.sprite);
            }
        }
    }

    private SpriteRenderer _spriteRenderer;

    [Header("Health")]
    private int maxHP;  //2의 배수
    private int currentHP;

    private void Awake()
    {
        //Property
        maxHP = 6;
        currentHP = maxHP;

        //Initialize
        _spriteRenderer = GetComponent<SpriteRenderer>();
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

        EffectManager.instance.PlayParticle("HitEffect", transform.position, _spriteRenderer.sortingOrder + 1);
        SoundManager.instance.PlaySFX("Damaged");
    }
}
