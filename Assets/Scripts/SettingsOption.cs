using UnityEngine;

public class SettingsOption : MonoBehaviour
{
    public GameObject settingMenu;
    public GameObject modelHEX;
    public GameObject modelRGB;
    public GameObject modelHSV;

    public void Open() => settingMenu.SetActive(true);
    public void Close() => settingMenu.SetActive(false);
    public void DisableActiveHEX() => modelHSV.SetActive(!modelHSV.activeInHierarchy); 
    public void DisableActiveHSV() => modelHSV.SetActive(!modelHSV.activeInHierarchy);
    public void DisableActiveRGB() => modelHSV.SetActive(!modelHSV.activeInHierarchy);
}
