using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : Item
{
    public abstract List<Tile> GetRange(Tile curTile);
    [HideInInspector] public int damage = 0;
}
