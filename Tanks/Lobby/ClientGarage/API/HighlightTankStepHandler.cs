namespace Tanks.Lobby.ClientGarage.API
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class HighlightTankStepHandler : TutorialStepHandler
    {
        [SerializeField]
        private RectTransform popupPositionRect;
        [SerializeField]
        private float cameraOffset;
        [SerializeField]
        private GameObject outlinePrefab;
        [SerializeField]
        private bool useOverlay;

        public override void RunStep(TutorialData data)
        {
            base.RunStep(data);
            data.Type = TutorialType.HighlightTankPart;
            data.PopupPositionRect = this.popupPositionRect;
            data.CameraOffset = this.cameraOffset;
            data.OutlinePrefab = this.outlinePrefab;
            data.ContinueOnClick = true;
            TutorialCanvas.Instance.Show(data, this.useOverlay, null, null);
        }
    }
}

