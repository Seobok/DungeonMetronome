using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item
{
    [HideInInspector] public int damage = 0;
    public abstract List<Tile> GetRange(Tile curTile, Vector2 dir);
    public abstract void AttackQTE(Player causer, List<IDamagable> damagableList);
}
