namespace DarkLordGame
{
    public enum FusionTag
    {
        North,
        West,
        East,
        Sounth,
        Forward,
        Backward,
    }

    [System.Serializable]
    public class FusionTagValue
    {
        public FusionTag tagType;
        public float value;
    }
}
