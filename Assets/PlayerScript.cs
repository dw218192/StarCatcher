using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    static public PlayerScript instance = null;
    public float desiredPlayerHeight = 1.7f;
    public GameObject cameraRig;
    public GameObject centerEyeAnchor;

    SphereCollider col;

    void Awake()
    {
        instance = this;
        transform.position = centerEyeAnchor.transform.position;
    }

    void Start() {
        float groundHeight = 0.0f;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground"))) {
            groundHeight = hit.point.y;
        }

        float curHeight = cameraRig.transform.position.y - groundHeight;
        float scaleY = desiredPlayerHeight / curHeight;
        cameraRig.transform.localScale = new Vector3(1, scaleY, 1);
    }
    void Update() {
        transform.position = centerEyeAnchor.transform.position;
    }
}
