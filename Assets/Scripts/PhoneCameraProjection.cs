using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PhoneCameraProjection : MonoBehaviour
{
    WebCamTexture cam_texture;
    public RawImage backGro;

    public GameObject[] targetItems;

    public RawImage setColor;
    public TMP_Text texto;

    string strHexColor, strRGBColor, strHSVColor;

    Color pixelColor;

    Resolution[] camResolutions;
    //camibar este ratio a su lugar 

    float scaleY;
    float scaleX;
    float heigth;
    float alturita;
    float anchito;

    Vector2 back;

    // Start is called before the first frame update
    void Start()
    {
        WebCamDevice[] cam_devices = WebCamTexture.devices;
        cam_texture = new WebCamTexture(cam_devices[0].name);// aca va lo de requested frames

        /* if(SystemInfo.deviceType == DeviceType.Handheld)
             camResolutions = cam_devices[0].availableResolutions;
        */
        
        ManagerAntiRotation();
        //ManagerBackgroundRatio();


        //scaleY = backGro.rectTransform.localScale.x;
        //scaleX = backGro.rectTransform.localScale.y;


        //backGro.rectTransform.position = new Vector2(0,1.25f);

        /*heigth = backGro.rectTransform.rect.y;
        var posY = backGro.rectTransform.rect;
        posY.x = 400;*/ //400/320 = 1.25


        //backGro.rectTransform.localScale = new Vector2(1.333f, 1);
        //backGro.rectTransform.anchoredPosition = new Vector2(0, 2f);

        //no funciona: Localposition
        //funciona pero con las medidas de unity position

        /*AspectRatioFitter aRFComponent = backGro.GetComponent<AspectRatioFitter>();
        aRFComponent.enabled = !aRFComponent.enabled;*/

        //ManagerBackgroundPosition();

        backGro.texture = cam_texture;

        if (cam_texture != null)
            cam_texture.Play();

        backGro.SetNativeSize();

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            backGro.rectTransform.sizeDelta = new Vector2(1066.6664f, 800);
            backGro.rectTransform.position = new Vector2(0, 1.24994f);
        }
        else
        {
            //backGro.rectTransform.position = new Vector2(-1.125f, 2.7499992f);
            backGro.rectTransform.sizeDelta = new Vector2(800, 1066.6664f);
            backGro.rectTransform.position = new Vector2(0, 1.24994f);
            // panel manual 177.78 y por codigo 177.7693, estoy perdiendo 0.02 UNY
        }
    }

    // Update is called once per frame
    void Update()
    {

        //backGro.rectTransform.position = new Vector2(-1.125f, 1.25f);
        //backGro.rectTransform.position = new Vector2(0, 1); //PARA PRUEBAS EL DE ARRIBA ES EL GOOD
        

        anchito = backGro.rectTransform.rect.width;  
        alturita = backGro.rectTransform.rect.height;

        var XXXX = backGro.rectTransform.sizeDelta.x;//funciona y me da el mismo valor que anchito
        //var ALT =backGro.rectTransform.sizeDelta = new Vector2(900,4666); //FUNCIONA PARA SETEAR ALTURA

        pixelColor = cam_texture.GetPixel(cam_texture.width / 2, cam_texture.height / 2);

        strHexColor = ColorUtility.ToHtmlStringRGB(pixelColor);
        strRGBColor = toStringRGB(pixelColor);
        strHSVColor = toStringHSV(pixelColor);

        
       // backGro.rectTransform.localScale = new Vector2(480*1.111f, 640*1.111f);

        texto.text =
            "Hex: " + strHexColor +
            "<br>RGB: " + strRGBColor +
            "<br>HSV: " + strHSVColor +
            "<br>Screen h w: " + (Screen.height) + " " + (Screen.width) +
            "<br>CamTexture h w: " + cam_texture.width + " " + cam_texture.height+
            "<br>rectTransAltura: " + alturita +
            "<br>rectTransAncho: " + /*anchito*/ anchito;
        Debug.Log(
            "Hex: " + strHexColor +
            " RGB: " + strRGBColor +
            " HSV: " + strHSVColor);

        //setColor.GetComponent<Renderer>().material.color = pixelColor;
        //setColor.color = pixelColor;

        // Set Color of Middle Pixel
        foreach (GameObject item in targetItems)
        {
            if (item.name == "Color")
            {
                item.GetComponent<RawImage>().color = pixelColor;
                continue;
            }
            item.GetComponent<Renderer>().material.color = pixelColor;
        }

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

        AspectRatioFitter aRFComponent = backGro.GetComponent<AspectRatioFitter>();
        aRFComponent.enabled = !aRFComponent.enabled;
        aRFComponent.aspectMode = AspectRatioFitter.AspectMode.FitInParent;
       

        //aRFComponent.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
        //myAspectRatioFitter.aspectRatio = ratio;
    }

    void ManagerBackgroundPosition()
    {
        /*AspectRatioFitter aRFComponent = backGro.GetComponent<AspectRatioFitter>();
        aRFComponent.enabled = !aRFComponent.enabled;*/
        
       // RectTransform rTComponent = backGro.rectTransform;
        /*rTComponent.anchorMin = new Vector2(0.5f, 1);
        rTComponent.anchorMax = new Vector2(0.5f, 1);
        rTComponent.pivot = new Vector2(0.5f, 1); */
        
        /*rTComponent.anchorMin = new Vector2(1, 0.5f);
        rTComponent.anchorMax = new Vector2(1, 0.5f);*/
       // rTComponent.pivot = new Vector2(1, 1);
        //rTComponent.position = new Vector2(1, 1);


        //backGro.rectTransform.anchoredPosition = new Vector2(0f,0f);
        // aRFComponent.enabled = !aRFComponent.enabled;
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
