using UnityEngine;

public class VentilationSwitchController : MonoBehaviour
{
    public GameObject[] switches;
    public Vector2 direction;
    public bool isFinalPoint = false;
    bool isActive = false;
    Rigidbody2D ratRb;
    Animator animator;
    AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        ratRb = GameObject.FindGameObjectWithTag("Rat").GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // if Player activates a switch
        if (isActive && !isFinalPoint && other.gameObject.tag == "Player")
        {
            Enable();               // animate it
            StartMove();            // move rat

            
        }
        // if Rat reaches the objective
        else if (isActive && isFinalPoint && other.gameObject.tag == "Rat")
        {
            GameObject.Find("VentilationGame").GetComponent<VentilationGameController>().EndGame();
        }
    }

    // animate the switch and stop all the others
    void Enable()
    {
        if (!animator.GetBool("enabled"))
        {
            audioSource.Play();
            animator.SetBool("enabled", true);            
        }
        DisableOthers();
    }

    // stop the switch animation
    public void Disable()
    {
        animator.SetBool("enabled", false);
    }

    // move the rat
    void StartMove()
    {
        ratRb.velocity = direction;
    }

    // tell if the switch is ready to receive collisions or not
    public void Activate(bool active)
    {
        isActive = active;
        if (!active)
            Disable();
    }

    // disable all the other switches
    void DisableOthers()
    {
        VentilationSwitchController ctrlr;
        foreach (GameObject ventil in switches)
        {
            ctrlr = ventil.GetComponent<VentilationSwitchController>();
            ctrlr.Disable();
        }
    }
}
