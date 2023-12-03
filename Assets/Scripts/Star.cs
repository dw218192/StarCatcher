using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public float fallForce = 2.0f;
    public ParticleSystem scoringEffect;
    public AudioClip scoringSound;

    public Collider collider;
    public GameObject meshRenderer;
    Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rigidbody.AddForce(Vector3.down * fallForce);
    }

    void Update() {
        if (GameMgr.instance.gameState != GameMgr.GameState.Playing)
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Consts.TAG_SWORD))
        {
            GameMgr.instance.Score += 1;
            scoringEffect.Play();
            AudioSource.PlayClipAtPoint(scoringSound, transform.position);
            collider.enabled = false;
            meshRenderer.SetActive(false);
            Destroy(gameObject, 0.5f);
        }
        else if (collision.gameObject.CompareTag(Consts.TAG_GROUND))
        {
            Destroy(gameObject);
        }
    }
}
