using UnityEngine;

public class KrampusHandler : MonoBehaviour
{
    [Header("Declaração de objetos")]
    [SerializeField] private Rigidbody RB;
    [SerializeField] private GameObject Player;
    [SerializeField] private LayerMask PlayerMask;
    [SerializeField] private Animator AnimatorMove;

    [Header("Controle de comportamento")]
    public bool attacking = false;

    [Header("Variáveis de comportamento")]
    [SerializeField] private float stalkSpeed = 3f;
    [SerializeField] private float leapSpeed = 8f;
    [SerializeField] private float leapDist = 8f;

    void FixedUpdate()
    {
        if (!attacking)
        {
            AnimatorMove.SetBool("Stalking", false);
            AnimatorMove.SetBool("Running", false);
            return;
        }

        HandlePlayerChase();
    }

    void HandlePlayerChase()
    {
        if (Player == null)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 100f, PlayerMask);
            foreach (var collider in hitColliders)
            {
                if (collider.gameObject.CompareTag("Player"))
                    {
                        Player = collider.gameObject;
                        break;
                    }
            }
        }

        Vector3 playerDirection = Player.transform.position - transform.position;

        Quaternion rotation = Quaternion.LookRotation(playerDirection, Vector3.up);
        transform.rotation = rotation;

        if (playerDirection.magnitude > leapDist)
        {
            AnimatorMove.SetBool("Running", false);
            AnimatorMove.SetBool("Stalking", true);
            RB.linearVelocity = transform.forward*stalkSpeed;
        }
        else
        {
            AnimatorMove.SetBool("Running", true);
            AnimatorMove.SetBool("Stalking", false);
            RB.linearVelocity = transform.forward*leapSpeed;
        }
    }
}
