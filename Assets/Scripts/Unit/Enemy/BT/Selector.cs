using System;

namespace Unit.Enemy.BT
{
    public class Selector : Composite
    {
        public override Result Invoke()
        {
            Result result = Result.Failure;

            for (int i = CurrentChildIndex; i < Children.Count; i++)
            {
                result = Children[i].Invoke();

                switch (result)
                {
                    case Result.Success:
                        CurrentChildIndex = 0;
                        return result;
                    
                    case Result.Failure:
                        CurrentChildIndex++;
                        break;
                    
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
