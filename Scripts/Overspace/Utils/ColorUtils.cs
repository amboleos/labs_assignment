using UnityEngine;

namespace Overspace.Utils
{
    public static class ColorUtils
    {
        public static Color[] GenerateShades(Color color)
        {
            Color[] colors = new Color[10];
            
            for (int i = 0; i < 10; i++)
            {
                float r = color.r * (1f - 0.1f * i);
                float g = color.g * (1f - 0.1f * i);
                float b = color.b * (1f - 0.1f * i);
                colors[i] = new Color(r, g, b);
            }

            return colors;
        }

        public static Color[] GenerateTints(Color color)
        {
            Color[] colors = new Color[10];
            
            for (int i = 0; i < 10; i++)
            {
                float r = color.r + (1f - color.r) * i * 0.1f;
                float g = color.g + (1f - color.g) * i * 0.1f;
                float b = color.b + (1f - color.b) * i * 0.1f;
                colors[i] = new Color(r, g, b);
            }

            return colors;
        }
        
        public static Color CombineColors(Color[] aColors)
        {
            Color result = new Color(0,0,0,0);
            foreach(Color c in aColors)
            {
                result += c;
            }
            result /= aColors.Length;
            return result;
        }
    }
}