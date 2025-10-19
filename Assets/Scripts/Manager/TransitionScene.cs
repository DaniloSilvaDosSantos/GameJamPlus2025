using UnityEngine;

public class TransitionScene : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.GM.ChangeScene("GamePlay");
            GameManager.GM.NightTime();
        }
    }
}
