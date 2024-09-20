using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualDagger : Weapon
{
    private void Awake()
    {
        damage = 3;
    }
    public override void AttackQTE(Player causer, List<IDamagable> damagableList)
    {
        var rand1 = Random.Range(2, 6);
        var rand2 = Random.Range(6, 10);
        QTEManager.instance.ActiveDwindlingCircle(new List<float>() { (float)rand1 / 10, (float)rand2 / 10 }, 2, causer, damagableList);
    }

    public override List<Tile> GetRange(Tile forwardTile, Vector2 dir)
    {
        List<Tile> range = new List<Tile>();
        range.Add(forwardTile);
        return range;
    }
}
