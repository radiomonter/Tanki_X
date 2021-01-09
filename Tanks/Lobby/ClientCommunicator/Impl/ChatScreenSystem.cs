namespace Tanks.Lobby.ClientCommunicator.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;

    public class ChatScreenSystem : ECSSystem
    {
        [OnEventFire]
        public void AddMainScreen(NodeAddedEvent e, SingleNode<ChatDialogComponent> chatDialog, SingleNode<MainScreenComponent> mainScreen)
        {
            mainScreen.component.AddListener(chatDialog.component);
        }

        [OnEventFire]
        public void LogTutorialStep(NodeAddedEvent e, SingleNode<TutorialScreenComponent> tutorial, SingleNode<ChatDialogComponent> dialog)
        {
            dialog.component.Hide();
        }

        [OnEventComplete]
        public void LogTutorialStep(NodeRemoveEvent e, SingleNode<TutorialScreenComponent> tutorial, [JoinAll] SingleNode<ChatDialogComponent> dialog)
        {
            dialog.component.Show();
        }

        [OnEventFire]
        public void MoveScreenToCanvas(NodeAddedEvent e, SingleNode<ChatScreenComponent> chatScreen, SingleNode<ScreensLayerComponent> layerNode)
        {
            GameObject gameObject = chatScreen.component.transform.parent.gameObject;
            chatScreen.component.transform.SetParent(layerNode.component.screens60Layer, false);
            Object.Destroy(gameObject);
        }

        [OnEventFire]
        public void OnMainScreen(NodeAddedEvent e, SingleNode<ChatScreenComponent> chatScreen, SingleNode<MainScreenComponent> main)
        {
            chatScreen.component.BuildDialog();
        }

        [OnEventFire]
        public void RemoveMainScreen(NodeRemoveEvent e, SingleNode<MainScreenComponent> mainScreen, [JoinAll] SingleNode<ChatDialogComponent> chatDialog)
        {
            chatDialog.component.Hide();
        }
    }
}

