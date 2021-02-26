using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheractorConroller : MonoBehaviour
{
    public Sprite targetPointSprite;
    public float moveSpeed = 1;
    private Rigidbody2D rB;
    private GameObject targetPoint;
    void Start()
    {
        rB = GetComponent<Rigidbody2D>();
        CreateTargetPoint();
    }
    void Update()
    {      
        targetPoint.transform.position = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
    }
    private void FixedUpdate()
    {
        rB.MovePosition(rB.position + new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * moveSpeed * Time.fixedDeltaTime);
        rB.rotation = Mathf.Atan2(Camera.main.ScreenToWorldPoint(Input.mousePosition).y - rB.position.y, Camera.main.ScreenToWorldPoint(Input.mousePosition).x - rB.position.x) * Mathf.Rad2Deg - 90f;
    }
    private void CreateTargetPoint()
    {
        Cursor.visible = false;
        targetPoint = new GameObject();
        targetPoint.name = "TargetPoint";
        targetPoint.AddComponent<SpriteRenderer>().sprite = targetPointSprite;
        targetPoint.GetComponent<SpriteRenderer>().sortingOrder = 10;
    }
}
