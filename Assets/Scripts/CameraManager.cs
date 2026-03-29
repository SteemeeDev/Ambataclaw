using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform[] cameraPositions;
    [SerializeField] private float turnTime = 1f;
    private float _turnTime; // Actual turn time, can be modified by events
    [SerializeField] ButtonHandler buttonHandler;
    [SerializeField] EnemyManager enemyManager;

    public bool holdSpecialCameraPosition;

    public enum CameraPosition
    {
        ArcadeMachine,
        Pipe,
        FromPipe,
        Arcade,
        FromShelf,
        Shelf
    }
    public CameraPosition camIndex;

    private CameraPosition previousCameraPosition;
    private bool turning;

    private void Update()
    {
        if (!turning && Input.GetAxisRaw("Horizontal") != 0 && !holdSpecialCameraPosition)
        {
            previousCameraPosition = camIndex;
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
                _turnTime = turnTime;
                break;
            case CameraPosition.Pipe:
                Cursor.lockState = CursorLockMode.None;
                _turnTime = turnTime;
                break;
            case CameraPosition.FromPipe:
                Cursor.lockState = CursorLockMode.None;
                _turnTime = turnTime * 0.5f;
                break;
            case CameraPosition.Arcade:
                Cursor.lockState = CursorLockMode.None;
                enemyManager.StartCoroutine(enemyManager.EnemySpotted());
                _turnTime = turnTime * 0.5f;
                break;
            case CameraPosition.FromShelf:
                Cursor.lockState = CursorLockMode.None;
                _turnTime = turnTime * 0.5f;
                break;
            case CameraPosition.Shelf:
                Cursor.lockState = CursorLockMode.None;
                _turnTime = turnTime;
                break;
        }

        turning = true;
        Quaternion lastRot = transform.rotation;
        Vector3 lastPos = transform.position;

        float elapsed = 0f;
        while (elapsed < _turnTime)
        {
            elapsed += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(lastRot, position.rotation, elapsed / _turnTime);
            transform.position = Vector3.Lerp(lastPos, position.position, elapsed / _turnTime);
            yield return null;
        }
        turning = false;

        if ((camIndex == CameraPosition.FromPipe || camIndex == CameraPosition.FromShelf) && previousCameraPosition != CameraPosition.Arcade)
        {
            camIndex = CameraPosition.Arcade;
            StartCoroutine(TurnCamera(cameraPositions[(int)CameraPosition.Arcade]));
        }
        else if (previousCameraPosition == CameraPosition.Arcade)
        {
            if (camIndex == CameraPosition.FromPipe)
            {
                camIndex = CameraPosition.Pipe;
                StartCoroutine(TurnCamera(cameraPositions[(int)CameraPosition.Pipe]));
            }
            else if (camIndex == CameraPosition.FromShelf)
            {
                camIndex = CameraPosition.Shelf;
                StartCoroutine(TurnCamera(cameraPositions[(int)CameraPosition.Shelf]));
            }
        }
    }
}
