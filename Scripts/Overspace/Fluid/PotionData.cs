using UnityEngine;

namespace Overspace.Fluid
{
    [CreateAssetMenu(fileName = "Fluid Data", menuName = "Overspace/Potion Data")]
    public class PotionData : ScriptableObject
    {
        public Color outer;
        public Color inner;
        public Texture2D main;
        public Texture2D noiseOne;
        public Texture2D noiseTwo;
    }
}