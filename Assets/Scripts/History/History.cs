using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ColorExtensions;
using TMPro;

public class History : MonoBehaviour
{   //VOLVER CLASE ESTATICA Y A SETTINGSOPTIONS TAMBIEN
    public PhoneCameraProjection phoneCameraProjection;
    WebCamTexture camTexture;

    [SerializeField]
    GameObject history;
    [SerializeField]
    GameObject recordPfs;
    [SerializeField]
    GameObject parent;
    [SerializeField]
    TMP_Text counterText;
    [SerializeField]
    GameObject pointer;
    [SerializeField]
    Button deleteHistoryButton; 

    byte uninstantiatedHexData;
    bool isFirstOpened;
    byte aperturas;

    [SerializeField]
    int limitAmount = 4;

    public bool haveData;


    private void Update()
    {
        ///METER BUENA INFO ACA que si se necesita para actualizar titulo por ejemplo o si el boton est� activo o inactivo
    }

    public void Save()
    {
        if (SaveManager.CountData() >= limitAmount)
        {
            Debug.LogError("Storage limit exceeded");
            return;
        }

        string hexColor = phoneCameraProjection.GetHexColor().ToString();//no es un hexColor, es un colorString
        SaveManager.SaveColorData(hexColor);
        uninstantiatedHexData++;
    }

#nullable enable
    public void Open()
    {
        pointer.SetActive(false);

        /*  Stop camera */
        camTexture = phoneCameraProjection.camTexture;
        if (camTexture.isPlaying)
            camTexture.Stop();

        /*
         * If is first opened recovery all data
         * If not first opened recovery only data uninstantiated
         */

        /*  Open histoy */
        isFirstOpened = aperturas == 0;
        if (aperturas == 0) aperturas++;

        ColorData? hexData = SaveManager.LoadHexData();//ver si realmente es hexdata

        counterText.text = $"{SaveManager.CountData()}/{limitAmount}";

        haveData = hexData == null;
        DisableDeleteHistoryButton(!haveData);

        //When history is opened in first time and there are data recover and instantiate all data
        if (isFirstOpened && hexData != null)
        {
            uninstantiatedHexData = (byte)hexData.GetHexModels().Count; 
            isFirstOpened = !isFirstOpened;
        }

        history.SetActive(true);
        InstantiateHexDataOnHistory(hexData, uninstantiatedHexData, isFirstOpened);

        uninstantiatedHexData = 0;
    }
#nullable disable
    public void Close()
    {
        pointer.SetActive(true);

        /*  Cam play    */
        if (!camTexture.isPlaying)
            camTexture.Play();

        /*  Close history   */
        //The history is only disabled, this allows instances to persist
        history.SetActive(false);
    }

#nullable enable
    void InstantiateHexDataOnHistory(ColorData? hexData, byte quantityToRecover, bool isFirstOpened)
    {
        /*  Empty history   */
        //If there is no data then open the empty histoy 
        if (hexData == null)
        {
            //Pop up or text that contains that no exist data
             
            return;
        }
        bool organizingMultipleInfoOnHistory = false;
        Stack<GameObject> temporalRegister = new Stack<GameObject>();

        /*  History with info   */
        //Instantiating uninstantiated object
        for (byte i = 0; i < quantityToRecover; i++)
        {
            GameObject record;
            string pixelColor = hexData.GetHexModels().Pop();
            record = Instantiate(recordPfs, parent.transform);
            record.name = $"Record_{pixelColor}";

            if (!isFirstOpened)
            {
                if (quantityToRecover == 1)//nad is tratingMultipleNewInfo is false
                    record.transform.SetAsFirstSibling();
                
                if (quantityToRecover > 1)//nad is tratingMultipleNewInfo is true
                {
                    organizingMultipleInfoOnHistory = true;
                    temporalRegister.Push(record);
                }
            }
            else
            {
                record.transform.SetAsFirstSibling();
            }
            


            /*  Assigning the info    */
            Image fill = record.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
            //RGBA(1.000, 1.000, 1.000, 1.000)

            //Recovering the original RGB color of string 
            //The chanels[3] is ignored because is alpha channel
            string[] channels = pixelColor.Split(", ");
            channels[0] = channels[0].Substring(5);
            channels[2] = channels[2].Substring(0, 5);

            float r = float.Parse(channels[0], System.Globalization.CultureInfo.InvariantCulture);
            float g = float.Parse(channels[1], System.Globalization.CultureInfo.InvariantCulture);
            float b = float.Parse(channels[2], System.Globalization.CultureInfo.InvariantCulture);

            Color RGB = new Color(r, g, b);

            //Assigning the color to fill
            fill.color = RGB;

            //Assign the info to texts  
            TMP_Text HEX_TMP_Text, RGB_TMP_Text, HSV_TMP_Text;
            string strHexColor, strRGBColor, strHSVColor;

            HEX_TMP_Text = record.transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<TMP_Text>();
            RGB_TMP_Text = record.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<TMP_Text>();
            HSV_TMP_Text = record.transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<TMP_Text>();


            strHexColor = ColorUtility.ToHtmlStringRGB(RGB);
            strRGBColor = RGB.ToStringRGB();
            strHSVColor = RGB.ToStringHSV();

            HEX_TMP_Text.text = strHexColor;
            RGB_TMP_Text.text = strRGBColor;
            HSV_TMP_Text.text = strHSVColor;

        }

        if (organizingMultipleInfoOnHistory)
        {
            foreach (GameObject record in temporalRegister)
            {
                record.transform.SetAsFirstSibling();
            }
            organizingMultipleInfoOnHistory = false;
        }
    }
#nullable disable

    private void DisableDeleteHistoryButton(bool state) => deleteHistoryButton.interactable = state;
    
    public void DeleteHistory()
    {
        SaveManager.DeleteData();

        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject recordToDestroy = parent.transform.GetChild(i).gameObject;
            Destroy(recordToDestroy);
        }

        counterText.text = $"{SaveManager.CountData()}/{limitAmount}";
        uninstantiatedHexData = 0;
        aperturas = 0;
        
    }
}
