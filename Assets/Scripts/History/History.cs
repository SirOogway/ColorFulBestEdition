using UnityEngine;
using UnityEngine.UI;

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

    byte uninstantiatedHexData;
    bool isFirstOpened;
    byte aperturas;

    public void Save()
    {
        string hexColor;
        hexColor = phoneCameraProjection.GetHexColor();
        SaveManager.SaveColorData(hexColor);
        uninstantiatedHexData++;
    }

#nullable enable
    public void Open()
    {
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

        ColorData? hexData = SaveManager.LoadHexData();

         //When history is opened in first time and there are data recover and instantiate all data
         if (isFirstOpened && hexData != null)
         {
             uninstantiatedHexData = (byte)hexData.GetHexModels().Count; 
             isFirstOpened = !isFirstOpened;
         }

         history.SetActive(true);
         InstantiateHexDataOnHistory(hexData, uninstantiatedHexData);
         uninstantiatedHexData = 0;
    }
#nullable disable
    public void Close()
    {
        /*  Cam play    */
        if (!camTexture.isPlaying)
            camTexture.Play();

        /*  Close history   */
        //The history is only disabled, this allows instances to persist
        history.SetActive(false);
    }

#nullable enable
    void InstantiateHexDataOnHistory(ColorData? hexData, byte quantityToRecover)
    {
        /*  Empty history   */
        //If there is no data then open the empty histoy 
        if (hexData == null)
        {
            //Pop up or text that contains that no exist data
             
            return;
        }

        /*  History with info   */
        //Instantiating uninstantiated object
        for (byte i = 0; i < quantityToRecover; i++)
        {
            GameObject record;
            record = Instantiate(recordPfs, parent.transform);
            string hex = hexData.GetHexModels().Pop();
            record.name = $"Record_{i + " " + hex}";

            /*  Asigning the info    */
            Image fill = record.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
            fill.color = Color.HSVToRGB(25,40,100);
            Debug.Log("Color: " + Color.HSVToRGB(25,40,100));
        }
    }
#nullable disable
    
}
