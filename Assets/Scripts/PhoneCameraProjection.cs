using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ColorExtensions;

public class PhoneCameraProjection : MonoBehaviour
{
    WebCamTexture camTexture; //es la referencia a la imagen que captura la camara
    WebCamDevice[] cam_devices; //almacena las camaras del celular
    int frontCamera;

    [SerializeField] [Tooltip("Background shows the image captured by the camera.")] 
    RawImage background; 

    [SerializeField] [Tooltip("Allows you to obtain the dimensions of the phone screen in units of unity.")]
    Canvas canvas;
    float widthCanvas;
    float heightCanvas;
    bool isPhoneDevice;

    Color pixelColor;

    [SerializeField][Tooltip("The models array contains the information about the color models.")]
    TMP_Text[] models;
    
    [SerializeField] Image colorImage;

    private void Awake()
    {
        Time.fixedDeltaTime = 1f; //1 excecution every 1 second
        isPhoneDevice = SystemInfo.deviceType == DeviceType.Handheld;

        cam_devices = WebCamTexture.devices; //obtengo los dispositivos de camara, si tiene 1 camara 2 camaras 3 camaras etc
        frontCamera = 0;
        camTexture = new WebCamTexture(cam_devices[frontCamera].name);
    }

    private void Start()
    {
        widthCanvas = canvas.GetComponent<RectTransform>().rect.width;
        heightCanvas = canvas.GetComponent<RectTransform>().rect.height;

        if (camTexture != null)
            camTexture.Play(); //la camara empieza a capturar

        background.texture = camTexture; //al background le asignamos la textura(imagen de la camara) obtenida

        ManagerAntiRotation();
        ManagerBackgroundSize();
        //ManagerBackgroundPosition();
    }

    void FixedUpdate()
    {
        pixelColor = camTexture.GetPixel(camTexture.width / 2, camTexture.height / 2);

        string strHexColor, strRGBColor, strHSVColor;

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
        if (isPhoneDevice)
        {
            int antiRotate = 360 - 90;
            Quaternion quatRotation = new Quaternion();
            quatRotation.eulerAngles = new Vector3(0, 0, antiRotate);
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

        heightBackground = heightCanvas;
        ratio = Ratio();
        widthBackground = heightBackground / ratio;

        if (isPhoneDevice)
        {
            background.rectTransform.sizeDelta = new Vector2(heightBackground, widthBackground);
            background.GetComponent<BoxCollider>().size = new Vector3(heightBackground, widthBackground, 1);
            return;
        }

        background.rectTransform.sizeDelta = new Vector2(widthBackground, heightBackground);
        background.GetComponent<BoxCollider>().size = new Vector3(widthBackground, heightBackground, 1);
    }

    void ManagerBackgroundPosition()
    {
        float displacementPerUnitScale, heightBackground, deltaTop, displacementScale;

        if (isPhoneDevice)//para celular
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
        if (isPhoneDevice)
        {
            Resolution cameraResolution;
            int mainResolution;

            mainResolution = 0;
            cameraResolution = cam_devices[frontCamera].availableResolutions[mainResolution];//de la camara principal cogeme la resolucion ej 4:3

            float height, width, ratio; //En esta seccion trabajo con unidades de pixeles

            height = cameraResolution.width;
            width = cameraResolution.height;
            ratio = height / width; // ej para 4:3 es 1.333333333, no tiene unidades 

            return ratio;
        }

        return (float)camTexture.height / (float)camTexture.width;
    }

    public WebCamTexture GetCamTexture() => camTexture;

    public Color GetHexColor() => pixelColor;
}
