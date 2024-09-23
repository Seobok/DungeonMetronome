using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 피해를 입을 수 있는 유닛
/// HP를 가지도록 설계하는 것이 기본
/// Damaged : causer로 부터 amount 만큼의 피해를 입었을 때 실행할 함수
/// </summary>
public interface IDamagable
{
    public void Damaged(int amount, Unit causer);
}
