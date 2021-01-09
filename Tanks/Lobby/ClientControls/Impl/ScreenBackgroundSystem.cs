namespace Tanks.Lobby.ClientControls.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Lobby.ClientControls.API;

    public class ScreenBackgroundSystem : ECSSystem
    {
        [OnEventFire]
        public void HideScreenBackground(HideScreenBackgroundEvent e, DarkenBackgroundNode background)
        {
            ScreenBackgroundAnimationComponent screenBackgroundAnimation = background.screenBackgroundAnimation;
            if (screenBackgroundAnimation.Animator.GetCurrentAnimatorStateInfo(screenBackgroundAnimation.LayerId).normalizedTime > 1f)
            {
                screenBackgroundAnimation.Animator.Play(screenBackgroundAnimation.State, screenBackgroundAnimation.LayerId, 1f);
            }
            screenBackgroundAnimation.Animator.SetFloat(screenBackgroundAnimation.SpeedMultiplicatorId, -1f);
        }

        [OnEventFire]
        public void ShowScreenBackground(ShowScreenBackgroundEvent e, DarkenBackgroundNode background)
        {
            ScreenBackgroundAnimationComponent screenBackgroundAnimation = background.screenBackgroundAnimation;
            if (screenBackgroundAnimation.Animator.GetCurrentAnimatorStateInfo(screenBackgroundAnimation.LayerId).normalizedTime < 0f)
            {
                screenBackgroundAnimation.Animator.Play(screenBackgroundAnimation.State, screenBackgroundAnimation.LayerId, 0f);
            }
            screenBackgroundAnimation.Animator.SetFloat(screenBackgroundAnimation.SpeedMultiplicatorId, 1f);
        }

        public class DarkenBackgroundNode : Node
        {
            public ScreenBackgroundAnimationComponent screenBackgroundAnimation;
        }
    }
}

