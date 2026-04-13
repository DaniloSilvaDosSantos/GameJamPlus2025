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
    [SerializeField] private Image DeathScreen;
    public AudioSource MainAudio;
    [SerializeField] private AudioSource DeathAudio;

    [SerializeField] private Material DuskSkybox;
    [SerializeField] private Material NightSkybox;

    public string currentScene = "";
    public string sceneToChange = "";

    public Vector3 RespawnPos = new Vector3 (0f,0f,0f);

    public GameObject Player = null;

    public bool Fade = false;
    public bool Dead = false;

    public float ChangeSpeed = 2f;
    public float Vol = 1f;
    public float Pitch = 1f;
    public float cooldownLevelChange = 0f;

    void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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

        if (currentScene != "") SceneManager.UnloadSceneAsync(currentScene);
        currentScene = "NewOutro";
        SceneManager.LoadScene("NewOutro", LoadSceneMode.Additive);
        GameObject Player= GameObject.FindWithTag("Player");

        Fade = true;
        FadeScreen.enabled = true;

        DeathAudio.Play();

        float alpha = 1f;
        Color currColor = DeathScreen.color;
        currColor = Color.white;
        currColor.a = alpha;
        FadeScreen.color = currColor;

        Invoke("AliveAgain", 3f);
    }

    void Update()
    {
        HandleFade();

        cooldownLevelChange = Mathf.Max(cooldownLevelChange - Time.deltaTime, 0f);

        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (currentScene == "MainMenu")
            {
                Application.Quit();
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
            }
            Invoke("Daytime", 2f);
            ChangeScene("MainMenu");
            RespawnPos = new Vector3 (0f,0f,0f);
        }

        var currColor = DeathScreen.color;

        if (currColor.a > 0f)
        {
           currColor.a = Mathf.Max(currColor.a - 0.5f * Time.deltaTime, 0f);
           DeathScreen.color = currColor;
        }
    }

    void Daytime()
    {
        RenderSettings.skybox = DuskSkybox;
        RenderSettings.ambientIntensity = 0.3f;
    }

    void FixedUpdate()
    {
        HandleFade();
        SoundSettings();
    }

    void SoundSettings()
    {
        if (Pitch < MainAudio.pitch)
        {
           MainAudio.pitch = Mathf.Max(MainAudio.pitch - ChangeSpeed*Time.deltaTime, Pitch);
        }
        if (Pitch > MainAudio.pitch)
        {
           MainAudio.pitch = Mathf.Min(MainAudio.pitch + ChangeSpeed*Time.deltaTime, Pitch);
        }

        if (Vol < MainAudio.volume)
        {
           MainAudio.volume = Mathf.Max(MainAudio.volume - ChangeSpeed*Time.deltaTime, Vol);
        }
        if (Vol > MainAudio.volume)
        {
           MainAudio.volume = Mathf.Min(MainAudio.volume + ChangeSpeed*Time.deltaTime, Vol);
        }
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
        DeathAudio.Play();

        Player= GameObject.FindWithTag("Player");
        Player.GetComponent<PlayerControl>().Jumpscare();

        Invoke("DoRestart", 1f);
    }

    public void DoRestart()
    {
        if(cooldownLevelChange != 0) return;
        cooldownLevelChange = 0.2f;

        if (currentScene != "") SceneManager.UnloadSceneAsync(currentScene);
        currentScene = "NewOutro";
        SceneManager.LoadScene("NewOutro", LoadSceneMode.Additive);

        Dead = true;
        Fade = true;
        FadeScreen.color = Color.black;
        FadeScreen.enabled = true;

        float alpha = 1f;
        Color currColor = DeathScreen.color;
        currColor.a = alpha;
        DeathScreen.color = currColor;
        currColor = FadeScreen.color;
        currColor.a = alpha;
        FadeScreen.color = currColor;

        Invoke("AliveAgain", 3f);

        Invoke("Reassign", 2f);
    }

    void AliveAgain()
    {
        Dead = false;
        Fade = false;
        Invoke("ReturnFade", 1f);
    }

    void Reassign()
    {
        Player= GameObject.FindWithTag("Player");
        if (RespawnPos != Vector3.zero)
        {
            Player.transform.position = RespawnPos;
        }
    }

    void ReturnFade()
    {
        FadeScreen.color = Color.black;
    }

    public void DoTheSceneChange()
    {
        if(cooldownLevelChange != 0) return;
        cooldownLevelChange = 0.2f;


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
