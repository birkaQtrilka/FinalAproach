using GXPEngine;
using gxpengine_template.MyClasses.PickUps;

namespace gxpengine_template
{
    public class PickUper : GameObject
    {
        readonly ITrigger _owner;

        public PickUper(ITrigger owner) 
        {
            _owner = owner;
            _owner.TriggerEnter += OnTrigger;
        }

        private void OnTrigger(GameObject obj)
        {
            if(obj is PickUp pick)
            {
                pick.Take(_owner);
            }
        }

        protected override void OnDestroy()
        {
            _owner.TriggerEnter -= OnTrigger;
        }

    }
}
