using UnityEngine;

public class SettingsOption : MonoBehaviour
{
    public GameObject settingMenu;

    public void Open() => settingMenu.SetActive(true);
    public void Close() => settingMenu.SetActive(false);
}
