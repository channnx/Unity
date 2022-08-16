using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RayCast : MonoBehaviour
{
    //assigned in inspector
    public GameObject particleExplosion;
    public AudioClip Ray;

    //accessed from destroy.cs
    public bool powerRay = false;
    public int magicSpellCounter = 0;

    LineRenderer lineRenderer;
    Vector3 whichDirection = Vector3.up;

    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
    }

    void Update()
    {
        Debug.Log(magicSpellCounter);
        if (magicSpellCounter == 4)
        {
            gameObject.tag = "AllPowerfull";
        } 
    
        //determine the direction of the walk nad set the...
        //vector diretion for the death ray

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (horizontal < 0)
            whichDirection = Vector3.left;
        else if (horizontal > 0)
            whichDirection = Vector3.right;
        else if (vertical > 0)
            whichDirection = Vector3.up;
        else if (vertical < 0)
            whichDirection = Vector3.down;
        

        if (Input.GetKey(KeyCode.Space))
        {
            //set the raycast to detect in the direction of the walk
            Vector3 fwd = transform.TransformDirection(whichDirection);

            //ray cast code
            RaycastHit hit;
            Transform whatTransform; //what the reaycast hit
            GameObject lineRendererTarget; //the target of the linerenderer will be the raycast hit object

            if (Physics.Raycast(transform.position, fwd, out hit, 10))
            {
                whatTransform = hit.transform;
            
                // did the ray hit a gate and did the chararcter get the power up?

                if (whatTransform.tag == "Gate" && powerRay)
                {
                    AudioSource.PlayClipAtPoint(Ray, transform.position);

                    //create the line renderer
                    lineRendererTarget = hit.transform.gameObject;
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, lineRendererTarget.transform.position);
                    lineRenderer.enabled = true; //turn it off after

                    //create the particle explosion and kill gate
                    Instantiate(particleExplosion, hit.transform.position, Quaternion.identity);
                    whatTransform.GetComponent<Destroy>().kill = true;


                    //show the ray for .15 second before killing it
                    StartCoroutine(Pause());
                    powerRay = false;
                    GetComponent<Move>().Wizlight.enabled = false; //turn off the halo after shooting a ray
                    gameObject.GetComponent<Renderer>().material.color = Color.white;
                }
            }
        }
    } // end update

    IEnumerator Pause()
    {
        yield return new WaitForSeconds(.15f);
        GetComponent<LineRenderer>().enabled = false;
    }
}
            
