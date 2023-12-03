using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeteorScript : MonoBehaviour
{
    public int damage = 1;
    public float fallForce = 2.0f;
    public AudioClip hitSound;
    public ParticleSystem scoringEffect;
    public GameObject meshRenderer;
    public Collider collider;

    Rigidbody rigidbody;
    Vector3 direction;

    bool isDeflected = false;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        
        var player = PlayerScript.instance;
        direction = player.transform.position - transform.position;
    }

    void FixedUpdate()
    {
        if (isDeflected) {
            rigidbody.AddForce(Vector3.down * fallForce);
        } else {
            rigidbody.AddForce(direction * fallForce);
        }
    }

    void Update() {
        if (GameMgr.instance.gameState != GameMgr.GameState.Playing)
            Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Consts.TAG_SWORD) || collision.gameObject.CompareTag(Consts.TAG_PLAYER))
        {
            GameMgr.instance.Score -= damage;
            GameMgr.instance.Score = Math.Clamp(GameMgr.instance.Score, 0, Int32.MaxValue);
            scoringEffect.Play();
            AudioSource.PlayClipAtPoint(hitSound, transform.position);
            collider.enabled = false;
            meshRenderer.SetActive(false);
            Destroy(gameObject, 0.5f);
        }
        else if (collision.gameObject.CompareTag(Consts.TAG_GROUND) || collision.gameObject.CompareTag(Consts.TAG_METEOR))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag(Consts.TAG_SHEILD)) 
        {
            isDeflected = true;
            rigidbody.AddForce(-rigidbody.velocity.normalized * 10.0f, ForceMode.Impulse);
        }
    }
}
