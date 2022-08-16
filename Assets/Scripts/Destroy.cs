using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    //spells
    public GameObject Power;
    public AudioClip magicSpell;

    //blue fire
    public GameObject Explosion;
    public AudioClip screamExplode;

    //red fire
    public AudioClip Boom;

    //gates
    public bool kill = false;

    //treasure
    public AudioClip Fanfare;


    Transform whatObject;

    void OnCollisionEnter(Collision col)
    {
        whatObject = col.gameObject.transform;

        if (col.gameObject.tag == "Wiz" && this.gameObject.tag == "Power") //hit a power spell
        {
            DestroyThisObject(magicSpell, whatObject, Power);

            //tell the RayCast script on the Wiz that the power up is true
            col.gameObject.GetComponent<RayCast>().powerRay = true;
            col.gameObject.GetComponent<RayCast>().magicSpellCounter += 1;
        }

        else if (col.gameObject.tag == "Wiz" && this.gameObject.tag == "Blue_Fire") //hit a blue flame
            DestroyThisObject(screamExplode, whatObject, Explosion);

        else if (col.gameObject.tag == "AllPowerfull" && this.gameObject.tag == "Fire")
        {
            //hit the last fire
            DestroyThisObject(Boom, whatObject, Explosion);
            GameObject.Find("Wizard").GetComponent<Move>().Wizlight.color = Color.red;
            GameObject.Find("Wizard").GetComponent<Renderer>().material.color = Color.white;
        }

        else if (col.gameObject.tag == "AllPowerfull" && this.gameObject.tag == "Treasure")
        {
            AudioSource.PlayClipAtPoint(Fanfare, transform.position);
            Destroy(GameObject.Find("Evil_Spell"));
        }
    }
    
    void DestroyThisObject(AudioClip whatAudio, Transform whatObject, GameObject whatParticle)
    {
        Destroy(this.gameObject);
        AudioSource.PlayClipAtPoint(whatAudio, transform.position);
        Instantiate(whatParticle, whatObject.transform.position, Quaternion.identity);
    }

    void Update()
    {
        //message sent from RayCast.cs to kill gates when ray hits a gate
        if (kill)
        {
            AudioSource.PlayClipAtPoint(Boom, transform.position);
            Destroy(this.gameObject);
            kill = false;
        }
    }
}
