using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ColorExtensions;

public class PhoneCameraProjection : MonoBehaviour
{
    WebCamDevice[] cam_devices;
    WebCamTexture cam_texture;

    [Tooltip("Background shows the image captured by the camera.")]
    public RawImage background;
    [Tooltip("The text that will contain the information about the color models.")]
    public TMP_Text texto;

    [HideInInspector]
    public Color pixelColor;

    string strHexColor, strRGBColor, strHSVColor;

    [Tooltip("Allows you to obtain the dimensions of the phone screen in units of unity.")]
    public Canvas canvas;
    float widthCanvas; 
    float heightCanvas;

    void Start()
    {
        widthCanvas = canvas.GetComponent<RectTransform>().rect.width;
        heightCanvas = canvas.GetComponent<RectTransform>().rect.height;

        cam_devices = WebCamTexture.devices;
        cam_texture = new WebCamTexture(cam_devices[0].name);// aca va lo de requested frames

        background.texture = cam_texture;

        if (cam_texture != null)
        {
            cam_texture.Play();
        }
        else
        {
            //set a bg and a color by default
        }

        ManagerAntiRotation();
        ManagerBackgroundSize();
        ManagerBackgroundPosition();
    }

    void Update()
    {
        pixelColor = cam_texture.GetPixel(cam_texture.width / 2, cam_texture.height / 2);

        strHexColor = ColorUtility.ToHtmlStringRGB(pixelColor);
        strRGBColor = pixelColor.ToStringRGB();
        strHSVColor = pixelColor.ToStringHSV();

        string txt;
        txt = "Hex: " + strHexColor +
            "<br>RGB: " + strRGBColor +
            "<br>HSV: " + strHSVColor;

        texto.text = txt;
        /*    
        Debug.Log(
            "Hex: " + strHexColor +
            " RGB: " + strRGBColor +
            " HSV: " + strHSVColor);
        */
    }

    void ManagerAntiRotation()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            float antiRotate = -90f;
            Quaternion quatRotation = new Quaternion();
            quatRotation.eulerAngles = new Vector3(0f, 0f, antiRotate);
            background.transform.rotation = quatRotation;
        }
    }

    /*void ManagerBackgroundRatio()
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

        AspectRatioFitter aRFComponent = backGro.GetComponent<AspectRatioFitter>();
        aRFComponent.enabled = !aRFComponent.enabled;
        aRFComponent.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
       

        //aRFComponent.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
        //myAspectRatioFitter.aspectRatio = ratio;
    }*/

    void ManagerBackgroundSize()
    {
        float heightBackground, widthBackground, ratio;

        widthBackground = widthCanvas;
        ratio = Ratio();
        heightBackground = ratio * widthBackground;

        if (SystemInfo.deviceType == DeviceType.Handheld)
            background.rectTransform.sizeDelta = new Vector2(heightBackground, widthBackground);
        else
            background.rectTransform.sizeDelta = new Vector2(widthBackground, heightBackground);
    }

    void ManagerBackgroundPosition()
    {
        float displacementPerUnitScale, heightBackground, deltaTop, displacementScale;

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            displacementPerUnitScale = heightCanvas / 10;
            heightBackground = background.rectTransform.rect.width;
            deltaTop = (heightCanvas - heightBackground) / 2;
            displacementScale = deltaTop / displacementPerUnitScale;

            background.rectTransform.position = new Vector2(0, displacementScale);
        }
        else
        {
            displacementPerUnitScale = heightCanvas / 10;
            heightBackground = background.rectTransform.rect.height;
            deltaTop = (heightCanvas - heightBackground) / 2;
            displacementScale = deltaTop / displacementPerUnitScale;

            background.rectTransform.position = new Vector2(0, displacementScale);
        }
    }

    float Ratio()
    {

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            Resolution cameraResolution;
            int frontCamera, mainResolution;

            frontCamera = 0;
            mainResolution = 0;
            cameraResolution = cam_devices[frontCamera].availableResolutions[mainResolution];

            float height, width, ratio;

            height = cameraResolution.width;
            width = cameraResolution.height;
            ratio = height / width;

            return ratio;
        }

        return (float)cam_texture.height / (float)cam_texture.width;
    }

}
