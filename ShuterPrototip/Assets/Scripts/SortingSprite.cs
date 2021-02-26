using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingSprite : MonoBehaviour
{
    int BaseOrder = 5000;
    [SerializeField]float ofset = 0f;
    public SpriteRenderer render;
    void Aweke()
    {
        
    }

    
    void LateUpdate()
    {   
        render.sortingOrder = (int)(BaseOrder - transform.position.y * 20 - ofset);
    }
}
