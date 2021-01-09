namespace Tanks.Lobby.ClientGarage.Impl.Tutorial
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using tanks.modules.lobby.ClientGarage.Scripts.API.UI.Items;
    using UnityEngine;
    using UnityEngine.Events;

    public class ModulesTutorialUtil
    {
        public static float Z_OFFSET = -7f;
        public static bool TUTORIAL_MODE = false;
        private static readonly List<GameObject> movedObjects = new List<GameObject>();
        private static NewModulesScreenUIComponent modulesScreen;
        [CompilerGenerated]
        private static UnityAction <>f__mg$cache0;
        [CompilerGenerated]
        private static UnityAction <>f__mg$cache1;
        [CompilerGenerated]
        private static UnityAction <>f__mg$cache2;
        [CompilerGenerated]
        private static UnityAction <>f__mg$cache3;

        public static ModuleItem GetModuleItem(TutorialData tutorialData)
        {
            ModuleItem item3;
            long itemMarketId = tutorialData.TutorialStep.GetComponent<TutorialSelectItemDataComponent>().itemMarketId;
            using (IEnumerator<ModuleItem> enumerator = GarageItemsRegistry.Modules.GetEnumerator())
            {
                while (true)
                {
                    if (enumerator.MoveNext())
                    {
                        ModuleItem current = enumerator.Current;
                        long id = current.MarketItem.Id;
                        if (!id.Equals(itemMarketId))
                        {
                            continue;
                        }
                        item3 = current;
                    }
                    else
                    {
                        return null;
                    }
                    break;
                }
            }
            return item3;
        }

        public static void LockInteractable(NewModulesScreenUIComponent modulesScreen)
        {
            modulesScreen.turretCollectionView.GetComponent<Animator>().enabled = false;
            modulesScreen.turretCollectionView.GetComponent<CanvasGroup>().alpha = 1f;
            modulesScreen.turretCollectionView.slotContainer.blocksRaycasts = false;
            modulesScreen.hullCollectionView.GetComponent<CanvasGroup>().blocksRaycasts = false;
            modulesScreen.GetComponent<Animator>().enabled = false;
            modulesScreen.collectionView.GetComponent<CanvasGroup>().blocksRaycasts = false;
            modulesScreen.backButton.interactable = false;
            modulesScreen.selectedModuleView.GetComponent<CanvasGroup>().blocksRaycasts = false;
            ModulesTutorialUtil.modulesScreen = modulesScreen;
            if (<>f__mg$cache2 == null)
            {
                <>f__mg$cache2 = new UnityAction(ModulesTutorialUtil.OnTutorialSkipButton_Unlock);
            }
            TutorialCanvas.Instance.SkipTutorialButton.GetComponent<Button>().onClick.AddListener(<>f__mg$cache2);
        }

        private static void OnTutorialSkipButton_Unlock()
        {
            UnlockInteractable(modulesScreen);
        }

        private static void OnTutorialSkipButtonComponent_ResetOffset()
        {
            ResetOffset();
        }

        public static void ResetOffset()
        {
            foreach (GameObject obj2 in movedObjects)
            {
                SetZOffset(obj2, 0f);
            }
            movedObjects.Clear();
            SetZOffset(TutorialCanvas.Instance.overlay, 0f);
            SetZOffset(TutorialCanvas.Instance.SkipTutorialButton, 0f);
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new UnityAction(ModulesTutorialUtil.OnTutorialSkipButtonComponent_ResetOffset);
            }
            TutorialCanvas.Instance.SkipTutorialButton.GetComponent<Button>().onClick.RemoveListener(<>f__mg$cache1);
        }

        public static void SetOffset(List<GameObject> objects)
        {
            foreach (GameObject obj2 in objects)
            {
                SetZOffset(obj2, Z_OFFSET);
            }
            SetZOffset(TutorialCanvas.Instance.overlay, Z_OFFSET + 5f);
            SetZOffset(TutorialCanvas.Instance.SkipTutorialButton, Z_OFFSET);
            movedObjects.AddRange(objects);
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new UnityAction(ModulesTutorialUtil.OnTutorialSkipButtonComponent_ResetOffset);
            }
            TutorialCanvas.Instance.SkipTutorialButton.GetComponent<Button>().onClick.AddListener(<>f__mg$cache0);
        }

        private static void SetZOffset(GameObject gameObject, float zOffset)
        {
            RectTransform component = gameObject.GetComponent<RectTransform>();
            Vector3 vector = component.anchoredPosition3D;
            vector.z = zOffset;
            component.anchoredPosition3D = vector;
        }

        public static void UnlockInteractable(NewModulesScreenUIComponent modulesScreen)
        {
            modulesScreen.turretCollectionView.GetComponent<Animator>().enabled = true;
            modulesScreen.turretCollectionView.slotContainer.blocksRaycasts = true;
            modulesScreen.collectionView.GetComponent<CanvasGroup>().blocksRaycasts = true;
            modulesScreen.GetComponent<Animator>().enabled = true;
            modulesScreen.backButton.interactable = true;
            modulesScreen.selectedModuleView.GetComponent<CanvasGroup>().blocksRaycasts = true;
            modulesScreen.hullCollectionView.GetComponent<CanvasGroup>().blocksRaycasts = true;
            if (<>f__mg$cache3 == null)
            {
                <>f__mg$cache3 = new UnityAction(ModulesTutorialUtil.OnTutorialSkipButton_Unlock);
            }
            TutorialCanvas.Instance.SkipTutorialButton.GetComponent<Button>().onClick.RemoveListener(<>f__mg$cache3);
        }

        [Inject]
        public static Tanks.Lobby.ClientGarage.API.GarageItemsRegistry GarageItemsRegistry { get; set; }
    }
}

