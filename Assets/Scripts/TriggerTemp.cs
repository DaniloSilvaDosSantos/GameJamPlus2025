using UnityEngine;

public class TriggerTemp : MonoBehaviour
{
    [SerializeField] private GameObject Spawnable;


    void TriggerAction()
    {
        Spawnable.SetActive(true);
    }

    void LeaveAction()
    {
        Destroy(Spawnable);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            TriggerAction();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            LeaveAction();
            Destroy(this);
        }
    }
}
