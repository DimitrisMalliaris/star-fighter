using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // configuration parameters
    [Header("Player")]
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 0.5f;
    [SerializeField] int health = 200;
    [SerializeField] int maxHealth = 200;

    [Header("Sound Effects")]
    [SerializeField] AudioClip[] deathSounds;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f;

    Coroutine firingCoroutine;

    Vector3 minPos;
    Vector3 maxPos;

    //Weapons
    [SerializeField] string primaryWeaponName = "PrimaryWeapon";
    Transform primaryWeapons;
    [SerializeField] string secondaryWeaponName = "SecondaryWeapon";
    Transform secondaryWeapons;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
        secondaryWeapons = transform.Find(secondaryWeaponName);
        primaryWeapons = transform.Find(primaryWeaponName);
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        minPos = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)) + new Vector3(padding, padding, 0);
        maxPos = gameCamera.ViewportToWorldPoint(new Vector3(1, 1, 0)) - new Vector3(padding, padding, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Tilt();
        Fire();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damageDealer = collision.gameObject.GetComponent<DamageDealer>();
        if (damageDealer) 
        {
            ProcessHit(damageDealer);
            return;
        }

        var healer = collision.gameObject.GetComponent<Healer>();
        if (healer)
        {
            Heal(healer);
            return;
        }
    }

    void ProcessHit(DamageDealer damagedealer)
    {
        // get damage
        int damage = damagedealer.GetDamage();
        // Shake camera
        Camera.main.GetComponent<CameraShake>().Shake((float)damage / maxHealth);
        // substractfromhealth
        health -= damage;
        damagedealer.Hit();
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    void Die()
    {
        FindObjectOfType<SceneLoader>().LoadGameOver();

        PlayDeathSound();

        Destroy(gameObject);
    }

    void PlayDeathSound()
    {
        var randomClipIndex = Random.Range(0, deathSounds.Length);
        AudioSource.PlayClipAtPoint(deathSounds[randomClipIndex], Camera.main.transform.position, deathSoundVolume);

        Debug.Log("You dead");
    }

    void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            foreach (Transform child in primaryWeapons)
            {
                var weapon = child.GetComponent<Weapon>();
                if (!weapon) { continue; }
                weapon.ActivateWeapon();
            }
        }

        if (Input.GetButtonUp("Fire1"))
        {
            foreach (Transform child in primaryWeapons)
            {
                var weapon = child.GetComponent<Weapon>();
                if (!weapon) { continue; }
                weapon.DeactivateWeapon();
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            foreach (Transform child in secondaryWeapons)
            {
                var weapon = child.GetComponent<Weapon>();
                if (!weapon) { continue; }
                weapon.ActivateWeapon();
            }
        }

        if (Input.GetButtonUp("Fire2"))
        {
            foreach (Transform child in secondaryWeapons)
            {
                var weapon = child.GetComponent<Weapon>();
                if (!weapon) { continue; }
                weapon.DeactivateWeapon();
            }
        }
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            foreach (Transform child in primaryWeapons)
            {
                var weapon = child.GetComponent<Weapon>();
                if (!weapon) { continue; }
                weapon.Fire();
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    void Tilt()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.rotation = Quaternion.Euler(0, 25, 0);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            transform.rotation = Quaternion.Euler(0, -25, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, minPos.x, maxPos.x);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, minPos.y, maxPos.y);

        //var newPos = new Vector2(newXPos, transform.position.y);
        var newPos = new Vector3(newXPos,newYPos,transform.position.z);

        transform.position = newPos;
    }

    void Heal(Healer healer)
    {
        var healAmount = healer.GetHealing();
        health = Mathf.Clamp(health + healAmount, health, maxHealth);
        healer.Hit();
    }

    public int GetHealth()
    {
        return health;
    }
}
