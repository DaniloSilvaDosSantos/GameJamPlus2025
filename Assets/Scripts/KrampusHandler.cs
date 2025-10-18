using UnityEngine;

public class KrampusHandler : MonoBehaviour
{
    [Header("Declaração de objetos")]
    [SerializeField] private Rigidbody RB;
    [SerializeField] private GameObject Player;
    [SerializeField] private LayerMask Mobs;

    [Header("Controle de comportamento")]
    public bool attacking = false;

    [Header("Variáveis de comportamento")]
    [SerializeField] private float stalkSpeed = 3f;
    [SerializeField] private float leapSpeed = 8f;
    [SerializeField] private float leapDist = 8f;

    void FixedUpdate()
    {
        if (!attacking) return;

        HandlePlayerChase();
    }

    void HandlePlayerChase()
    {
        Vector3 playerDirection = Player.transform.position - transform.position;

        Quaternion rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
        transform.rotation = rotation;

        if (playerDirection.magnitude > leapDist)
        {
            RB.linearVelocity = transform.forward*stalkSpeed;
        }
        else
        {
            RB.linearVelocity = transform.forward*leapSpeed;
        }
    }
}
