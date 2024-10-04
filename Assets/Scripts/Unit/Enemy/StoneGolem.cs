using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGolem : Enemy, IDamagable
{
    private Animator animator;

    [Header("StoneGolem Laser")]
    [SerializeField] private Transform muzzle;
    [SerializeField] private GameObject laser;
    private int hp;
    private float rightMuzzleXPos;

    private GameObject laserGo;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rightMuzzleXPos = muzzle.localPosition.x;

        hp = 10;
        detactRange = 7;
        moveMaxCnt = 1;
        moveCnt = 1;
        attackDamage = 2;
        rightDirRange = new List<Vector2>() { new Vector2(1, 0), new Vector2(2, 0), new Vector2(3, 0), new Vector2(4, 0) };
    }

    public override void SetRange()
    {
        foreach (var rightDir in rightDirRange)
        {
            leftDirRange.Add(new Vector2(-rightDir.x, rightDir.y));
        }
    }

    public override void SetAttackAnim(bool b)
    {
        animator.SetBool("IsAttack", b);

        if(b)
        {
            laserGo = Instantiate(laser);

            var laserSprite = laserGo.GetComponent<SpriteRenderer>();

            //Laser Effect Generate
            if (spriteRenderer.flipX)
            {
                muzzle.transform.localPosition = new Vector3(-rightMuzzleXPos, muzzle.transform.localPosition.y, muzzle.transform.localPosition.z);
                laserSprite.flipX = true;
            }
            else
            {
                muzzle.transform.localPosition = new Vector3(rightMuzzleXPos, muzzle.transform.localPosition.y, muzzle.transform.localPosition.z);
                laserSprite.flipX = false;
            }

            laserGo.transform.SetParent(muzzle);
            laserGo.transform.localPosition = Vector3.zero;
            laserSprite.sortingOrder = spriteRenderer.sortingOrder + 1;
        }
    }

    public override void Attack()
    {
        base.Attack();
        //Laser Animation
        if(laserGo)
        {
            laserGo.GetComponent<Animator>().SetBool("IsShot", true);
            StartCoroutine(DestroyLaser());
        }
    }

    public void Damaged(int amount, Unit causer)
    {
        if (amount <= 0) return;

        Debug.Log($"StoneGolem이 {causer.name}에 의해 {amount}의 피해를 입었습니다.");
        EffectManager.instance.PlayParticle("HitEffect", transform.position);
        SoundManager.instance.PlaySFX("MonsterDamaged");
        hp -= amount;
        if (hp <= 0)
        {
            hp = 0;
            //죽는 애니메이션 또는 파티클
            MonsterSpawner.instance.Die(this);

            GameManager.instance.score += 10;
        }
    }

    IEnumerator DestroyLaser()
    {
        float animationLength = laserGo.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        var destoryLaser = laserGo;
        Debug.Log(animationLength);

        yield return new WaitForSeconds(animationLength);

        Destroy(destoryLaser);
        SetAttackAnim(false);
    }
}
