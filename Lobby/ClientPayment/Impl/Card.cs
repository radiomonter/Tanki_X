namespace Lobby.ClientPayment.Impl
{
    using System;
    using UnityEngine;

    public class Card
    {
        public string number;
        public string holderName;
        public string expiryMonth;
        public string expiryYear;
        public string cvc;
        public string generationtime;

        public override string ToString()
        {
            this.generationtime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
            return JsonUtility.ToJson(this);
        }
    }
}

