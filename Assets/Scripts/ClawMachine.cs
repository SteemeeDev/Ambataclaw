using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawMachine : MonoBehaviour
{
    [SerializeField] Transform clawModel;
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

    // Update is called once per frame
    void Update()
    {
        float inputZ = Input.GetAxisRaw("Horizontal");
        float inputX = Input.GetAxisRaw("Vertical");
        float mouseY = Input.GetAxisRaw("Mouse Y");


        xlerp -= inputX * Time.deltaTime * clawSpeed;
        zlerp += inputZ * Time.deltaTime * clawSpeed;
        ylerp -= mouseY * Time.deltaTime * clawYSensitivity;

        xlerp = Mathf.Clamp01(xlerp);
        ylerp = Mathf.Clamp01(ylerp);
        zlerp = Mathf.Clamp01(zlerp);

        clawModel.transform.position = new Vector3( 
            Mathf.Lerp(clawBound1.position.x, clawBound2.position.x, xlerp),
            Mathf.Lerp(clawBoundY1.position.y, clawBoundY2.position.y, ylerp),
            Mathf.Lerp(clawBound1.position.z, clawBound2.position.z, zlerp)
        );


    }
}
