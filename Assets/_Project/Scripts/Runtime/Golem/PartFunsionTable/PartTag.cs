namespace DarkLordGame
{
    public enum PartTag
    {
        North,
        West,
        East,
        Sounth,
        Forward,
        Backward,
    }

    [System.Serializable]
    public class PartTagValue
    {
        public PartTag tagType;
        public float value;
    }
}
