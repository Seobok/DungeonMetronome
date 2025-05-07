namespace Unit.Enemy.BT
{
    public class AttackTargetTileAction : Node
    {
        private readonly BlackBoard _blackBoard;
        private readonly Enemy _enemy;

        public AttackTargetTileAction(BlackBoard blackBoard, Enemy enemy)
        {
            _blackBoard = blackBoard;
            _enemy = enemy;
        }

        public override Result Invoke()
        {
            if (_blackBoard.IsReadyToAttack)
            {
                _enemy.Attack(_blackBoard.NextAttackCoords);
                _blackBoard.IsReadyToAttack = false;
                return Result.Success;
            }

            _blackBoard.IsReadyToAttack = true;
            _enemy.SetNextAttackTile(_blackBoard.NextAttackCoords);
            return Result.Running;
        }
    }
}