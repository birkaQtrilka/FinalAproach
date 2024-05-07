using System;

namespace gxpengine_template.MyClasses.Coroutines
{
    public class WaitUntill : ICoroutineStepper
    {
        readonly Func<bool> _condition;

        public WaitUntill(Func<bool> condition)
        {
            _condition = condition;
        }

        public bool IsDone()
        {
            return _condition();
        }
    }
}
