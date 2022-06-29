using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] float health = 100;
    [SerializeField] int scoreValue = 8;

    [Header("Shooting")]
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] float projectileSpeed = 10f;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] string primaryWeaponName = "PrimaryWeapon";
    Transform primaryWeapons;

    [Header("Visual Effects")]
    [SerializeField] GameObject deathVFX;
    [SerializeField] float durationOfExplosion = 0.5f;

    [Header("Sound Effects")]
    [SerializeField] AudioClip[] fireSounds;
    [SerializeField] [Range(0, 1)] float fireSoundVolume = 0.75f;
    [SerializeField] AudioClip[] deathSounds;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        primaryWeapons = transform.Find(primaryWeaponName);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    void Fire()
    {
        //PlayFireSound();

        foreach (Transform child in primaryWeapons)
        {
            var weapon = child.GetComponent<Weapon>();
            if (!weapon) { continue; }
            weapon.ActivateWeapon();
        }
    }

    private void PlayFireSound()
    {
        var randomClipIndex = Random.Range(0, fireSounds.Length);
        AudioSource.PlayClipAtPoint(fireSounds[randomClipIndex], Camera.main.transform.position, fireSoundVolume);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damagedealer = collision.gameObject.GetComponent<DamageDealer>();
        if (!damagedealer) { return; }
        ProcessHit(damagedealer);
    }

    void ProcessHit(DamageDealer damagedealer)
    {
        health -= damagedealer.GetDamage();
        damagedealer.Hit();
        if (health <= 0)
        {
            // drop Item
            var itemDrop = GetComponent<ItemDrop>();
            if (itemDrop)
            {
                itemDrop.DropItem();
            }
            // die
            Die();
        }
    }

    void Die()
    {
        // update score
        FindObjectOfType<GameSession>().AddToScore(scoreValue);
        // play sound
        PlayDeathSound();
        // instantiate explosion
        var explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        explosion.GetComponent<ParticleSystem>().Play();
        Destroy(explosion.gameObject, durationOfExplosion);
        // destroy game object
        Destroy(gameObject);
    }

    private void PlayDeathSound()
    {
        var randomClipIndex = Random.Range(0, deathSounds.Length);
        AudioSource.PlayClipAtPoint(deathSounds[randomClipIndex], Camera.main.transform.position, deathSoundVolume);
    }
}
