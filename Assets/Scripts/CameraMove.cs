using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public Transform target;

    private Transform tr;

    void Start()
    {
        tr = GetComponent<Transform>();
    }
    
    void LateUpdate()
    {
        tr.position = new Vector3(target.position.x + 1f, target.position.y+15, target.position.z - 22f);
        tr.LookAt(target);
    }
}
