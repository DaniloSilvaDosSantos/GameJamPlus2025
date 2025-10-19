using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MainMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public string menuCanvasName = "CanvasMenu";
    public UnityEngine.UI.Button[] menuButtons;
    public int currentButtonIndex = 0;

    [Space]
    public Color highlightColor;
    public Color defaultColor;

    void Start()
    {
        MainMenuManager menuController = GameObject.Find("CanvasMainMenu").GetComponent<MainMenuManager>();

        if (menuController.currentButtonIndexTemp != -1)
        {
            Debug.Log(menuController.currentButtonIndexTemp);
            currentButtonIndex = menuController.currentButtonIndexTemp;
            menuController.currentButtonIndexTemp = -1;
        }

        Cursor.visible = false;

        menuButtons[0].onClick.AddListener(PlayGame);
        menuButtons[1].onClick.AddListener(OpenOptions);

        Debug.Log(currentButtonIndex);
        UpdateButtonHighlights();
    }

    void Update()
    {
        HandleNavigation();
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            menuButtons[currentButtonIndex].onClick.Invoke();
        }
        if (Input.GetMouseButton(0))
        {
            Cursor.visible = false;
        }
    }

    void HandleNavigation()
    {
        int previousButtonIndex = currentButtonIndex;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentButtonIndex--;
            if (currentButtonIndex < 0) currentButtonIndex = menuButtons.Length - 1;
            Debug.Log(currentButtonIndex);
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentButtonIndex++;
            if (currentButtonIndex >= menuButtons.Length) currentButtonIndex = 0;
            Debug.Log(currentButtonIndex);
        }

        if (previousButtonIndex != currentButtonIndex)
        {
            UpdateButtonHighlights();
        }
    }

    void UpdateButtonHighlights()
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            Image buttonImage = menuButtons[i].GetComponent<Image>();
            if (i == currentButtonIndex)
            {
                buttonImage.color = highlightColor;
            }
            else
            {
                buttonImage.color = defaultColor;
            }
        }
    }
    
    void PlayGame()
    {
        Debug.Log("Going to play the game!");

        GameManager.GM.ChangeScene("GamePlay");
    }

    void OpenOptions()
    {
        Debug.Log("Opening Options Menu!");

        MainMenuManager mainMenuManager = GameObject.Find("CanvasMainMenu").GetComponent<MainMenuManager>();
        GameObject menuCanvas = GameObject.Find(menuCanvasName);
        Instantiate(optionsMenu, menuCanvas.transform);
        mainMenuManager.previousMenu = "MainMenu";

        Destroy(gameObject);
    }
}
