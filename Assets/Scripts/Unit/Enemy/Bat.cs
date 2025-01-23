using Unit.Enemy.BT;
using UnityEngine;

namespace Unit.Enemy
{
    public class Bat : Enemy
    {
        public Bat()
        {
            //BT
            BehaviourTree = new BehaviourTree();
            // BehaviourTree.SetRoot(new Selector())
            //     .Sequence()
            //         .Selector()
            //             .Execution(HasTargetPlayer)
            //             .Execution(DetectTargetPlayer)
            //         .CloseComposite()
            //         .Selector()
            //             .Sequence()
            //                 .Execution(CheckTooFar)
            //                 .Execution(RemoveTarget)
            //             .CloseComposite()
            //             .Sequence()
            //                 .Execution(CanAttack)
            //                 .Execution(Attack)
            //             .CloseComposite()
            //             .Execution(MoveToTarget)
            //         .CloseComposite()
            //     .CloseComposite()
            //     .Execution(MoveRandomly);
            BehaviourTree.SetRoot(new Sequence())
                .Execution(DetectTargetPlayer)
                .Execution(MoveToTarget);

        }


        public override int Hp { get; set; } = 1;
        public override int DetectRange { get; set; } = 5;
        public override int MoveSpeed { get; set; } = 2;

        
        public override void Act()
        {
            BehaviourTree.Invoke();
        }
    }
}