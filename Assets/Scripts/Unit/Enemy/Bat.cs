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
                        .Sequence()
                            .Execution(CanAttack)
                            .Execution(Attack)
                        .CloseComposite()
                        .Execution(MoveToTarget)
                    .CloseComposite()
                .CloseComposite()
                .Execution(MoveRandomly);

            //SetSpec
            EnemySpec batSpec = Resources.Load<EnemySpec>("Prefabs/Enemy/Bat");
            SetEnemySpec(batSpec);
        }
        
        public override void Act()
        {
            BehaviourTree.Invoke();
        }
    }
}