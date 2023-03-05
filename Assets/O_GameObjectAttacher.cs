using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_GameObjectAttacher : MonoBehaviour
{
    public GameObject attach;
    void Update()
    {
        transform.position = attach.transform.position;
    }
}
