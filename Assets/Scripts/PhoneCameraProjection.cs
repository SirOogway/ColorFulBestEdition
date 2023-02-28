using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PhoneCameraProjection : MonoBehaviour
{
    WebCamTexture cam_texture;
    public RawImage backGro;

    public RawImage setColor;
    public TMP_Text texto;

    string strHexColor, strRGBColor, strHSVColor;

    Color pixelColor;

    Resolution[] camResolutions;
    //camibar este ratio a su lugar 

    // Start is called before the first frame update
    void Start()
    {
        WebCamDevice[] cam_devices = WebCamTexture.devices;
        cam_texture = new WebCamTexture(cam_devices[0].name);// aca va lo de requested frames

        /* if(SystemInfo.deviceType == DeviceType.Handheld)
             camResolutions = cam_devices[0].availableResolutions;
        */

        ManagerAntiRotation();
        ManagerBackgroundRatio();

        backGro.texture = cam_texture;

        if (cam_texture != null)
            cam_texture.Play();
    }

    // Update is called once per frame
    void Update()
    {
        pixelColor = cam_texture.GetPixel(cam_texture.width / 2, cam_texture.height / 2);

        strHexColor = ColorUtility.ToHtmlStringRGB(pixelColor);
        strRGBColor = toStringRGB(pixelColor);
        strHSVColor = toStringHSV(pixelColor);

        texto.text =
            "Hex: " + strHexColor +
            "<br>RGB: " + strRGBColor +
            "<br>HSV: " + strHSVColor +
            "<br>Screen h w: " + (Screen.height) + " " + (Screen.width) +
            "<br>CamTexture h w: " + cam_texture.width + " " + cam_texture.height/* +
            "<br>Ratio: " + ratio*/;
        Debug.Log(
            "Hex: " + strHexColor +
            " RGB: " + strRGBColor +
            " HSV: " + strHSVColor);

        //setColor.GetComponent<Renderer>().material.color = pixelColor;
        setColor.color = pixelColor;
    }

    void ManagerAntiRotation()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            float antiRotate = -90f;
            Quaternion quatRotation = new Quaternion();
            quatRotation.eulerAngles = new Vector3(0f, 0f, antiRotate);
            backGro.transform.rotation = quatRotation;
        }
    }

    void ManagerBackgroundRatio()
    {
        float ratio;
        //ratio = (float)Screen.height / (float)Screen.width;
        ratio = 1.3333f; // Relacion de aspecto de la resolución de la camara
        float height = ratio;
        float width = 1f;

        if (SystemInfo.deviceType == DeviceType.Handheld)
            backGro.transform.localScale = new Vector2(height, width);
        else //For PC
            backGro.transform.localScale = new Vector2(width, height);

        AspectRatioFitter myAspectRatioFitter = backGro.GetComponent<AspectRatioFitter>();
        myAspectRatioFitter.enabled = true;
        myAspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
        //myAspectRatioFitter.aspectRatio = ratio;
    }







    //-------------- Extension functions
    string toStringHSV(Color color) // Convert to extension function
    {
        int conversionFactH, conversionFactSV;
        int roundedH, roundedS, roundedV;

        conversionFactH = 359;
        conversionFactSV = 100;

        Color.RGBToHSV(color, out float H, out float S, out float V);

        roundedH = Mathf.RoundToInt(H * conversionFactH);
        roundedS = Mathf.RoundToInt(S * conversionFactSV);
        roundedV = Mathf.RoundToInt(V * conversionFactSV);

        return $"{roundedH}, {roundedS}, {roundedV}";
    }

    string toStringRGB(Color color)// convert to extension function
    {
        //Beacuse for the Color class the RGB color values are porcentages of themselves
        int conversionFactRGB = 255;
        return $"{color.r * conversionFactRGB}, {color.g * conversionFactRGB}, {color.b * conversionFactRGB}";
    }
}
