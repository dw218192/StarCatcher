using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public GameObject camera;
    public static PlayerScript instance;
    public CapsuleCollider collider;
    public float desiredPlayerHeight = 1.7f;
    public GameObject cameraRig;

    void Awake()
    {
        instance = this;
        collider.height = desiredPlayerHeight;
        float groundHeight = 0.0f;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Ground"))) {
            groundHeight = hit.point.y;
        }

        float curHeight = cameraRig.transform.position.y - groundHeight;
        float scaleY = curHeight / desiredPlayerHeight;
        cameraRig.transform.localScale = new Vector3(1, scaleY, 1);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = camera.transform.position - new Vector3(0, desiredPlayerHeight / 2, 0);
    }
}
