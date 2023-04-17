using UnityEngine;

public class Menu : MonoBehaviour
{
    [Tooltip("GameObject that contains the menu options.")]
    [SerializeField] GameObject menu;
    [SerializeField] GameObject history;
    [SerializeField] GameObject settings;

    History historyScript;
    SettingsOption settingsScript;

    [SerializeField] Camera mainCamera;

    bool isOpen;
    bool isPhoneDevice;

    private void Awake()
    {
        /*  Close all panels    */
        menu.SetActive(false);
        history.SetActive(false);
        settings.SetActive(false);

        /*  Get respectives scripts of each GameObject   */
        historyScript = history.GetComponent<History>();
        settingsScript = settings.GetComponent<SettingsOption>();

        mainCamera = Camera.main; //Gets the main camera
        isPhoneDevice = SystemInfo.deviceType == DeviceType.Handheld;
        isOpen = false;
    }

    private void Update()
    {
        if (isPhoneDevice && Input.touchCount > 0)
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out hit) && hit.transform.name != "Menu")
                HideMenu();
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit) && hit.transform.name != "Menu")
            {
                HideMenu();
                hit.transform.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                Debug.DrawRay(ray.origin, ray.direction * 100f, Color.cyan, 5f);
            }
        }
    }

    public void ShowHideMenu()
    {
        isOpen = !isOpen;
        menu.SetActive(!menu.activeSelf);
    }

    public void HideMenu() => menu.SetActive(false);
    //public void OpenHistory() => historyScript.Open(); //como no es una clase estatica 
    public void CloseHistory() => historyScript.Close();
    public void OpenSettings() => settingsScript.Open();
}
