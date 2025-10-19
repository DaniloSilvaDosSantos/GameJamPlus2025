using UnityEngine;

public class TriggerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject Spawnable;
    [SerializeField] private Transform Spawnpos;


    void TriggerAction()
    {
        Instantiate(Spawnable, Spawnpos.position, Quaternion.identity);
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
