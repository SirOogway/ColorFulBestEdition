using UnityEngine;

namespace ColorExtensions
{
    public static class ColorExtensions
    {
        public static string ToStringHSV(this Color hexadecimalColor)
        {
            int conversionFactH, conversionFactSV;
            int roundedH, roundedS, roundedV;

            conversionFactH = 360;
            conversionFactSV = 100;

            Color.RGBToHSV(hexadecimalColor, out float H, out float S, out float V);

            roundedH = Mathf.RoundToInt(H * conversionFactH);
            roundedS = Mathf.RoundToInt(S * conversionFactSV);
            roundedV = Mathf.RoundToInt(V * conversionFactSV);

            return $"{roundedH}, {roundedS}, {roundedV}";
        }

        public static string ToStringRGB(this Color hexadecimalColor)
        {
            //Beacuse for the Color class the RGB color values are porcentages of themselves
            int conversionFactRGB = 255;
            return $"{hexadecimalColor.r * conversionFactRGB}, {hexadecimalColor.g * conversionFactRGB}, {hexadecimalColor.b * conversionFactRGB}";
        }
    }

}
