using UnityEngine;

public class SettingsOption : MonoBehaviour
{
    [SerializeField]
    GameObject settingMenu;
    [SerializeField]
    GameObject pointer;
    public void Open()
    {
        pointer.SetActive(false);
        settingMenu.SetActive(true);
    }
    public void Close()
    {
        pointer.SetActive(true);
        settingMenu.SetActive(false);
    }

}
