using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public ParticleSystem muzzleFlash;   // Reference to the muzzle flash particle system

    private void Awake()
    {
        muzzleFlash = GetComponent<ParticleSystem>();
    }

    public void Shoot()
    {
        // Play muzzle flash
        muzzleFlash.Play();

    }
}
