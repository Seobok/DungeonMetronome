using Effect;
using Map;
using Unit.Enemy.BT;
using UnityEngine;
using Utility;

namespace Unit.Enemy
{
    public class Slime : Enemy
    {
        public Slime(Dungeon dungeon, UnitManager unitManager, EffectPool effectPool) : base(dungeon, unitManager,effectPool)
        {
            Renderer.sprite = Resources.Load<Sprite>("Sprites/Slime/Slime");
            Animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Sprites/Slime/Animator/Slime");
            
            //BT
            BehaviourTree = new BehaviourTree();
            BehaviourTree.SetRoot(new Selector())
                .Sequence()
                    .Selector()
                        .Execution(new HasTargetPlayerAction(BlackBoard))
                        .Execution(new DetectTargetPlayerAction(dungeon, unitManager, BlackBoard, this))
                    .CloseComposite()
                    .Selector()
                        .Sequence()
                            .Execution(new CheckTooFarAction(BlackBoard, this))
                            .Execution(new RemoveTargetAction(BlackBoard))
                        .CloseComposite()
                        .Sequence()
                            .Execution(new CanAttackAction(dungeon, BlackBoard, this))
                            .Execution(new AttackTargetTileAction(BlackBoard, this))
                        .CloseComposite()
                        .Execution(new MoveToTargetAction(Dungeon, BlackBoard, this))
                    .CloseComposite()
                .CloseComposite()
                .Execution(new MoveRandomlyAction(Dungeon, BlackBoard, this));

            Hp = CsvReader.EnemyData[nameof(Slime)].Hp;
            DetectRange = CsvReader.EnemyData[nameof(Slime)].DetectRange;
            MoveSpeed = CsvReader.EnemyData[nameof(Slime)].MoveSpeed;
            
            BlackBoard.Patterns.Add(new Coord[]
            {
                new Coord(0, 1),
                new Coord(1, 0),
                new Coord(0, -1),
                new Coord(1, 1),
                new Coord(1, -1),
                new Coord(-1, 0),
                new Coord(-1, -1),
                new Coord(-1, 1),
            });
        }

        
    }
}