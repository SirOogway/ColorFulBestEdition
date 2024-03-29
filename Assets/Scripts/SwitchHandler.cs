using UnityEngine;
using TMPro;

public class SwitchHandler : MonoBehaviour
{
    [SerializeField]
    GameObject handlerButton;
    float displaceX;

    [SerializeField]
    Color disableColor;
    [SerializeField]
    Color enableColor;
    
    bool swModels;

    [SerializeField]
    GameObject parent;
    [SerializeField]
    GameObject typeModel;
    [SerializeField]
    GameObject model;

    TMP_Text typeModelText;
    TMP_Text modelText;

    private void Awake()
    {
        swModels = true;

        typeModelText = typeModel.GetComponent<TMP_Text>();
        modelText = model.GetComponent<TMP_Text>();
        
        enableColor = typeModelText.color;
    }

    public void OnSwitchButtonClicked()
    {
        displaceX = -handlerButton.GetComponent<RectTransform>().anchoredPosition.x;
        handlerButton.GetComponent<RectTransform>().localPosition = new Vector3 (displaceX, 0, 0);
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

        if (swModels)
            DisableModel();
        else
            ActivateModel();
        swModels = !swModels;
    }

    void ActivateModel()
    {
        typeModelText.color = enableColor;
        modelText.color = enableColor;
    }

    void DisableModel()
    {
        typeModelText.color = disableColor;
        modelText.color = disableColor;
    }

    /*  Statics properties and functions    */
    static bool stateHEX = true;
    static bool stateRGB = true;
    static bool stateHSV = true;

    public static bool GetStateHEXModel() => stateHEX;
    public static bool GetStateRGBModel() => stateRGB;
    public static bool GetStateHSVModel() => stateHSV;
}
