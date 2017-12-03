using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentilationSwitchController : MonoBehaviour
{
    public GameObject[] switches;
    public Vector2 direction;
    Rigidbody2D ratRb;
    Animator animator;
    AudioSource audio;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        ratRb = GameObject.FindGameObjectWithTag("Rat").GetComponent<Rigidbody2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
            Enable();
    }


    void Enable()
    {
        ratRb.velocity = direction;
        animator.SetBool("enabled", true);
        DisableOthers();
        audio.Play();
    }

    public void Disable()
    {
        animator.SetBool("enabled", false);
    }

    void DisableOthers()
    {
        foreach(GameObject ventil in switches)
        {
            ventil.GetComponent<VentilationSwitchController>().Disable();
        }
    }
}
