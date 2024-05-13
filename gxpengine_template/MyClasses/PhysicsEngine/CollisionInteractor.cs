using GXPEngine;
using System;
using System.Collections.Generic;

namespace Physics
{
    public abstract class CollisionInteractor : GameObject
    {
        public event Action<Collider> OnTriggerStay;

        public float Bounciness { get; set; } = 1f;
        public Collider Collider => myCollider;
        public bool IsTrigger => isTrigger;
        private bool _enabled;

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                if (value)
                    SetCollider(myCollider);
                else
                    DisableCollider();
                _enabled = value;
            }
        } 


        protected Collider myCollider { get; private set; }
        protected readonly ColliderManager engine;
        protected readonly bool isTrigger;

        public CollisionInteractor(GameObject owner, bool isTrigger = false, float bounciness = 1)
        {
            this.isTrigger = isTrigger;
            engine = ColliderManager.main;
            Bounciness = bounciness;
            owner.AddChild(this);
        }

        public void SetCollider(Collider col)
        {
            myCollider = col;
            if (isTrigger)
                engine.AddTriggerCollider(myCollider);
            else
                engine.AddSolidCollider(myCollider);

        }

        protected virtual void OnCollision(CollisionInfo info) { }

        protected virtual void OnTrigger(Collider collider) { }

        protected abstract void ResolveCollision(CollisionInfo colInfo);


        protected void CheckForTriggers()
        {
            foreach (Collider overlap in engine.GetOverlaps(myCollider))
            {
                OnTrigger(overlap);
                OnTriggerStay?.Invoke(overlap);
            }
        }

        protected override void OnDestroy()
        {
            OnTriggerStay = null;
            DisableCollider();
        }

        void DisableCollider()
        {
            if (isTrigger)
                engine.RemoveTriggerCollider(myCollider);
            else
                engine.RemoveSolidCollider(myCollider);
        }

        public List<Collider> GetOverlaps()
        {
            return engine.GetOverlaps(myCollider);
        }

        public void UpdateColliderPosition()
        {
            myCollider.position = new Vec2(parent.x, parent.y);
        }
    }
}
