using UnityEngine;

public class RunningDeer : MonoBehaviour
{
    public bool running = false;
    public bool willDeerSappear = false;
    public bool startle = false;
    public float startleRange = 8f;
    public float timer = 10f;
    [SerializeField] private Rigidbody RB;
    [SerializeField] private AudioSource Audio;
    [SerializeField] private Animator AnimatorMove;
    [SerializeField] private Transform Player;

    // Update is called once per frame
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
