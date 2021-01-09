namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using TMPro;
    using UnityEngine;

    public class FractionScoresContainerComponent : BehaviourComponent
    {
        [SerializeField]
        private FractionScoresUiBehaviour _fractionScoresPrefab;
        [SerializeField]
        private GameObject _container;
        [SerializeField]
        private TextMeshProUGUI _cryFundText;
        private readonly Dictionary<long, FractionScoresUiBehaviour> _fractions = new Dictionary<long, FractionScoresUiBehaviour>();
        [CompilerGenerated]
        private static Func<FractionScoresUiBehaviour, long> <>f__am$cache0;

        public void UpdateScores(long fractionId, FractionInfoComponent info, long scores)
        {
            if (this._fractions.ContainsKey(fractionId))
            {
                this._fractions[fractionId].FractionScores = scores;
            }
            else
            {
                FractionScoresUiBehaviour behaviour2 = Instantiate<FractionScoresUiBehaviour>(this._fractionScoresPrefab, this._container.transform);
                behaviour2.FractionName = info.FractionName;
                behaviour2.FractionLogoUid = info.FractionLogoImageUid;
                Color defaultColor = new Color();
                behaviour2.FractionColor = FractionsCompetitionUiSystem.GetColorFromHex(info.FractionColor, defaultColor);
                behaviour2.FractionScores = scores;
                this._fractions.Add(fractionId, behaviour2);
            }
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = fraction => fraction.FractionScores;
            }
            List<FractionScoresUiBehaviour> list = this._fractions.Values.OrderByDescending<FractionScoresUiBehaviour, long>(<>f__am$cache0).ToList<FractionScoresUiBehaviour>();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].transform.SetSiblingIndex(i);
            }
        }

        public long WinnerId
        {
            set
            {
                foreach (KeyValuePair<long, FractionScoresUiBehaviour> pair in this._fractions)
                {
                    pair.Value.IsWinner = pair.Key == value;
                }
            }
        }

        public long TotalCryFund
        {
            set => 
                this._cryFundText.text = $"{value:0}";
        }
    }
}

