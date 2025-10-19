using UnityEngine;

public class TriggerEnabler : MonoBehaviour
{
    [SerializeField] private GameObject Spawnable;


    void TriggerAction()
    {
        Spawnable.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TriggerAction();
            Destroy(this);
        }
    }
}
