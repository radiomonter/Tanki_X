namespace Tanks.Lobby.ClientControls.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;

    public class CaptchaSystem : ECSSystem
    {
        [OnEventFire]
        public void ParseCaptcha(NodeAddedEvent e, CaptchaNode node)
        {
            node.captcha.Animator.SetTrigger("Normal");
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(node.captchaBytes.bytes);
            Vector2 pivot = new Vector2();
            Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, (float) texture.width, (float) texture.height), pivot);
            node.captcha.CaptchaSprite = sprite;
            node.Entity.RemoveComponent<CaptchaBytesComponent>();
        }

        [OnEventFire]
        public void SendUpdateCaptcha(ButtonClickEvent e, SingleNode<CaptchaComponent> node)
        {
            base.ScheduleEvent<UpdateCaptchaEvent>(node);
        }

        [OnEventFire]
        public void SendUpdateEvent(NodeAddedEvent e, SingleNode<CaptchaComponent> node)
        {
            base.ScheduleEvent<UpdateCaptchaEvent>(node);
        }

        [OnEventFire]
        public void TransitionToWaitState(ShowCaptchaWaitAnimationEvent e, SingleNode<CaptchaComponent> node)
        {
            node.component.Animator.SetTrigger("Wait");
        }

        public class CaptchaNode : Node
        {
            public CaptchaComponent captcha;
            public CaptchaBytesComponent captchaBytes;
        }
    }
}

