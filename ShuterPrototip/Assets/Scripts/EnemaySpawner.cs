using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemaySpawner : MonoBehaviour
{
    public GameObject enemy;
    void Start()
    {
        StartCoroutine("Spawn");
    }
    void Update()
    {
        
    }
    IEnumerator  Spawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 10f));
            Instantiate(enemy, transform.position + new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f)), Quaternion.identity);
        }
    }
}
