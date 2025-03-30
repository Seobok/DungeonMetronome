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
            
            //BT
            BehaviourTree = new BehaviourTree();
            BehaviourTree.SetRoot(new Selector())
                .Sequence()
                    .Selector()
                        .Execution(HasTargetPlayer)
                        .Execution(DetectTargetPlayer)
                    .CloseComposite()
                    .Selector()
                        .Sequence()
                            .Execution(CheckTooFar)
                            .Execution(RemoveTarget)
                        .CloseComposite()
                        .Execution(MoveToTarget)
                    .CloseComposite()
                .CloseComposite()
                .Execution(MoveRandomly);

            Hp = CsvReader.EnemyData[nameof(Bat)].Hp;
            DetectRange = CsvReader.EnemyData[nameof(Bat)].DetectRange;
            MoveSpeed = CsvReader.EnemyData[nameof(Bat)].MoveSpeed;
        }


        public override int Hp { get; set; }
        public override int DetectRange { get; set; }
        public override int MoveSpeed { get; set; }

        
        public override void Act()
        {
            BehaviourTree.Invoke();
        }
    }
}