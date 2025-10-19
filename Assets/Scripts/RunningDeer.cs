using UnityEngine;

public class RunningDeer : MonoBehaviour
{
    public bool running = false;
    public bool willDeerSappear = false;
    public float timer = 10f;
    [SerializeField] private Rigidbody RB;
    [SerializeField] private AudioSource Audio;
    [SerializeField] private Animator AnimatorMove;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (RB.linearVelocity.magnitude > 0.2f) AnimatorMove.SetBool("Running", true);
        else AnimatorMove.SetBool("Running", false);

        if (willDeerSappear)
        {
            willDeerSappear = false;
            Invoke("Removing", timer);
        }

        if (running)
        {
            RB.linearVelocity = 20f * transform.forward;
            if (!Audio.isPlaying) Audio.Play();
        }
        else RB.linearVelocity = 0f * transform.forward;
    }

    void Removing()
    {
        Destroy(this.gameObject);
    }
}
