using UnityEngine;
using TMPro;

public class SwitchHandler : MonoBehaviour
{
    [SerializeField] GameObject handlerButton;

    [SerializeField] Color disableColor;
    [SerializeField] Color enableColor;

    [SerializeField] GameObject parent;
    [SerializeField] GameObject typeModel;
    [SerializeField] GameObject model;

    bool isActiveModel; 

    TMP_Text typeModelText;
    TMP_Text modelText;

    /*  Statics properties  */
    static bool stateHEX;
    static bool stateRGB;
    static bool stateHSV;

    private void Awake()
    {
        stateHEX = true;
        stateRGB = true;
        stateHSV = true;

        isActiveModel = true;

        typeModelText = typeModel.GetComponent<TMP_Text>();
        modelText = model.GetComponent<TMP_Text>();
        enableColor = typeModelText.color;
    }

    public void OnSwitchButtonClicked()
    {
        Transform myPosition;
        float displaceX;

        displaceX = -handlerButton.GetComponent<RectTransform>().anchoredPosition.x;
        myPosition = handlerButton.GetComponent<RectTransform>();
        myPosition.localPosition = new Vector3 (displaceX, 0, 0);
        
        Debug.Log($"Switch handler\n" +
            $"Displaced on x: {displaceX}\n");
        ActiveDisableModel();
    }

    void ActiveDisableModel()
    {
        string parentName = parent.name;
        if (parentName == "HEX")
            stateHEX = !stateHEX;
        if (parentName == "RGB")
            stateRGB = !stateRGB;
        if (parentName == "HSV")
            stateHSV = !stateHSV;

        if (isActiveModel)
            DisableModel();
        else
            ActivateModel();

        //Mantengo un control sobre el estado del modelo de color
        isActiveModel = !isActiveModel;
    }

    void ActivateModel()
    {
        /*  Only change the colors  */
        typeModelText.color = enableColor;
        modelText.color = enableColor;
    }

    void DisableModel()
    {
        /*  Only change the colors  */
        typeModelText.color = disableColor;
        modelText.color = disableColor;
    }

    /*  Statics functions    */

    public static bool GetStateHEXModel() => stateHEX;
    public static bool GetStateRGBModel() => stateRGB;
    public static bool GetStateHSVModel() => stateHSV;
}
