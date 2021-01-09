namespace Tanks.Lobby.ClientSettings.API
{
    using System;
    using UnityEngine;

    public class Quality
    {
        private readonly string name;
        private readonly int level;
        public static Quality fastest = new Quality("Fastest", 0);
        public static Quality ultra = new Quality("Ultra", 5);
        public static Quality high = new Quality("High", 3);
        public static Quality maximum = new Quality("Maximum", 4);
        public static Quality medium = new Quality("Medium", 2);
        public static Quality mininum = new Quality("Minimum", 1);
        private static readonly Quality[] qualities = new Quality[] { fastest, mininum, medium, high, maximum, ultra };

        public Quality(string name, int level)
        {
            this.name = name;
            this.level = level;
        }

        public static Quality GetQualityByName(string qualityName)
        {
            qualityName = qualityName.ToLower();
            for (int i = 0; i < qualities.Length; i++)
            {
                Quality quality = qualities[i];
                if (quality.Name.ToLower().Equals(qualityName))
                {
                    return quality;
                }
            }
            throw new ArgumentException("Quality with name " + qualityName + " was not found.");
        }

        public static void ValidateQualities()
        {
            int index = 0;
            while (index < QualitySettings.names.Length)
            {
                int num2 = 0;
                while (true)
                {
                    if (num2 >= qualities.Length)
                    {
                        index++;
                        break;
                    }
                    Quality quality = qualities[index];
                    if (!quality.Name.Equals(QualitySettings.names[index]) || (index != quality.Level))
                    {
                        throw new Exception($"There is no quality {quality.Name} with level {quality.Level}");
                    }
                    num2++;
                }
            }
        }

        public string Name =>
            this.name;

        public int Level =>
            this.level;

        public enum QualityLevel
        {
            Fastest,
            Minimum,
            Meduim,
            High,
            Maximum,
            Ultra
        }
    }
}

