using UnityEngine;
namespace DarkLordGame
{
    public class IDGenerator : PropertyAttribute
    {
        public readonly string saveName;
        public readonly int maxNumbers;
        /// <summary>
        /// Initializes a new instance of the <see cref="DarkLordGame.IDGeneratorAttributes"/> class.
        /// </summary>
        /// <param name="sName">S name.</param>
        /// <param name="MaxNumbers">Max Numbers</param>
        public IDGenerator(string _saveName, int _maxNumbers)
        {
            saveName = _saveName;
            maxNumbers = _maxNumbers;
        }
    }
}