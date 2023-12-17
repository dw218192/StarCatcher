using Autohand;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public Transform barrelTip;
    public float hitPower = 1;
    public float recoilPower = 1;
    public float range = 100;
    public LayerMask layer;
    public ParticleSystem muzzleFlash;

    public AudioClip shootSound;
    public float shootVolume = 1f;

    private void Start()
    {
        muzzleFlash.playOnAwake = false;
    }

    public void Shoot()
    {
        //Play the audio sound
        if (shootSound)
            AudioSource.PlayClipAtPoint(shootSound, transform.position, shootVolume);

        RaycastHit hit;
        if (Physics.Raycast(barrelTip.position, barrelTip.forward, out hit, range, layer))
        {
            if (hit.collider.gameObject.TryGetComponent<Star>(out var star))
            {
                star.Die();
            }
        }
            
        muzzleFlash.Play();
    }
}