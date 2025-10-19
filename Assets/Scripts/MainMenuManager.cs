using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public GameObject menuCanvas;
    public GameObject mainMenu;
    public GameObject optionsMenu;

    public int currentButtonIndexTemp = -1;
    public string previousMenu;

    void Start()
    {
        Instantiate(mainMenu, menuCanvas.transform);
    }
}
