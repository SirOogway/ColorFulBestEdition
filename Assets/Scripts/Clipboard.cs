using UnityEngine;
using ColorExtensions;

public class Clipboard : MonoBehaviour
{
    string clipboardText;

    [SerializeField]
    PhoneCameraProjection phoneCameraProjection;

    Color pixelColor;

    public void Copy()
    {
        clipboardText = "";
        pixelColor = phoneCameraProjection.GetHexColor();

        //conversiones de color
        string strHexColor, strRGBColor, strHSVColor;
        
        strHexColor = ColorUtility.ToHtmlStringRGB(pixelColor);
        strRGBColor = pixelColor.ToStringRGB();
        strHSVColor = pixelColor.ToStringHSV();

        //When a model is active it is possible copy into clipboard
        if (SwitchHandler.GetStateHEXModel())
          clipboardText = ($"HEX: {strHexColor}");
        if (SwitchHandler.GetStateRGBModel())
          clipboardText += ($"\nRGB: {strRGBColor}");
        if (SwitchHandler.GetStateHSVModel())
          clipboardText += ($"\nHSV: {strHSVColor}");

        //Copying into clipboard
        TextEditor textEditor = new TextEditor();
        textEditor.text = clipboardText;
        textEditor.SelectAll();
        textEditor.Copy();

        Debug.Log($"Copied to clipboard \n" +
            $"{textEditor.text}");
    }

}
