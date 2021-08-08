using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class o_billboard : MonoBehaviour
{
    public Camera cam;
    void Update()
    {
        transform.LookAt(cam.transform.position, Vector3.up);   
    }
}
