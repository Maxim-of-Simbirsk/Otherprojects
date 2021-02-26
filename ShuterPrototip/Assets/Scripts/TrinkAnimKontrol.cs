using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinkAnimKontrol : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private TrinkMuver trinkMuver;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        trinkMuver = GetComponent<TrinkMuver>();
        GetComponent<Health>().Death += OnDeath;
    }
    private void OnDeath()
    {
        anim.SetBool("Dead", true);
        Destroy(gameObject, 25f);
       // enabled = false;
    }
    void Update()
    {
        if (Vector2.Distance(rb.position, trinkMuver.target) > 0.5f)
        {
            float vektiorTarget = Mathf.Atan2(trinkMuver.target.x - rb.position.x, trinkMuver.target.y - rb.position.y) * Mathf.Rad2Deg;
            anim.SetBool("Idle", false);
            if ((vektiorTarget < 45f && vektiorTarget > 0f) || (vektiorTarget > -45f && vektiorTarget < 0f))
            {
                anim.SetFloat("Vertikal", 1f);
                anim.SetFloat("Horizontal", -0.1f);
            }
            else if ((vektiorTarget > 135f && vektiorTarget < 180f) || (vektiorTarget < -135f && vektiorTarget > -180f))
            {
                anim.SetFloat("Vertikal", -1f);
                anim.SetFloat("Horizontal", -0.1f);

            }
            else if (vektiorTarget < -45f && vektiorTarget > -135f)
            {
                anim.SetFloat("Vertikal", 0f);
                anim.SetFloat("Horizontal", -1f);
                sr.flipX = false;
            }
            else if (vektiorTarget > 45f && vektiorTarget < 135f)
            {
                anim.SetFloat("Vertikal", 0f);
                anim.SetFloat("Horizontal", -1f);
                sr.flipX = true;
            }
        }       
        else
            anim.SetBool("Idle", true);


    }
}
