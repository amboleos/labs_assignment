using System.Collections.Generic;
using UnityEngine;

namespace Overspace.Fluid
{
    [CreateAssetMenu(fileName = "Fluid Data", menuName = "Overspace/Fluid Data")]
    public class FluidData : ScriptableObject
    {
        public string fluidName;
        public Sprite fluidIcon;
        public Color fluidColor = Color.white;
    }
}