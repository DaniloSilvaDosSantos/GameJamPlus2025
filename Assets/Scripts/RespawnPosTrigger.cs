using UnityEngine;

public class RespawnPosTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.GM.RespawnPos = other.gameObject.transform.position;
            Destroy(this);
        }
    }
}
