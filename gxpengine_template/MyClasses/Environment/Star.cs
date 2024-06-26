﻿using GXPEngine;
using gxpengine_template.MyClasses.PickUps;
using Physics;
using System;
using System.Collections;
using TiledMapParser;

namespace gxpengine_template.MyClasses.Environment
{
    public class Star : PickUp
    {
        public event Action<Star> Grabbed;

        float _volume;

        StaticObj _trigger;

        public Star(string fileName, int c, int r, TiledObject data) : base(fileName, c, r, data)
        {
            AddChild(new AnimationCycler(this, data.GetIntProperty("AnimationDelayMs", 500)));

            AddChild(new Coroutine(Init()));
            
            // Sounds
            _volume = data.GetFloatProperty("Volume");
        }

        IEnumerator Init()
        {
            yield return null;
            _trigger = new StaticObj(this, true);
            _trigger.SetCollider(new Circle(_trigger, new Vec2(x, y), width / 2));
        }

        protected override void RemoveSelf()
        {
            visible = false;
            _trigger.Enabled = false;
        }

        public void ReEnable()
        {
            visible = true;
            _trigger.Enabled = true;
        }

        protected override void Grab(ITrigger taker)
        {
            Grabbed?.Invoke(this);
        }

        protected override void OnDestroy()
        {
            Grabbed = null;
        }
    }
}
