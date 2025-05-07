using Map;
using Unit.Enemy.BT;
using UnityEngine;
using Utility;
using VContainer;

namespace Unit.Enemy
{
    public class Bat : Enemy
    {
        public Bat(Dungeon dungeon, UnitManager unitManager) : base(dungeon, unitManager)
        {
            Renderer.sprite = Resources.Load<Sprite>("Sprites/Bat/Bat");
            Animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>("Sprites/Bat/Animator/Bat");
            
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
                        .Execution(new MoveToTargetAction(Dungeon, BlackBoard, this))
                    .CloseComposite()
                .CloseComposite()
                .Execution(new MoveRandomlyAction(Dungeon, BlackBoard, this));

            Hp = CsvReader.EnemyData[nameof(Bat)].Hp;
            DetectRange = CsvReader.EnemyData[nameof(Bat)].DetectRange;
            MoveSpeed = CsvReader.EnemyData[nameof(Bat)].MoveSpeed;
        }
    }
}