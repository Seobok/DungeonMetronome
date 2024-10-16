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
    [HideInInspector] public int maxHP;  //2의 배수
    [HideInInspector] public int currentHP;

    [Header("FogOfWar")]
    List<Vector2> visibleRange = new List<Vector2>();
    List<Tile> curVisible = new List<Tile>();
    const int VISIBLE_DIST = 5;

    private void Awake()
    {
        //Property
        maxHP = 6;
        currentHP = maxHP;

        //Initialize
        _spriteRenderer = GetComponent<SpriteRenderer>();

        //FogOfWar
        //VisibleRange Init
        for (int i = -VISIBLE_DIST; i <= VISIBLE_DIST; i++)
        {
            for (int j = -VISIBLE_DIST; j <= VISIBLE_DIST; j++)
            {
                if (Mathf.Abs(i) + Mathf.Abs(j) > VISIBLE_DIST) continue;

                visibleRange.Add(new Vector2(i, j));
            }
        }
    }

    /// <summary>
    /// 공격 범위 안에 있는 IDamagable 유닛에게 피해를 입히는 함수
    /// </summary>
    /// <param name="inRange">공격범위 안에 있는 IDamagable 유닛</param>
    public void Attack(List<IDamagable> inRange, float damageRate, bool isPower)
    {
        if(weapon == null)
        {
            //공격할 수 없음
            return;
        }

        float multiple = 1f;
        if (isPower) multiple = 2f;

        foreach(var damagableUnit in inRange)
        {
            damagableUnit.Damaged((int)(weapon.damage * damageRate * multiple), this);
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

        if (currentHP <= 0)
        {
            currentHP = 0;
            InGameUIManager.Instance.ActiveGameOver(causer.name);
        }

        if (InGameUIManager.Instance != null)
        {
            InGameUIManager.Instance.SetHealthBar(maxHP, currentHP);
        }

        EffectManager.instance.PlayParticle("HitEffect", transform.position, _spriteRenderer.sortingOrder + 1);
        SoundManager.instance.PlaySFX("Damaged");
    }

    public void ShowVisibleTile()
    {
        foreach(var tile in curVisible)
        {
            tile.isVisible = false;
        }

        curVisible.Clear();
        curVisible = curRoom.GetTiles(GetTile(), visibleRange);

        foreach(var tile in curVisible)
        {
            tile.isVisible = true;
        }
    }
}
