namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientControls.API;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.UI;

    public class AvatarButton : MonoBehaviour
    {
        private const string equipedFrameName = "equiped";
        private const string selectedFrameName = "selected";
        [SerializeField]
        private Button button;
        [SerializeField]
        private ImageSkin icon;
        [SerializeField]
        private ImageListSkin frame;
        [SerializeField]
        private GameObject selectedFrame;
        [SerializeField]
        private GameObject equipedFrame;
        [SerializeField]
        private GameObject lockImage;
        public Action OnPress;
        public Func<int> GetIndex;
        public Action OnDoubleClick;
        private bool isUserItem;
        private float delta;
        private float time;
        [CompilerGenerated]
        private static Action <>f__am$cache0;
        [CompilerGenerated]
        private static Func<int> <>f__am$cache1;
        [CompilerGenerated]
        private static Action <>f__am$cache2;

        public AvatarButton()
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = new Action(AvatarButton.<OnPress>m__0);
            }
            this.OnPress = <>f__am$cache0;
            <>f__am$cache1 ??= new Func<int>(AvatarButton.<GetIndex>m__1);
            this.GetIndex = <>f__am$cache1;
            <>f__am$cache2 ??= new Action(AvatarButton.<OnDoubleClick>m__2);
            this.OnDoubleClick = <>f__am$cache2;
            this.delta = 0.2f;
        }

        [CompilerGenerated]
        private static int <GetIndex>m__1() => 
            0;

        [CompilerGenerated]
        private static void <OnDoubleClick>m__2()
        {
        }

        [CompilerGenerated]
        private static void <OnPress>m__0()
        {
        }

        private void Awake()
        {
            this.button.onClick.AddListener(new UnityAction(this.OnPressButton));
        }

        public void Init(string iconUid, string rarity, IAvatarStateChanger changer)
        {
            this.icon.SpriteUid = iconUid;
            this.frame.SelectSprite(rarity);
            changer.SetEquipped = new Action<bool>(this.SetEquipped);
            changer.SetSelected = new Action<bool>(this.SetSelected);
            changer.SetUnlocked = new Action<bool>(this.SetUnlocked);
            changer.OnBought = new Action(this.SetAsBought);
            this.lockImage.SetActive(false);
            Color white = Color.white;
            white.a = 0.1f;
            this.icon.GetComponent<Image>().color = white;
            this.frame.GetComponent<Image>().color = white;
        }

        private void OnPressButton()
        {
            this.OnPress();
            if ((Time.realtimeSinceStartup - this.time) >= this.delta)
            {
                this.time = Time.realtimeSinceStartup;
            }
            else
            {
                this.OnDoubleClick();
                this.time = 0f;
            }
        }

        public void SetAsBought()
        {
            this.isUserItem = true;
            Color white = Color.white;
            this.icon.GetComponent<Image>().color = white;
            this.frame.GetComponent<Image>().color = white;
        }

        public void SetEquipped(bool equipped)
        {
            this.equipedFrame.SetActive(equipped);
        }

        public void SetSelected(bool selected)
        {
            this.selectedFrame.SetActive(selected);
        }

        public void SetUnlocked(bool unlocked)
        {
            this.lockImage.SetActive(!unlocked);
        }
    }
}

