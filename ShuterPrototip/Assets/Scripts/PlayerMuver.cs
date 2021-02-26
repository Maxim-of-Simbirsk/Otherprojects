using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMuver : MonoBehaviour
{
    private PlayerInpyt playerInput;  
    private Rigidbody2D rb;
    [SerializeField] private float speedWalk = 2f;
    //[SerializeField] private float speedRun= 4f;
    public Transform hands;
    public SpriteRenderer sR;
    public Animator anim;  
    private bool flip;
    public GameObject targetPoint;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInpyt>();       
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        CreateTargetPoint();
    }
    private void Update()
    {
        float rotataHandsZ = Mathf.Atan2(playerInput.mausPosition.y - hands.transform.position.y - 0.1f, playerInput.mausPosition.x - hands.transform.position.x) * Mathf.Rad2Deg;
        hands.transform.rotation = Quaternion.Euler(0f, 0f, rotataHandsZ);
        if (rb.velocity.x > 0.1f || rb.velocity.y > 0.1f || rb.velocity.x < -0.1f || rb.velocity.y < -0.1f) anim.SetFloat("Ran", 1f);
        else anim.SetFloat("Ran", 0f);
        if (playerInput.mausPosition.x - transform.position.x < 0 && !flip)
        {
            hands.transform.localScale = new Vector3(1f, -1f, 1f);
            sR.flipX = true;
            flip = true;
        }
        else if (playerInput.mausPosition.x - transform.position.x > 0 && flip)
        {
            hands.transform.localScale = new Vector3(1f, 1f, 1f);
            sR.flipX = false;
            flip = false;
        }

        if (rotataHandsZ > 30f && rotataHandsZ < 150f)
        {
            for (int i = 0; i < transform.GetChild(1).GetChild(0).childCount; i++)
            {
                if (i == 0) transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = sR.sortingOrder - 4;
                else
                transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = sR.sortingOrder - 3;
            }
            anim.SetBool("Front", false);
        }
        else
        {
            for (int i = 0; i < transform.GetChild(1).GetChild(0).childCount; i++)
            {
                if (i==0) transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = sR.sortingOrder + 3;
                else
                transform.GetChild(1).GetChild(0).GetChild(i).GetComponent<SpriteRenderer>().sortingOrder = sR.sortingOrder + 4;
            }
            anim.SetBool("Front", true);
        }
        targetPoint.transform.position = playerInput.mausPosition;
    }               
    private void FixedUpdate()
    {
        rb.AddForce(playerInput.playerDirektion.normalized * speedWalk * Time.fixedDeltaTime);
    }
    private void CreateTargetPoint()
    {
        Cursor.visible = false;
        targetPoint = Instantiate(targetPoint, playerInput.mausPosition, Quaternion.identity);
        
    }
}
