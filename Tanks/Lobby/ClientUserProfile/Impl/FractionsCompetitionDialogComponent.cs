namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using System;
    using Tanks.Lobby.ClientGarage.Impl;

    public class FractionsCompetitionDialogComponent : ConfirmDialogComponent
    {
        public FractionSelectWindowComponent FractionSelectWindow;
        public CurrentCompetitionWindowComponent CurrentCompetitionWindow;
        public CompetitionAwardWindowComponent CompetitionAwardWindow;
        public FractionLearnMoreWindowComponent LearnMoreWindow;

        public override void Hide()
        {
            this.FractionSelectWindow.gameObject.SetActive(false);
            this.CurrentCompetitionWindow.gameObject.SetActive(false);
            this.CompetitionAwardWindow.gameObject.SetActive(false);
            this.LearnMoreWindow.gameObject.SetActive(false);
            base.Hide();
        }
    }
}

