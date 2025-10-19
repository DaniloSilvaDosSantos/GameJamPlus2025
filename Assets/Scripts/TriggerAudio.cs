using UnityEngine;

public class TriggerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource Audio;
    [SerializeField] private bool doneThing = false;


    void TriggerAction()
    {
        Audio.Play();
    }

    void OnTriggerEnter(Collider other)
    {
        if (doneThing) return;
        if (other.gameObject.CompareTag("Player"))
        {
            TriggerAction();
            doneThing = true;
            Invoke("SelfRemove", 5f);
        }
    }

    void SelfRemove()
    {
        Destroy(this);
    }
}

