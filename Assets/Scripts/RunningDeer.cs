using UnityEngine;

public class RunningDeer : MonoBehaviour
{
    public bool running = false;
    public bool willDeerSappear = false;
    public bool startle = false;
    public bool diagonal = false;
    public bool audioloop = true;
    private bool hasLooped = false;
    public float startleRange = 8f;
    public float timer = 10f;
    [SerializeField] private Rigidbody RB;
    [SerializeField] private AudioSource Audio;
    [SerializeField] private Animator AnimatorMove;
    [SerializeField] private Transform Player;

    void Awake()
    {
        AnimatorMove.SetFloat("Offset", Random.Range(0f, 1f));
    }

    void FixedUpdate()
    {
        if (RB.linearVelocity.magnitude > 0.2f) AnimatorMove.SetBool("Running", true);
        else AnimatorMove.SetBool("Running", false);

        if (startle && !running)
        {
            if ((transform.position - Player.position).magnitude < startleRange) running = true;
        }


        if (willDeerSappear)
        {
            willDeerSappear = false;
            Invoke("Removing", timer);
        }

        if (running)
        {
            if(diagonal) RB.linearVelocity = 13f * transform.forward + 6f * transform.up;
            else RB.linearVelocity = 20f * transform.forward;
            if (!Audio.isPlaying && !hasLooped)
            {
                if(!audioloop) hasLooped = true;
                Audio.Play();
            }
        }
        else RB.linearVelocity = 0f * transform.forward;
    }

    public void StartledStart()
    {
        Invoke("Startle", Random.Range(0.05f, 0.25f));
    }

    void Startle()
    {
        running = true;
        willDeerSappear = true;
    }

    void Removing()
    {
        Destroy(this.gameObject);
    }
}
