using UnityEngine;
using ColorExtensions;

public class Clipboard : MonoBehaviour
{
    [SerializeField] PhoneCameraProjection phoneCameraProjection;

    public void Copy()
    {
        Color pixelColor;
        pixelColor = phoneCameraProjection.GetHexColor();

        //conversiones de color
        string clipboardText, strHexColor, strRGBColor, strHSVColor;

        clipboardText = "";

        strHexColor = ColorUtility.ToHtmlStringRGB(pixelColor);
        strRGBColor = pixelColor.ToStringRGB();
        strHSVColor = pixelColor.ToStringHSV();

        //When a model is active it is possible copy into clipboard
        if (SwitchHandler.GetStateHEXModel())
          clipboardText = ($"HEX: {strHexColor}\n");
        if (SwitchHandler.GetStateRGBModel())
          clipboardText += ($"RGB: {strRGBColor}\n");
        if (SwitchHandler.GetStateHSVModel())
          clipboardText += ($"HSV: {strHSVColor}");

        //Copying into clipboard
        TextEditor textEditor = new TextEditor();
        textEditor.text = clipboardText;
        textEditor.SelectAll();
        textEditor.Copy();

        Debug.Log($"Copied to clipboard \n" +
            $"{textEditor.text}");
    }
}
