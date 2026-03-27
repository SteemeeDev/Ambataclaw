using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform[] cameraPositions;
    [SerializeField] private float turnTime = 1f;
    [SerializeField] ButtonHandler buttonHandler;

    public bool holdSpecialCameraPosition;

    public enum CameraPosition
    {
        ArcadeMachine,
        Pipe,
        Arcade,
        Shelf
    }
    public CameraPosition camIndex;
    private bool turning;

    private void Update()
    {
        if (!turning && Input.GetAxisRaw("Horizontal") != 0 && !holdSpecialCameraPosition)
        {
            camIndex += (int)Input.GetAxisRaw("Horizontal");

            if ((int)camIndex >= cameraPositions.Length) camIndex = 0;
            else if ((int)camIndex < 0) camIndex = CameraPosition.Shelf;

            StartCoroutine(TurnCamera(cameraPositions[(int)camIndex]));
        }

    }

    public IEnumerator TurnCamera(Transform position)
    {
        Debug.Log("Truning camera!");
        switch (camIndex)
        {
            case CameraPosition.ArcadeMachine:
                Cursor.lockState = CursorLockMode.Locked;
                buttonHandler.StartCoroutine(buttonHandler.startButton.IEPressButton());
                break;
            case CameraPosition.Pipe:
                Cursor.lockState = CursorLockMode.None;
                break;
            case CameraPosition.Arcade:
                Cursor.lockState = CursorLockMode.None;
                break;
            case CameraPosition.Shelf:
                Cursor.lockState = CursorLockMode.None;
                break;
        }

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
