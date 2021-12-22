using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; 

public class MouseLook : MonoBehaviourPunCallbacks
{
    public static bool cursorLocked = true; 

    public Transform player;
    public Transform cams; 
    public Transform weapon; 

    public float xSens; 
    public float ySens; 
    public float maxAngle; 

    private Quaternion camCenter;	

    void Start()
    {
        camCenter = cams.localRotation; 
    }

    void Update()
    {
        if(!photonView.IsMine) return;

        SetY();
        SetX();

        UpdateCursorLocked();
    }

    void SetY ()
    {
        float mouseY = Input.GetAxis("Mouse Y") * ySens * Time.deltaTime;
        Quaternion adj = Quaternion.AngleAxis(mouseY, -Vector3.right);
        Quaternion delta = cams.localRotation * adj; 

        if(Quaternion.Angle(camCenter, delta) < maxAngle)
        {
            cams.localRotation = delta; 
        } 

        weapon.rotation = cams.rotation; 
    }

    void SetX ()
    {
        float mouseX = Input.GetAxis("Mouse X") * xSens * Time.deltaTime;
        Quaternion adj = Quaternion.AngleAxis(mouseX, Vector3.up);
        Quaternion delta = player.localRotation * adj; 
        player.localRotation = delta; 
    }

    void UpdateCursorLocked ()
    {
        if(cursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked; 
            Cursor.visible = false; 

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                cursorLocked = false; 
            }
        }
        else 
        {
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true; 

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                cursorLocked = true; 
            }
        }
    }

}
