using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneGolem : Enemy, IDamagable
{
    private Animator animator;

    private int hp;
    [SerializeField] private GameObject laser;
    private float rightMuzzleXPos;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rightMuzzleXPos = laser.transform.localPosition.x;

        hp = 5;
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
        if(spriteRenderer.flipX)
        {
            laser.transform.localPosition = new Vector3(-rightMuzzleXPos, laser.transform.localPosition.y, laser.transform.localPosition.z);
            laser.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            laser.transform.localPosition = new Vector3(rightMuzzleXPos, laser.transform.localPosition.y, laser.transform.localPosition.z);
            laser.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    public override void Attack()
    {
        base.Attack();
        //Laser Animation
    }

    public void Damaged(int amount, Unit causer)
    {
        if (amount <= 0) return;

        Debug.Log($"StoneGolem이 {causer.name}에 의해 {amount}의 피해를 입었습니다.");
        EffectManager.instance.PlayParticle("HitEffect", transform.position);
        hp -= amount;
        if (hp <= 0)
        {
            hp = 0;
            //죽는 애니메이션 또는 파티클
            MonsterSpawner.instance.Die(this);

            GameManager.instance.score += 10;
        }
    }
}
