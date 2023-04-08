using UnityEngine;

public class Menu : MonoBehaviour
{
    [Tooltip("GameObject that contains the menu options.")]
    public GameObject menu;
    public GameObject history;
    public GameObject settings;

    History historyScript;
    SettingsOption settingsScript;

    private void Awake()
    {
        /*  Close all panels    */
        menu.SetActive(false);
        history.SetActive(false);
        settings.SetActive(false);

        /*  Get respectives functions and properties of each GameObject   */
        historyScript = history.GetComponent<History>();
        settingsScript = settings.GetComponent<SettingsOption>();

    }

    public void ShowHideMenu() => menu.SetActive(!menu.activeSelf);
    public void OpenHistory() => historyScript.Open();
    public void OpenSettings() => settingsScript.Open();
}
