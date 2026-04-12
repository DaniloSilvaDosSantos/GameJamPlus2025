using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public string menuCanvasName = "CanvasMenu";
    public GameObject mainMenu;

    public Button backButton;
    public Slider volumeSlider;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        volumeSlider.value = PlayerPrefs.GetFloat("volume", 0.8f);
        volumeSlider.onValueChanged.AddListener(SetVolume);

        backButton.onClick.AddListener(CloseOptions);
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.Save();
        AudioListener.volume = volume;
    }

    public void CloseOptions()
    {
        ResetAllWorldButtons();

        GameObject canvas = GameObject.Find(menuCanvasName);
        MainMenuManager manager = GameObject.Find("CanvasMainMenu").GetComponent<MainMenuManager>();

        if (manager.previousMenu == "MainMenu")
        {
            Instantiate(mainMenu, canvas.transform);
        }

        Destroy(gameObject);
    }

    void ResetAllWorldButtons()
    {
        foreach (var btn in FindObjectsByType<WorldButtonFeedback>(FindObjectsSortMode.None))
        {
            btn.ResetMaterial();
        }
    }
}
