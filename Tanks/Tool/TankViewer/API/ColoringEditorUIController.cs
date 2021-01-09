namespace Tanks.Tool.TankViewer.API
{
    using System;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class ColoringEditorUIController : MonoBehaviour
    {
        public ColoringEditorUIView view;
        public ColoringCreationLogic logic;
        public TextureDataSource textureDataSource;
        public ResultsDataSource resultsDataSource;

        public void Awake()
        {
            this.view.SwitchToViewer();
            this.textureDataSource.onStartUpdatingAction += new Action(this.view.creatorView.Disable);
            this.textureDataSource.onStartUpdatingAction += new Action(this.logic.CleanTextures);
            this.textureDataSource.onCompleteUpdatingAction += new Action(this.EnableCreatorView);
            this.textureDataSource.UpdateData();
            this.view.viewerView.resultsDropdownView.dropdown.interactable = false;
            if (this.resultsDataSource.IsReady)
            {
                this.UpdateResultsDropdown();
            }
            this.resultsDataSource.onChange += new Action(this.UpdateResultsDropdown);
        }

        public void EnableCreatorView()
        {
            this.view.creatorView.Enable();
        }

        public void OnCancelClick()
        {
            this.view.SwitchToViewer();
        }

        public void OnCreateColoringButtonClick()
        {
            ColoringComponent coloringComponent = this.logic.CreateNewColoring();
            this.view.SwitchToEditor(coloringComponent);
        }

        public void OnResultsDropdownChanged()
        {
            int num = this.view.viewerView.resultsDropdownView.dropdown.value - 1;
            if (num >= 0)
            {
                ColoringComponent coloringComponent = this.resultsDataSource.GetColoringComponents()[num];
                this.logic.UpdateColoring(coloringComponent);
            }
        }

        public void OnSaveClick()
        {
            this.logic.Save();
            this.view.SwitchToViewer();
            this.view.viewerView.resultsDropdownView.SelectLastOption();
        }

        public void OnSomeParamChange()
        {
            if (this.view.creatorView.gameObject.activeSelf)
            {
                CreatorView creatorView = this.view.creatorView;
                this.logic.UpdateColoring(creatorView.colorView.GetColor(), creatorView.textureView.GetSelectedTexture(), creatorView.textureView.GetAlphaMode(), creatorView.normalMapView.GetSelectedNormalMap(), creatorView.normalMapView.GetNormalScale(), creatorView.metallicView.GetFloat(), creatorView.smoothnessView.GetOverrideSmoothness(), creatorView.smoothnessView.GetSmoothnessStrenght(), creatorView.intensityThresholdView.GetUseIntensityThreshold(), creatorView.intensityThresholdView.GetIntensityThreshold());
            }
        }

        public void OnUpdateTexturesButtonClick()
        {
            this.textureDataSource.UpdateData();
        }

        private void UpdateResultsDropdown()
        {
            this.view.viewerView.resultsDropdownView.dropdown.interactable = true;
        }
    }
}

