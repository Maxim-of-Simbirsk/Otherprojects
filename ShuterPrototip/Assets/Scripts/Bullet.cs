using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] int damage = 1;
    public GameObject blud;
    void Start()
    {
        Destroy(gameObject, 1f);
    }
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HitBox")
        {
            Instantiate(blud, transform.position, transform.rotation);
            collision.gameObject.GetComponentInParent<Health>().TakDamage(damage);
        }
        Destroy(gameObject);
    }
}
