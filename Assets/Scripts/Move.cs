using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Move : MonoBehaviour
{
    public float speed = 5.0f;
    private Animator animator;
    public Light Wizlight; //accessed from Raycast

    public GameObject explosion;
    public AudioClip screamExplode;

    void Start()
    {
        Wizlight = this.GetComponent<Light>();
        Wizlight.enabled = false;

        animator = this.GetComponent<Animator>();
    }

    void Update()
    {
        // walking animation
        animator.SetInteger("Direction", 4);

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (y > 0)
            animator.SetInteger("Direction", 2);
        else if (y < 0)
            animator.SetInteger("Direction", 0);
        else if (x > 0)
            animator.SetInteger("Direction", 3);
        else if (x < 0)
            animator .SetInteger("Direction", 1);
 
        // move the Wiz
        transform.Translate(x * Time.deltaTime * speed, y * Time.deltaTime * speed, 0);
    }

    //control wiz colors
    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Blue_Fire")
        {
            gameObject.GetComponent<Renderer>().material.color = Color.red;
            Wizlight.enabled = false;
            GetComponent<RayCast>().powerRay = false;
        }

        else if (col.gameObject.tag == "Power")
        {
            gameObject.GetComponent<Renderer>().material.color = Color.cyan;
            Wizlight.enabled = true;
        }

        else if (col.gameObject.tag == "EvilSpell")
        {
            StartCoroutine(Pause());
        }
    }

    IEnumerator Pause()
    {
        AudioSource.PlayClipAtPoint(screamExplode, transform.position);
        Instantiate(explosion, this.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("LoseScene");
    }
}
