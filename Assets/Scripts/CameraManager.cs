using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform[] cameraPositions;
    [SerializeField] private float turnTime = 1f;

    public enum CameraPosition
    {
        ArcadeMachine,
        Pipe,
        Arcade
    }
    public CameraPosition camIndex;
    private bool turning;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !turning)
        {
            if ((int)camIndex >= cameraPositions.Length-1) camIndex = 0;
            else camIndex++;

            StartCoroutine(TurnCamera(cameraPositions[(int)camIndex]));
        }

        switch (camIndex)
        {
            case CameraPosition.ArcadeMachine:
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case CameraPosition.Pipe:
                Cursor.lockState = CursorLockMode.None;
                break;
            case CameraPosition.Arcade:
                Cursor.lockState = CursorLockMode.None;
                break;
        }
    }

    IEnumerator TurnCamera(Transform position)
    {
        turning = true;
        Quaternion lastRot = transform.rotation;
        Vector3 lastPos = transform.position;

        float elapsed = 0f;
        while (elapsed < turnTime)
        {
            elapsed += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(lastRot, position.rotation, elapsed / turnTime);
            transform.position = Vector3.Lerp(lastPos, position.position, elapsed / turnTime);
            yield return null;
        }
        turning = false;
    }
}
