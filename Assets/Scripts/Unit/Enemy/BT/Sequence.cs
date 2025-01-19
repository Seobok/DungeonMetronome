using System;

namespace Unit.Enemy.BT
{
    public class Sequence : Composite
    {
        public override Result Invoke()
        {
            Result result = Result.Success;

            for (int i = CurrentChildIndex; i < Children.Count; i++)
            {
                result = Children[i].Invoke();

                switch (result)
                {
                    case Result.Success:
                        CurrentChildIndex++;
                        break;
                    
                    case Result.Failure:
                        CurrentChildIndex = 0;
                        return result;
                    
                    case Result.Running:
                        return result;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            CurrentChildIndex = 0;
            return result;
        }
    }
}
