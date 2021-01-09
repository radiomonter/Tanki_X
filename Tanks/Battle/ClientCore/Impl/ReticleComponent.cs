namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class ReticleComponent : Component
    {
        private const string objectName = "reticle";
        private Object prefabData;
        private Transform parent;
        private List<Reticle> reticles = new List<Reticle>();
        [CompilerGenerated]
        private static Func<IGrouping<Entity, TargetData>, TargetData> <>f__mg$cache0;
        [CompilerGenerated]
        private static Func<TargetData, bool> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<TargetData, Entity> <>f__am$cache1;
        [CompilerGenerated]
        private static Func<TargetData, Entity> <>f__am$cache2;
        [CompilerGenerated]
        private static Func<TargetData, TargetData> <>f__am$cache3;
        [CompilerGenerated]
        private static Func<Reticle, bool> <>f__am$cache4;

        public void Create(Object prefabData, Transform parent)
        {
            this.prefabData = prefabData;
            this.parent = parent;
            this.reticles.Add(new Reticle(prefabData, parent, this.CanvasSize));
        }

        public void Deactivate()
        {
            foreach (Reticle reticle in this.reticles)
            {
                reticle.Deactivate();
            }
        }

        public void Destroy()
        {
            foreach (Reticle reticle in this.reticles)
            {
                reticle.GameObject.RecycleObject();
            }
            this.reticles.Clear();
        }

        private Dictionary<Entity, TargetData> GetHammerTargets(List<TargetData> targets)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = x => x.PriorityWeakeningCount == 0;
            }
            <>f__am$cache1 ??= x => x.TargetEntity;
            <>f__mg$cache0 ??= new Func<IGrouping<Entity, TargetData>, TargetData>(Enumerable.First<TargetData>);
            <>f__am$cache2 ??= x => x.TargetEntity;
            if (<>f__am$cache3 == null)
            {
                <>f__am$cache3 = x => x;
            }
            return (from x in targets.Where<TargetData>(<>f__am$cache0).GroupBy<TargetData, Entity>(<>f__am$cache1).Select<IGrouping<Entity, TargetData>, TargetData>(<>f__mg$cache0)
                where this.isValidAndVisible(x)
                select x).ToDictionary<TargetData, Entity, TargetData>(<>f__am$cache2, <>f__am$cache3);
        }

        private bool isValidAndVisible(TargetData targetData) => 
            (targetData.ValidTarget && !targetData.TargetEntity.HasComponent<TankInvisibilityEffectWorkingStateComponent>()) && !targetData.TargetEntity.HasComponent<TankInvisibilityEffectActivationStateComponent>();

        public void Reset()
        {
            foreach (Reticle reticle in this.reticles)
            {
                reticle.Reset();
            }
        }

        public void SetFree()
        {
            foreach (Reticle reticle in this.reticles)
            {
                reticle.SetFree();
            }
        }

        private void SetHammerTargets(List<TargetData> targets, Vector2 canvasSize)
        {
            Dictionary<Entity, TargetData> hammerTargets = this.GetHammerTargets(targets);
            foreach (Reticle reticle in this.reticles)
            {
                if ((reticle.Entity == null) || !hammerTargets.ContainsKey(reticle.Entity))
                {
                    reticle.SetFree();
                    continue;
                }
                reticle.SetEnemy(hammerTargets[reticle.Entity]);
                hammerTargets.Remove(reticle.Entity);
            }
            foreach (KeyValuePair<Entity, TargetData> pair in hammerTargets)
            {
                if (<>f__am$cache4 == null)
                {
                    <>f__am$cache4 = x => ReferenceEquals(x.Entity, null);
                }
                Reticle item = this.reticles.FirstOrDefault<Reticle>(<>f__am$cache4);
                if (item == null)
                {
                    item = new Reticle(this.prefabData, this.parent, this.CanvasSize);
                    this.reticles.Add(item);
                }
                item.Entity = pair.Key;
                item.SetEnemy(hammerTargets[item.Entity]);
            }
        }

        public void SetTargets(List<TargetData> targets, Vector2 canvasSize)
        {
            if (this.Hammer)
            {
                this.SetHammerTargets(targets, canvasSize);
            }
            else if (this.reticles.Any<Reticle>())
            {
                Reticle reticle = this.reticles.First<Reticle>();
                TargetData targetData = targets.FirstOrDefault<TargetData>(x => (x.PriorityWeakeningCount == 0) && this.isValidAndVisible(x));
                if (targetData == null)
                {
                    reticle.SetFree();
                }
                else if (this.CanHeal && (targetData.TargetEntity.GetComponent<TeamGroupComponent>().Key == this.TeamKey))
                {
                    reticle.SetTeammate(targetData);
                }
                else
                {
                    reticle.SetEnemy(targetData);
                }
            }
        }

        public bool Hammer { get; set; }

        public bool CanHeal { get; set; }

        public long TeamKey { get; set; }

        public Vector2 CanvasSize { get; set; }

        private class Reticle
        {
            private const string nameOfDefaultAnimation = "Hide";
            private const string nameOfHideEnemyAnimation = "HideEnemy";
            private const string nameOfHideTeammateAnimation = "HideTeammate";
            private const float reticleSize = 0.12f;
            private const float freeAnimationTime = 1f;
            private const float reticleHeightCorrection = 1.1f;
            public Platform.Kernel.ECS.ClientEntitySystem.API.Entity Entity;
            private Vector2 canvasSize;
            private Vector3 lastWorldPosition = ((Vector3) Vector2.zero);
            private UnityEngine.GameObject gameObject;
            private Animator animator;
            private Transform transform;
            private State state;
            private bool active = true;
            private ReticleComponent.ReticleTimer timer;

            public Reticle(Object prefabData, Transform parent, Vector2 canvasSize)
            {
                this.canvasSize = canvasSize;
                UnityEngine.GameObject obj2 = Object.Instantiate(prefabData) as UnityEngine.GameObject;
                obj2.name = "reticle";
                obj2.transform.SetParent(parent);
                obj2.transform.SetAsFirstSibling();
                obj2.transform.localScale = Vector3.one;
                obj2.transform.localPosition = Vector3.zero;
                RectTransform component = obj2.GetComponent<RectTransform>();
                Vector2 anchorMin = component.anchorMin;
                anchorMin.y = 0.44f;
                component.anchorMin = anchorMin;
                Vector2 anchorMax = component.anchorMax;
                anchorMax.y = 0.56f;
                component.anchorMax = anchorMax;
                component.offsetMax = Vector2.zero;
                component.offsetMin = Vector2.zero;
                this.timer = obj2.AddComponent<ReticleComponent.ReticleTimer>();
                this.GameObject = obj2;
            }

            public void Deactivate()
            {
                this.animator.Play("Hide");
                this.active = false;
                this.Detach();
            }

            private void Detach()
            {
                this.Entity = null;
                this.timer.Break();
            }

            private Vector2 GetPoint(Vector3 originWithOffset)
            {
                Vector2 vector = Camera.main.WorldToScreenPoint(originWithOffset);
                Vector2 vector2 = new Vector2((vector.x / ((float) Screen.width)) * this.canvasSize.x, (vector.y / ((float) Screen.height)) * this.canvasSize.y);
                return (vector2 - (this.canvasSize / 2f));
            }

            public void Reset()
            {
                this.state = State.Hiden;
                this.animator.Play("Hide");
                this.active = true;
            }

            public void SetEnemy(TargetData targetData)
            {
                this.SetTarget(targetData, State.HighlightEnemy);
            }

            public void SetFree()
            {
                if (this.active && (this.state != State.Hiden))
                {
                    if (this.state == State.HighlightEnemy)
                    {
                        this.animator.Play("HideEnemy");
                    }
                    else
                    {
                        this.animator.Play("HideTeammate");
                    }
                    this.state = State.Hiden;
                    this.timer.SetAction(new Action(this.SetLastPoint), 1f, new Action(this.Detach));
                }
            }

            private void SetLastPoint()
            {
                this.transform.localPosition = (Vector3) this.GetPoint(this.lastWorldPosition);
            }

            private void SetTarget(TargetData targetData, State state)
            {
                if (this.active)
                {
                    Vector3 originWithOffset = targetData.TargetPosition + (targetData.TargetEntity.GetComponent<HullInstanceComponent>().HullInstance.transform.up * 1.1f);
                    this.lastWorldPosition = originWithOffset;
                    Vector2 point = this.GetPoint(originWithOffset);
                    if (this.state != state)
                    {
                        this.animator.Play(state.ToString());
                        this.state = state;
                        this.timer.Break();
                    }
                    this.transform.localPosition = (Vector3) point;
                }
            }

            public void SetTeammate(TargetData targetData)
            {
                this.SetTarget(targetData, State.HighlightTeammate);
            }

            public UnityEngine.GameObject GameObject
            {
                get => 
                    this.gameObject;
                private set
                {
                    this.gameObject = value;
                    this.animator = value.GetComponent<Animator>();
                    this.transform = value.transform;
                }
            }

            private enum State
            {
                Hiden,
                HighlightEnemy,
                HighlightTeammate
            }
        }

        private class ReticleTimer : MonoBehaviour
        {
            private Action action;
            private Action end;
            private float time;

            public void Break()
            {
                this.time = 0f;
                this.end = null;
            }

            public void SetAction(Action action, float time, Action end)
            {
                this.action = action;
                this.end = end;
                this.time = Time.realtimeSinceStartup + time;
            }

            private void Update()
            {
                if (Time.realtimeSinceStartup < this.time)
                {
                    this.action();
                }
                else if (this.end != null)
                {
                    this.end();
                }
            }
        }
    }
}

