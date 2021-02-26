using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FildOfView2D : MonoBehaviour
{
    public LayerMask layerMask;
    [Range(0f, 40f)] public float dist = 5f;
    [Range(1f, 360f)] public float sector = 30f;
    [Range(2, 720)] public int Rays = 2;
    private Mesh mesh;
    private float meshHeight = 0f; //высота отрисовки меша
    public enum View
    {
        All,
        Fild,
        Rays
    }
    public View view;
    void Start()
    {
        mesh = new Mesh(); 
        GetComponent<MeshFilter>().mesh = mesh; 

    }
    void Update()
    {
        MeshDrow();

    }
    void MeshDrow()
    {
        Rays = Mathf.Max(Rays, 2); 
        sector = Mathf.Clamp(sector, 1f, 360f); 

        Vector3[] vertices = new Vector3[Rays + 1]; 
        vertices[0] = new Vector3(0, 0, meshHeight); //начальная точка меша относительно родительского объекта
        float ugolS = (sector / 2) * Mathf.Deg2Rad; //смещаем сектор обзора на половину в сторону взгляда
        float ugolR = 0; 

        for (int i = 0; i < Rays; i++) //генерируем точки меша
        {
            float x = Mathf.Sin(ugolR - ugolS) * dist;
            float y = Mathf.Cos(ugolR - ugolS) * dist;
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, transform.TransformDirection(x, y, 0), dist, layerMask);
            if (raycastHit2D.collider != null)
            {
                vertices[i + 1] = transform.InverseTransformPoint(raycastHit2D.point);
                if (view != View.Fild)
                    Debug.DrawLine(transform.position, raycastHit2D.point, Color.red);
            }
            else
            {
                vertices[i + 1] = new Vector3(x, y, meshHeight); 
                if (view != View.Fild) Debug.DrawRay(transform.position, transform.TransformDirection(x, y, 0), Color.green);
            }
            ugolR += sector / (Rays - 1) * Mathf.Deg2Rad;      
        }
        int[] triangles = new int[(Rays - 1) * 3]; 
        for (int i = 0; i < (Rays - 1) * 3; i += 3) 
        {
            triangles[i] = 0;
            triangles[i + 1] = i / 3 + 1;
            triangles[i + 2] = i / 3 + 2;
        }
        if (view != View.Rays) GetComponent<MeshRenderer>().enabled = true;
        else GetComponent<MeshRenderer>().enabled = false;

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.Optimize();
        mesh.RecalculateNormals();

    }
}
