using UnityEngine;

namespace ColorExtensions
{
    public static class ColorExtensions
    {
        public static string ToStringHSV(this Color color) //Color is on format rgb but in a range of 0-1 
        {
            int conversionFactH, conversionFactSV;
            int realHue, realSaturation, realValue;

            conversionFactH = 360;
            conversionFactSV = 100;

            Color.RGBToHSV(color, out float H, out float S, out float V);

            realHue = Mathf.RoundToInt(H * conversionFactH); 
            realSaturation = Mathf.RoundToInt(S * conversionFactSV);
            realValue = Mathf.RoundToInt(V * conversionFactSV);

            return $"{realHue}, {realSaturation}, {realValue}";
        }

        public static string ToStringRGB(this Color color)
        {
            //Beacuse for the Color class the RGB color values are porcentages of themselves
            int conversionFactRGB = 255;

            int r, g, b;
            r = Mathf.RoundToInt(color.r * conversionFactRGB);
            g = Mathf.RoundToInt(color.g * conversionFactRGB);
            b = Mathf.RoundToInt(color.b * conversionFactRGB);

            return $"{r}, {g}, {b}";
        }
    }

}
