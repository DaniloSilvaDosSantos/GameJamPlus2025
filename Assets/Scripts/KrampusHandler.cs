using UnityEngine;

public class KrampusHandler : MonoBehaviour
{
    [Header("Declaração de objetos")]
    [SerializeField] private Rigidbody RB;
    [SerializeField] private GameObject Player;
    [SerializeField] private LayerMask PlayerMask;
    [SerializeField] private Animator AnimatorMove;

    [Header("Declaração de audio")]
    [SerializeField] private AudioSource AS;
    [SerializeField] private AudioClip Slowstep;
    [SerializeField] private AudioClip Faststep;
    [SerializeField] private AudioClip Scream1;

    [Header("Controle de comportamento")]
    public bool attacking = false;
    public bool startle = false;
    public bool currentlyStartled = false;
    private bool screamed = false;

    [Header("Variáveis de comportamento")]
    [SerializeField] private float stalkSpeed = 3f;
    [SerializeField] private float leapSpeed = 8f;
    [SerializeField] private float leapDist = 8f;
    [SerializeField] public float startleRange = 8f;

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

    
    public void DelayedRemove()
    {
        Invoke("Removeself", 5f);
    }

    void Removeself()
    {
        Destroy(this.gameObject);
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


        if (currentlyStartled)
        {
            AnimatorMove.SetBool("Stalking", false);
            AnimatorMove.SetBool("Running", false);
            AnimatorMove.SetBool("Scared", true);
            if (!screamed)
            {
                if (!AS.isPlaying) AS.PlayOneShot(Scream1);
                screamed = true;
            }
            if (!AS.isPlaying) AS.PlayOneShot(Slowstep);
            RB.linearVelocity = -transform.forward*stalkSpeed + new Vector3(0f, -1f, 0f);
        }
        else
        {
            screamed = false;
            if (playerDirection.magnitude > 20f)
            {
                AnimatorMove.SetBool("Running", false);
                AnimatorMove.SetBool("Stalking", true);
                RB.linearVelocity = transform.forward*2f*stalkSpeed + new Vector3(0f, -1f, 0f);
            }
            else if (playerDirection.magnitude > leapDist)
            {
                AnimatorMove.SetBool("Running", false);
                AnimatorMove.SetBool("Stalking", true);
                if (!AS.isPlaying) AS.PlayOneShot(Slowstep);
                RB.linearVelocity = transform.forward*stalkSpeed + new Vector3(0f, -1f, 0f);
            }
            else
            {
                AnimatorMove.SetBool("Running", true);
                AnimatorMove.SetBool("Stalking", false);
                if (!AS.isPlaying) AS.PlayOneShot(Faststep);
                RB.linearVelocity = transform.forward*leapSpeed + new Vector3(0f, -3f, 0f);
            }
        }

    }
}
