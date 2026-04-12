using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public string menuCanvasName = "CanvasMenu";

    public Button playButton;
    public Button optionsButton;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        playButton.onClick.AddListener(PlayGame);
        optionsButton.onClick.AddListener(OpenOptions);
    }
    
    void PlayGame()
    {
        ResetAllWorldButtons();
        GameManager.GM.ChangeScene("NewIntro");
    }

    void OpenOptions()
    {
        ResetAllWorldButtons();

        MainMenuManager manager = GameObject.Find("CanvasMainMenu").GetComponent<MainMenuManager>();
        GameObject canvas = GameObject.Find(menuCanvasName);

        Instantiate(optionsMenu, canvas.transform);
        manager.previousMenu = "MainMenu";

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
