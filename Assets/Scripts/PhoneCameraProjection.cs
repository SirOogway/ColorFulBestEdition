using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ColorExtensions;

public class PhoneCameraProjection : MonoBehaviour
{
    WebCamDevice[] cam_devices; //vector para almacenar las camaras del celular
    WebCamTexture cam_texture; //lo que captura la camara

    [Tooltip("Allows you to obtain the dimensions of the phone screen in units of unity.")]
    public Canvas canvas;
    float widthCanvas;
    float heightCanvas;

    [Tooltip("Background shows the image captured by the camera.")] //mostrar mensajes en el inspector
    public RawImage background;

    [HideInInspector] public Color pixelColor;
    public Image colorImage;
    [Tooltip("The models array contains the information about the color models.")]
    public TMP_Text[] models;

    [Tooltip("The text that will contain the information about the color models.")]
    string strHexColor, strRGBColor, strHSVColor;
    
    void Start() //se ejecuta una sola vez al iniciar la aplicacion
    {
        widthCanvas = canvas.GetComponent<RectTransform>().rect.width;
        heightCanvas = canvas.GetComponent<RectTransform>().rect.height;

        cam_devices = WebCamTexture.devices; //obtengo los dispositivos de camara, si tiene 1 camara 2 camaras 3 camaras etc
        cam_texture = new WebCamTexture(cam_devices[0].name);//cam_devices[0].name <- nombre de donde sacaré la textura , la textura es la secuencia de imagenes (video que
                                                             //captura la camara)

        if (cam_texture != null)
        {
            cam_texture.Play(); //la camara empieza a capturar
        }
        else
        {
            //set a bg and a color by default
        }

        background.texture = cam_texture; //al background le asignamos la textura(la imagen de la camara) obtenida

        ManagerAntiRotation();
        ManagerBackgroundSize();
        ManagerBackgroundPosition();
    }

    void Update() //EXPLICARLES POR QUË ESTO ESTÄ EN UPDATE Y NO EN START
    {
        pixelColor = cam_texture.GetPixel(cam_texture.width / 2, cam_texture.height / 2);

        //conversiones de color
        strHexColor = ColorUtility.ToHtmlStringRGB(pixelColor);
        strRGBColor = pixelColor.ToStringRGB();
        strHSVColor = pixelColor.ToStringHSV();

        //asignamos los modos de color a los textos
        models[0].text = strHexColor;
        models[1].text = strRGBColor;
        models[2].text = strHSVColor;

        //asignamos el color a la imagen
        colorImage.color = pixelColor;

    }

    void ManagerAntiRotation()
    {
        //probar con tablet
        if (SystemInfo.deviceType == DeviceType.Handheld) //si es un dispositivo movil has esto
        {
            float antiRotate = -90f;
            Quaternion quatRotation = new Quaternion();
            quatRotation.eulerAngles = new Vector3(0f, 0f, antiRotate);// 0grados de rotacion, antirotate = -90 grados de rotacion, eulerAngles es una variable de quatRotation
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

        if (SystemInfo.deviceType == DeviceType.Handheld) //para celular
            background.rectTransform.sizeDelta = new Vector2(heightBackground, widthBackground);
        else //para pc
            background.rectTransform.sizeDelta = new Vector2(widthBackground, heightBackground);
    }

    void ManagerBackgroundPosition()
    {
        float displacementPerUnitScale, heightBackground, deltaTop, displacementScale;

        if (SystemInfo.deviceType == DeviceType.Handheld)//para celular
        {
            displacementPerUnitScale = heightCanvas / 10;
            heightBackground = background.rectTransform.rect.width;
            deltaTop = (heightCanvas - heightBackground) / 2;
            displacementScale = deltaTop / displacementPerUnitScale;

            background.rectTransform.position = new Vector2(0, displacementScale);
        }
        else//para pc
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
            cameraResolution = cam_devices[frontCamera].availableResolutions[mainResolution];//de la camara principal cogeme la resolucion ej 4:3

            float height, width, ratio; //En esta seccion trabajo con unidades de pixeles

            height = cameraResolution.width;
            width = cameraResolution.height;
            ratio = height / width; // ej para 4:3 es 1.333333333, no tiene unidades 

            return ratio;
        }

        return (float)cam_texture.height / (float)cam_texture.width;
    }

    public string GetHexColor() => strHexColor;

}
