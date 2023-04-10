using UnityEngine;

namespace ColorExtensions
{
    public static class ColorExtensions
    {
        public static string ToStringHSV(this Color hexadecimalColor)
        {
            int conversionFactH, conversionFactSV;
            int roundedHue, roundedSaturation, roundedValue;

            conversionFactH = 360;
            conversionFactSV = 100;

            Color.RGBToHSV(hexadecimalColor, out float H, out float S, out float V);

            roundedHue = Mathf.RoundToInt(H * conversionFactH);
            roundedSaturation = Mathf.RoundToInt(S * conversionFactSV);
            roundedValue = Mathf.RoundToInt(V * conversionFactSV);

            return $"{roundedHue}, {roundedSaturation}, {roundedValue}";
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
