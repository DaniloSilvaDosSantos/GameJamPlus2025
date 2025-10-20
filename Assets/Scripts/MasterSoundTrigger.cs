using UnityEngine;

public class MasterSoundTrigger : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    [Header("Trigger Logics")]
    public bool ChangeAudio = false;
    public bool Repeatable = false;

    [Header("Audio Specifics")]
    [SerializeField] private AudioClip ToPlay;
    public float speed = 2f;
    public float vol = 1f;
    public float pitch = 1f;

    void Start()
    {
        meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;
    }

    void TriggerAction()
    {
        GameManager.GM.ChangeSpeed = 1f/speed * Mathf.Min(Mathf.Max(vol, GameManager.GM.Vol), 0.2f);
        GameManager.GM.Vol = vol;
        GameManager.GM.Pitch = pitch;

        if(ChangeAudio) 
        {
            if(GameManager.GM.MainAudio.clip != ToPlay)
            {
                GameManager.GM.MainAudio.clip = ToPlay;
                GameManager.GM.MainAudio.Play();
            }
        }
;
        if (!Repeatable) gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TriggerAction();
        }
    }
}
