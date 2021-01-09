namespace Tanks.Lobby.ClientUserProfile.API
{
    using System;

    public class WaitingForInviteToLobbyAnswerUIComponent : WaitingAnswerUIComponent
    {
        public bool AlreadyInLobby
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

