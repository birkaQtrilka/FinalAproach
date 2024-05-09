using GXPEngine;

namespace Physics
{
    public abstract class CollisionInteractor : GameObject
    {
        public float bounciness = 1f;

        public Collider Collider => myCollider;
        public bool IsTrigger => isTrigger;

        protected Collider myCollider { get; private set; }
        protected readonly ColliderManager engine;
        protected readonly bool isTrigger;

        public CollisionInteractor(GameObject owner, bool isTrigger = false, float bounciness = 1)
        {
            this.isTrigger = isTrigger;
            engine = ColliderManager.main;
            this.bounciness = bounciness;
            owner.AddChild(this);
        }

        public void SetCollider(Collider col)
        {
            myCollider = col;
            if (isTrigger)
                engine.AddTriggerCollider(myCollider);
            else
                engine.AddSolidCollider(myCollider);

            //parent.x = myCollider.position.x;
            //parent.y = myCollider.position.y;
        }

        public virtual void OnCollision(CollisionInfo info) { }

        public virtual void OnTrigger(Collider collider) { }

		public abstract void ResolveCollision(CollisionInfo colInfo);

        protected override void OnDestroy()
        {
            if (isTrigger)
                engine.RemoveTriggerCollider(myCollider);
            else
                engine.RemoveSolidCollider(myCollider);
        }


    }
}
