using Controller;
using Map;
using UnityEngine;
using VContainer;

namespace Unit.Player
{
    public class Knight : UnitBase
    {
        public Knight(Dungeon dungeon, UnitManager unitManager) : base(dungeon, unitManager)
        {
            GameObject.name = "Knight";
            PlayerController = new PlayerController(this);
            Renderer.sprite = Resources.Load<Sprite>("Sprites/Knight/knight");
        }
        
        private int Hp { get; set; }
        
        public PlayerController PlayerController { get; set; }
        
        public void TakeDamage(int damage)
        {
            Debug.Log("TakeDamage");
            Hp -= damage;
            if (Hp <= 0)
            {
                //Die();
            }
        }
    }
}
