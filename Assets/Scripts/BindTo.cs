using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BindTo : MonoBehaviour
{
    public Transform target;
    public Vector3 localPosition;
    public Quaternion localRotation;

    void Update()
    {
        transform.position = target.transform.TransformPoint(localPosition);
        transform.rotation = target.transform.rotation * localRotation;
    }
}
