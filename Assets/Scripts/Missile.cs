using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public Rigidbody rb;
    public ParticleSystem trail;
    public float velocity;
    public float spread;
    GameObject hitEffectPrefab;
    Transform hitEffectSpawn;

    void Awake()
    {
        Destroy(this.gameObject, 2);

        hitEffectPrefab = (GameObject)Resources.Load("Prefabs/Explosion", typeof(GameObject));
        hitEffectSpawn = transform.Find("HitEffectSpawn");

        velocity += FindObjectOfType<ShipController>().localVelocity.z;
        rb.velocity = transform.forward * velocity + (transform.right + transform.up) * UnityEngine.Random.Range(spread, -spread); //Apply Velocity
        GetComponent<AudioSource>().pitch = UnityEngine.Random.Range(GetComponent<AudioSource>().pitch * 0.8f, GetComponent<AudioSource>().pitch * 1.2f); //Random Bitch
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Missile")
        {
            GetComponentInChildren<Collider>().enabled = false;
            GetComponent<Light>().enabled = false;
            GetComponent<Renderer>().enabled = false;
            trail.Stop();

        }
        GameObject projectile = Instantiate(hitEffectPrefab, hitEffectSpawn.position, transform.rotation); /**/
        // projectile.transform.SetParent(transform.parent); //Spawn & Make Hit Effect A Child Of This Game Object
    }
}
