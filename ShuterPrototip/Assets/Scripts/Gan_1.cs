using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gan_1 : MonoBehaviour
{
    [SerializeField] private float reytOfFire = 700;
    [SerializeField] private float spread = 5f;
    public GameObject bullet;
    public Transform firePoint;
    public SpriteRenderer sRFlesh;
    private AudioSource shootSaund;
    private float flashDelay = 0.05f;
    private float timer;
    private PlayerInpyt playerInpyt;
    private void Awake()
    {
        shootSaund = GetComponent<AudioSource>();
        playerInpyt = FindObjectOfType<PlayerInpyt>();
    }
    private void Update()
    {
        if (transform?.parent && transform.parent.gameObject.name == "Hands")
            playerInpyt.Fired += OnFired;
    }
   
    private void OnDisable()
    {
        if (playerInpyt != null)
            playerInpyt.Fired -= OnFired;
    }
    private IEnumerator Flash(float time)
    {
        sRFlesh.enabled = true;
        yield return new WaitForSeconds(time);
        sRFlesh.enabled = false;
    }
    private void OnFired()
    {
        if (Time.time > timer)
        {
            timer = Time.time + (1/(reytOfFire/60));
            if (sRFlesh != null)
                StartCoroutine(Flash(flashDelay));
            
            Instantiate(bullet, firePoint.position, firePoint.rotation).GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(100f, Random.Range(-spread*2, spread*2)));
            shootSaund.Play();
        }
    }
}
