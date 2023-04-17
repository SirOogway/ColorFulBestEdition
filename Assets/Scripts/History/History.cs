using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ColorExtensions;
using TMPro;

public class History : MonoBehaviour
{   
    public PhoneCameraProjection phoneCameraProjection;
    WebCamTexture camTexture;

    [SerializeField] GameObject history;
    [SerializeField] GameObject recordPfs;
    [SerializeField] GameObject parent;
    [SerializeField] TMP_Text counterText;
    [SerializeField] GameObject pointer;
    [SerializeField] Button deleteHistoryButton; 

    [SerializeField] int limitAmount = 6;
    byte uninstantiatedHexData;
    bool isFirstOpened;
    byte aperturas;
    bool haveData;

    public void Save()
    {
        if (SaveManager.CountData() >= limitAmount)
        {
            Debug.LogError("Storage limit exceeded");
            return;
        }

        string colorString;
        colorString = phoneCameraProjection.GetHexColor().ToString();
        SaveManager.SaveColorData(colorString);
        uninstantiatedHexData++;
    }

#nullable enable
    public void Open()
    {
        pointer.SetActive(false);

        /*  Stop camera */
        camTexture = phoneCameraProjection.GetCamTexture();
        if (camTexture.isPlaying)
            camTexture.Stop();

        /*
         * If is first opened recovery all data
         * If not first opened recovery only data uninstantiated
         */

        /*  Open histoy */
        ColorData? colorData = SaveManager.LoadColorData();

        counterText.text = $"{SaveManager.CountData()}/{limitAmount}";

        haveData = colorData != null;
        DisableDeleteHistoryButton(haveData); //When there are data delete button is active 

        isFirstOpened = aperturas == 0;
        //if (isFirstOpened) aperturas++;
        if (isFirstOpened && colorData != null) //When history is opened in first time and there are data recover and instantiate all data
        {
            uninstantiatedHexData = (byte)colorData.GetHexModels().Count; 
            isFirstOpened = !isFirstOpened;
            aperturas++;
        }

        history.SetActive(true);
        InstantiateHexDataOnHistory(colorData, uninstantiatedHexData);

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
    void InstantiateHexDataOnHistory(ColorData? colorData, byte quantityToRecover)
    {
        /*  Empty history   */
        //If there is no data then open the empty histoy 
        if (colorData == null)
            return;

        bool organizingMultipleInfoOnHistory = false;
        Stack<GameObject> temporalRegister = new Stack<GameObject>();

        /*  History with info   */
        //Instantiating uninstantiated object
        for (byte i = 0; i < quantityToRecover; i++)
        {
            GameObject record;
            string pixelColor;

            pixelColor = colorData.GetHexModels().Pop();
            record = Instantiate(recordPfs, parent.transform);
            record.name = $"Record_{pixelColor}";

            if (!isFirstOpened)
            {
                if (quantityToRecover == 1)
                    record.transform.SetAsFirstSibling();
                
                if (quantityToRecover > 1)
                {
                    organizingMultipleInfoOnHistory = true;
                    temporalRegister.Push(record);
                }
            }
            else
                record.transform.SetAsFirstSibling();
            
            Image image = record.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();

            /*  Recovering the original RGB color of string */
            //The chanels[3] is ignored because is alpha channel and it is not necesary RGBA(1.000, 1.000, 1.000, 1.000)
            string[] channels = pixelColor.Split(", ");
            channels[0] = channels[0].Substring(5);
            channels[2] = channels[2].Substring(0, 5);

            //Rounding to delete the decimals 
            float r = float.Parse(channels[0], System.Globalization.CultureInfo.InvariantCulture);
            float g = float.Parse(channels[1], System.Globalization.CultureInfo.InvariantCulture);
            float b = float.Parse(channels[2], System.Globalization.CultureInfo.InvariantCulture);

            Color RGB = new Color(r, g, b);

            //Assigning the color to fill
            image.color = RGB;

            TMP_Text HEX_TMP_Text, RGB_TMP_Text, HSV_TMP_Text;
            string strHexColor, strRGBColor, strHSVColor;

            //Getting the text componets 
            HEX_TMP_Text = record.transform.GetChild(1).GetChild(0).GetChild(1).GetComponent<TMP_Text>();
            RGB_TMP_Text = record.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<TMP_Text>();
            HSV_TMP_Text = record.transform.GetChild(1).GetChild(2).GetChild(1).GetComponent<TMP_Text>();

            //Getting the real model colors 
            strHexColor = ColorUtility.ToHtmlStringRGB(RGB);
            strRGBColor = RGB.ToStringRGB();
            strHSVColor = RGB.ToStringHSV();

            //Assign the info to texts  
            HEX_TMP_Text.text = strHexColor;
            RGB_TMP_Text.text = strRGBColor;
            HSV_TMP_Text.text = strHSVColor;

        }

        // This process is important because the new info is saving in last position on history 
        if (organizingMultipleInfoOnHistory) //Organizing on top the new info when there are existing info
            foreach (GameObject record in temporalRegister)
                record.transform.SetAsFirstSibling();
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
