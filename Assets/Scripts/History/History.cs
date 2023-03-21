using UnityEngine;

public class History : MonoBehaviour
{
    public void SaveHexColor()
    {
        string hexColor;
        PhoneCameraProjection phoneCameraProjection;

        phoneCameraProjection = FindObjectOfType<PhoneCameraProjection>();
        hexColor = phoneCameraProjection.GetHexColor();
        SaveManager.SaveColorData(hexColor);
        Debug.Log($"Saved {hexColor}");
    }

#nullable enable
    public void LoadHexColor()
    {
        ColorData? colorData = SaveManager.LoadColorData();

        if (colorData == null) Debug.Log("No exist color data");
        else
        {
            string hexColor;
            hexColor = colorData.hexColor;
            Debug.Log($"Loaded Color {hexColor}");
        }
    }

}
