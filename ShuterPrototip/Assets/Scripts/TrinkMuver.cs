using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrinkMuver : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    private Rigidbody2D rb;
    [HideInInspector] public Vector2 target;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponent<Health>().Death += OnDeath;
    }
    private void OnDeath()
    {
        GetComponent<Collider2D>().enabled = false;
        enabled = false;   
    }
    void Start()
    {
        //target = rb.position;
    }
    void Update()
    {
        target = GameObject.Find("Caracter").transform.position;
        //if (Input.GetKeyDown(KeyCode.Mouse1))
        //   target = Camera.main.ScreenToWorldPoint(Input.mousePosition);       
    }
    void FixedUpdate()
    {
        rb.position = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
    }
}
