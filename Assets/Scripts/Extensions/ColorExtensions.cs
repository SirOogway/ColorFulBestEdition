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

        public static string ToStringRGB(this Color hexadecimalColor)
        {
            //Beacuse for the Color class the RGB color values are porcentages of themselves
            int conversionFactRGB = 255;
            return $"{hexadecimalColor.r * conversionFactRGB}, {hexadecimalColor.g * conversionFactRGB}, {hexadecimalColor.b * conversionFactRGB}";
        }
    }

}
