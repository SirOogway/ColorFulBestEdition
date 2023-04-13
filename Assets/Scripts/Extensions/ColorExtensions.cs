using UnityEngine;

namespace ColorExtensions
{
    public static class ColorExtensions
    {
        public static string ToStringHSV(this Color hexadecimalColor)
        {
            int conversionFactH, conversionFactSV;
            int realHue, realSaturation, realValue;

            conversionFactH = 360;
            conversionFactSV = 100;

            Color.RGBToHSV(hexadecimalColor, out float H, out float S, out float V);

            realHue = Mathf.RoundToInt(H * conversionFactH);
            realSaturation = Mathf.RoundToInt(S * conversionFactSV);
            realValue = Mathf.RoundToInt(V * conversionFactSV);

            return $"{realHue}, {realSaturation}, {realValue}";
        }

        public static string ToStringRGB(this Color rgb)
        {
            //Beacuse for the Color class the RGB color values are porcentages of themselves
            int conversionFactRGB = 255;

            int r, g, b;
            r = Mathf.RoundToInt(rgb.r * conversionFactRGB);
            g = Mathf.RoundToInt(rgb.g * conversionFactRGB);
            b = Mathf.RoundToInt(rgb.b * conversionFactRGB);

            return $"{r}, {g}, {b}";
        }
    }

}
