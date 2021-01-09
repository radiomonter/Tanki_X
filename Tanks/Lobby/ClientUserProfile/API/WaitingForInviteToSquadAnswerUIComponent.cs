namespace Tanks.Lobby.ClientUserProfile.API
{
    using System;

    public class WaitingForInviteToSquadAnswerUIComponent : WaitingAnswerUIComponent
    {
        public bool AlreadyInSquad
        {
            set
            {
                base.Waiting = false;
                if (value)
                {
                    base.inviteButton.SetActive(false);
                }
                base.alreadyInLabel.SetActive(value);
            }
        }
    }
}

