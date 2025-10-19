using UnityEngine;

public class TriggerDisabler : MonoBehaviour
{
    [SerializeField] private GameObject Spawnable;
    [SerializeField] private AudioSource Audio;
    [SerializeField] private bool hasAudio;
    [SerializeField] private bool doneThing = false;


    void TriggerAction()
    {
        if (hasAudio) Audio.Play();
        Destroy(Spawnable);
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
