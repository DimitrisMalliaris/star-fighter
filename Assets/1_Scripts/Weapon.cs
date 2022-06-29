using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float cooldownTimeInSeconds = 5f;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] GameObject projectilePrefab;

    [SerializeField] AudioClip[] fireSounds;
    [SerializeField] [Range(0, 1)] float fireSoundVolume = 0.75f;
    [SerializeField] bool isAutomatic = false;
    [SerializeField] bool isFiring = false;

    Coroutine firingCoroutine;

    float cooldownCountDown = 0f;

    public void Fire()
    {
        if (Time.realtimeSinceStartup - cooldownCountDown >= cooldownTimeInSeconds)
        {
            cooldownCountDown = Time.realtimeSinceStartup;
            FireProjectile();
        }
    }

    private void FireProjectile()
    {
        var projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(projectileSpeed, projectileSpeed) * projectile.transform.up;
        PlayFireSound();
    }

    public void ActivateWeapon()
    {
        if (isAutomatic)
        {
            isFiring = true;
            var previousCoroutine = firingCoroutine;
            firingCoroutine = StartCoroutine(FireContinuously(previousCoroutine));
        }
        else
        {
            Fire();
        }
    }

    public void DeactivateWeapon()
    {
        if (isAutomatic)
        {
            isFiring = false;
        }
    }

    private IEnumerator FireContinuously(Coroutine previousCoroutine)
    {
        yield return previousCoroutine;
        while (isFiring)
        {
            FireProjectile();
            yield return new WaitForSeconds(cooldownTimeInSeconds);
        }
        //yield return new WaitForSeconds(cooldownTimeInSeconds);
    }

    private void PlayFireSound()
    {
        if (fireSounds.Length == 0) { return; }
        var randomClipIndex = Random.Range(0, fireSounds.Length);
        AudioSource.PlayClipAtPoint(fireSounds[randomClipIndex], transform.position, fireSoundVolume);
    }
}
