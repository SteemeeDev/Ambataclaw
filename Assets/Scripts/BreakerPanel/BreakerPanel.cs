using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreakerPanel : MonoBehaviour
{
    [SerializeField] LayerMask interactionLayer;
    [SerializeField] BoxCollider interactionBox;
    [SerializeField] Transform breakerBoxCameraPos;
    [SerializeField] CameraManager camManager;
    [SerializeField] ClawMachine clawMachine;
    [SerializeField] GameObject breakerPanelUI;
    [SerializeField] BreakerSwitch[] breakerSwitches;

    [Header("LIGHT SETTINGS")]
    [SerializeField] LightFlicker[] affectedLights;
    [SerializeField] float minTimeToGoOut = 10f;
    [SerializeField] float maxTimeToGoOut = 80f;

    public bool allSwitchesOn = true;
    bool previousSwitchState = true;

    bool uiOpen;
    float timeSinceLastLightOut = 0f;
    float targetTime;

    private void Start()
    {
        targetTime = Random.Range(minTimeToGoOut, maxTimeToGoOut);
    }
    private void Update()
    {

        // Random flipping of breakers
        timeSinceLastLightOut += Time.deltaTime;
        if (timeSinceLastLightOut >= targetTime)
        {
            targetTime = Random.Range(minTimeToGoOut, maxTimeToGoOut);
            timeSinceLastLightOut = 0;

            for (int i = 0; i < breakerSwitches.Length; i++)
            {
                if (Random.Range(0, 2) == 0)
                {
                    breakerSwitches[i].StartCoroutine(breakerSwitches[i].IEFlipSwitch());
                }
            }
        }

        // Turning on or off lights
        previousSwitchState = allSwitchesOn;

        foreach (BreakerSwitch breakerSwitch in breakerSwitches)
        {
            if (!breakerSwitch.isOn)
            {
                allSwitchesOn = false;
                break;
            }

            allSwitchesOn = true;
        }

        
        if (previousSwitchState != allSwitchesOn)
        {
            foreach (LightFlicker light in affectedLights)
            {
                light.StartCoroutine(light.IEFlickerLight(Random.Range(1, 4), !allSwitchesOn));
            }
        }

        // UI and camera management
        if (camManager.camIndex != CameraManager.CameraPosition.Pipe) return;


        if (Input.GetMouseButtonDown(0) && !uiOpen)
        {
            Ray ray = camManager.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);


            if (Physics.Raycast(ray, Mathf.Infinity, interactionLayer))
            {
                camManager.holdSpecialCameraPosition = true;
                StartCoroutine(camManager.TurnCamera(breakerBoxCameraPos));
                breakerPanelUI.SetActive(true);
                uiOpen = true;
                interactionBox.enabled = false;
            }
        }
        else if (Input.GetMouseButtonDown(0) && uiOpen)
        {
            Ray ray = camManager.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactionLayer))
            {
                BreakerSwitch breakerSwitch = hit.transform.GetComponent<BreakerSwitch>();
                if (breakerSwitch != null)
                {
                    breakerSwitch.StartCoroutine(breakerSwitch.IEFlipSwitch());
                }
            }
        }

    }
    // Triggered when clicking ui "Back button"
    public void LeaveBreakerPanel()
    {
        camManager.holdSpecialCameraPosition = false;
        StartCoroutine(camManager.TurnCamera(camManager.cameraPositions[(int)camManager.camIndex]));
        breakerPanelUI.SetActive(false);
        uiOpen = false;
        interactionBox.enabled = true;
    }
}
