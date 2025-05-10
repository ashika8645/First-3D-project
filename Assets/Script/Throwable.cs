using System;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] float delay = 3f;
    [SerializeField] float damageRadius = 20f;
    [SerializeField] float explosionForce = 1200f;

    float countdown;

    bool hasExploded = false;
    public bool hasBeenThrown = false;

    public enum ThrowableType
    {
        None,
        Grenade,
        Smoke
    }

    public ThrowableType throwableType;

    private void Start()
    {
        countdown = delay;
    }

    private void Update()
    {
        if (hasBeenThrown)
        {
            countdown -= Time.deltaTime;
            if (countdown <= 0 && !hasExploded)
            {
                Exploded();
                hasExploded = true;
            }
        }
    }

    private void Exploded()
    {
        GetThrowableEffect();

        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
        switch (throwableType)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                break;

            case ThrowableType.Smoke:
                SmokeEffect();
                break;
        }
    }

    private void SmokeEffect()
    {
        GameObject smokeEffect = GlobalReferences.instance.smokeEplosionEffectl;
        Instantiate(smokeEffect, transform.position, transform.rotation);

        SoundManager.instance.grenadeExplosion.PlayOneShot(SoundManager.instance.grenadeExpl);

        Collider[] collider = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in collider)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {

            }
        }
    }

    private void GrenadeEffect()
    {
        GameObject explosionEffect = GlobalReferences.instance.grenadeExplosionEffectl;
        Instantiate(explosionEffect, transform.position, transform.rotation);

        SoundManager.instance.grenadeExplosion.PlayOneShot(SoundManager.instance.grenadeExpl);

        Collider[] collider = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in collider)
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);

            }
        }
    }
}
