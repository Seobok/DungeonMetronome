using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingBot : Unit, IDamagable
{
    public void Damaged(int amount, Unit causer)
    {
        Debug.Log($"{causer.name}에게 {amount}의 피해를 입었습니다.");
        EffectManager.instance.HitEffect(transform.position);
    }
}
