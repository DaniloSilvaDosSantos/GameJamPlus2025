using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public string menuCanvasName = "CanvasMenu";
    public GameObject mainMenu;
    public Button[] optionsButtons;
    public Image[] optionsButtonHighlights;
    public int currentButtonIndex = 0;
    public Slider volumeSlider;

    [Space]
    public Color highlightColor;
    public Color defaultColor;

    void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("volume", 0.8f);
        volumeSlider.onValueChanged.AddListener(SetVolume);

        optionsButtons[0].onClick.AddListener(() => { });
        optionsButtons[1].onClick.AddListener(CloseOptions);

        Debug.Log(currentButtonIndex);
        UpdateButtonHighlights();
    }

    void Update()
    {
        HandleNavigation();
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            optionsButtons[currentButtonIndex].onClick.Invoke();
        }
    }

    void HandleNavigation()
    {
        int previousButtonIndex = currentButtonIndex;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentButtonIndex--;
            if (currentButtonIndex < 0) currentButtonIndex = optionsButtons.Length - 1;
            Debug.Log(currentButtonIndex);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            currentButtonIndex++;
            if (currentButtonIndex >= optionsButtons.Length) currentButtonIndex = 0;
            Debug.Log(currentButtonIndex);
        }

        if (previousButtonIndex != currentButtonIndex)
        {
            UpdateButtonHighlights();
        }

        if (currentButtonIndex == 0 && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            volumeSlider.value -= 0.1f;
        }
        else if (currentButtonIndex == 0 && (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            volumeSlider.value += 0.1f;
        }
    }

    void UpdateButtonHighlights()
    {
        for (int i = 0; i < optionsButtons.Length; i++)
        {
            Image buttonImage = optionsButtons[i].GetComponent<Image>();
            if (i == currentButtonIndex)
            {
                buttonImage.color = highlightColor;
                optionsButtonHighlights[i].gameObject.SetActive(true);
            }
            else
            {
                buttonImage.color = defaultColor;
                optionsButtonHighlights[i].gameObject.SetActive(false);
            }
        }
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.Save();
        AudioListener.volume = volume;
    }

    public void CloseOptions()
    {
        Debug.Log("Closing Options Menu !");

        GameObject menuCanvas = GameObject.Find(menuCanvasName);
        MainMenuManager menuController = GameObject.Find("CanvasMainMenu").GetComponent<MainMenuManager>();
        if(menuController.previousMenu == "MainMenu")
        {
            Instantiate(mainMenu, menuCanvas.transform);
            menuController.currentButtonIndexTemp = 1;
        } 
        
        Destroy(gameObject);
    }
}
