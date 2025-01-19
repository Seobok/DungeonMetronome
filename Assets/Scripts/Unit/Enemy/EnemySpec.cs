using UnityEngine;

namespace Unit.Enemy
{
    [CreateAssetMenu(fileName = "EnemySpec", menuName = "Scriptable Objects/EnemySpec")]
    public class EnemySpec : ScriptableObject
    {
        public int _hp;
        public int _detectRange;
        public int _attackDamage;
        public int _moveSpeed;
        public Sprite _sprite;
    }
}
