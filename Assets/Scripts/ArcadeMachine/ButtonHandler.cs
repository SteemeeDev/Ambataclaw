using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ButtonHandler : MonoBehaviour
{
    [SerializeField] Transform joystick;
    public ClawMachineButton startButton;
    public ClawMachineButton downButton;
    public ClawMachineButton grabButton;

    public void TurnJoyStick(Vector2 moveDir)
    {
        moveDir = moveDir.normalized;

        float tiltAngle = 30f;
        Quaternion targetRot = Quaternion.Euler(moveDir.x * tiltAngle, 0f, moveDir.y * tiltAngle);

        joystick.rotation = Quaternion.Slerp(joystick.rotation, targetRot, Time.deltaTime * 5f);
    }
}
