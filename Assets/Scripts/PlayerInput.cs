using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveVAxisName = "Vertical";
    public string moveHAxisName = "Horizontal";
    public string fireLeftButtonName = "FireLeft";
    public string fireRightButtonName = "FireRight";
    
    public float moveV { get; private set; }
    public float moveH { get; private set; }
    public bool fireLeft { get; private set; }
    public bool fireRight { get; private set; }

    public Vector3 mousePos { get; private set; }

    private void Update()
    {
        //if (GameManager.instance != null && GameManager.instance.isgameover)
        //{
        //    movev = 0;
        //    moveh = 0;
        //    fireleft = false;
        //    fireright = false;
        //    mousepos = vector3.zero;
        //    return;
        //}

        moveV = Input.GetAxis(moveVAxisName);
        moveH = Input.GetAxis(moveHAxisName);
        fireLeft = Input.GetButton(fireLeftButtonName);
        fireRight = Input.GetButton(fireRightButtonName);

        mousePos = Input.mousePosition;
    }
}
