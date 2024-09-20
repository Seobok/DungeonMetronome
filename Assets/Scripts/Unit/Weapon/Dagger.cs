using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Weapon
{
    private void Awake()
    {
        damage = 1;
    }
    public override List<Tile> GetRange(Tile forwardTile, Vector2 dir)
    {
        List<Tile> range = new List<Tile>();
        range.Add(forwardTile);
        return range;
    }

    public override void AttackQTE(Player causer, List<IDamagable> damagableList)
    {
        var rand = Random.Range(2, 8);
        QTEManager.instance.ActiveDwindlingCircle(new List<float>() { (float)rand / 10 }, 2, causer, damagableList);
    }
}
