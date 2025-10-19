using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GM;

    [SerializeField] private Image FadeScreen;
    [SerializeField] private AudioSource MainAudio;

    [SerializeField] private Material DuskSkybox;
    [SerializeField] private Material NightSkybox;

    public string currentScene = "";
    public string sceneToChange = "";

    public Vector3 RespawnPos = new Vector3 (0f,0f,0f);

    public bool Fade = false;

    public float Vol = 1f;
    public float Pitch = 1f;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        GM = this;
        FadeScreen.gameObject.SetActive(true);

        RenderSettings.skybox = DuskSkybox;
        RenderSettings.ambientIntensity = 0.3f;
        ChangeScene("MainMenu");

    }

    public void NightTime()
    {
        Vol = 0f;
        RenderSettings.skybox = NightSkybox;
        RenderSettings.ambientIntensity = 0f;
    }

    void Update()
    {
        HandleFade();
    }

    void FixedUpdate()
    {
        HandleFade();
        SoundSettings();
    }

    void SoundSettings()
    {
        MainAudio.pitch = Mathf.Lerp(MainAudio.pitch, Pitch, 0.5f * Time.deltaTime);
        MainAudio.volume = Mathf.Lerp(MainAudio.volume, Vol, 0.5f * Time.deltaTime);
    }

    void HandleFade()
    {
        var tempColor = FadeScreen.color;

        if (tempColor.a < 0.05f) FadeScreen.enabled = false;
        else FadeScreen.enabled = true;

        if (Fade && tempColor.a < 1f)
        {
            tempColor.a = Mathf.Min(tempColor.a + 0.5f * Time.deltaTime, 1f);
            FadeScreen.color = tempColor;
        }
        else if (!Fade && tempColor.a > 0f)
        {
            tempColor.a = Mathf.Max(tempColor.a - 0.5f * Time.deltaTime, 0f);
            FadeScreen.color = tempColor;
        }
    }

    public void OffFade()
    {
        Fade = false;
    }

    public void ChangeScene(string newScene, bool doFade = true)
    {
        if (doFade)Fade = true;

        sceneToChange = newScene;

        Invoke("DoTheSceneChange", 2f);
    }

    public void RestartLevel()
    {
        if (currentScene != "") SceneManager.UnloadSceneAsync(currentScene);
        currentScene = sceneToChange;
        SceneManager.LoadScene(sceneToChange, LoadSceneMode.Additive);
        GameObject Player= GameObject.FindWithTag("Player");

        if (RespawnPos != new Vector3(0f,0f,0f))
        {
            Player.transform.position = RespawnPos;
        }
    }

    public void DoTheSceneChange()
    {
        if (currentScene != "") SceneManager.UnloadSceneAsync(currentScene);
        currentScene = sceneToChange;
        SceneManager.LoadScene(sceneToChange, LoadSceneMode.Additive);

        Invoke("OffFade", 2f);
    }

    public void LeaveGame()
    {
        Application.Quit();
    }
}
