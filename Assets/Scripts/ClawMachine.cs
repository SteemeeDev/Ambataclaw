using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawMachine : MonoBehaviour
{
    [SerializeField] Transform clawModel;
    [SerializeField] ClawHandler claw;

    [SerializeField] float clawSpeed = 0.1f;
    [SerializeField] float clawYSensitivity = 1f;

    [SerializeField] Transform clawBound1;
    [SerializeField] Transform clawBound2;

    [SerializeField] Transform clawBoundY1;
    [SerializeField] Transform clawBoundY2;

    [SerializeField] float xlerp = 0f;
    [SerializeField] float ylerp = 0f;
    [SerializeField] float zlerp = 0f;

    [SerializeField] CameraMove cameraMove;

    bool moveVertical;
    bool hasMovedDown;

    // Update is called once per frame
    void Update()
    {
        if (ylerp >= 0.1f) hasMovedDown = true;

        if (Input.GetMouseButtonDown(0) && ylerp < 0.1f)
        {
            moveVertical = true;
        }
        else if (ylerp < 0.1f && hasMovedDown == true)
        {
            moveVertical = false;
            hasMovedDown = false;
        }


        if (Input.GetMouseButtonDown(1))
        {
            if (!claw.clawIsAnimating)
            {
                if (claw.clawOpen) claw.openingRoutine = StartCoroutine(claw.IEcloseClaw());
                else               claw.openingRoutine = StartCoroutine(claw.IEopenClaw());
            }
        }

        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        if (!moveVertical)
        {
            xlerp -= mouseY * Time.deltaTime * clawSpeed;
            zlerp += mouseX * Time.deltaTime * clawSpeed;
        }
        else if (moveVertical)
        {
            ylerp -= mouseY * Time.deltaTime * clawYSensitivity;
        }

        xlerp = Mathf.Clamp01(xlerp);
        ylerp = Mathf.Clamp01(ylerp);
        zlerp = Mathf.Clamp01(zlerp);

        clawModel.transform.position = new Vector3(
            Mathf.Lerp(clawBound1.position.x, clawBound2.position.x, xlerp),
            clawBoundY1.position.y,
            Mathf.Lerp(clawBound1.position.z, clawBound2.position.z, zlerp)
        );


        claw.transform.position = new Vector3(
            Mathf.Lerp(clawBound1.position.x, clawBound2.position.x, xlerp),
            Mathf.Lerp(clawBoundY1.position.y, clawBoundY2.position.y, ylerp),
            Mathf.Lerp(clawBound1.position.z, clawBound2.position.z, zlerp)
        );
    }
}
