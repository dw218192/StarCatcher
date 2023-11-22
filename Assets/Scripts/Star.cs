using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public float fallForce = 2.0f;
    Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rigidbody.AddForce(Vector3.down * fallForce);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(Consts.TAG_SWORD))
        {
            GameMgr.instance.Score += 1;
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag(Consts.TAG_GROUND))
        {
            Destroy(gameObject);
        }
    }
}
