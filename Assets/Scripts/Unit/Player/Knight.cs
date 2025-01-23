using Map;
using UnityEngine;

namespace Unit.Player
{
    public class Knight : UnitBase
    {
        public override Coord Position
        {
            get => new Coord((int)Manager.KnightGameObject.transform.position.x, (int)Manager.KnightGameObject.transform.position.y);
            set => Manager.KnightGameObject.transform.position = new Vector3(value.X, value.Y, Manager.KnightGameObject.transform.position.z);
        }
    }
}
