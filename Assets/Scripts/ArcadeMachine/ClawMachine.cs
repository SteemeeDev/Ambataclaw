using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class ClawMachine : MonoBehaviour
{
    [SerializeField] Transform clawModel;
    
    [Header("CLAW SETTINGS")]
    [SerializeField] float clawSpeed = 0.1f;
    [SerializeField] float clawYSensitivity = 1f;

    [SerializeField] Transform clawBound1;
    [SerializeField] Transform clawBound2;

    [SerializeField] Transform clawBoundY1;
    [SerializeField] Transform clawBoundY2;

    [SerializeField] float xlerp = 0f;
    [SerializeField] float ylerp = 0f;
    [SerializeField] float zlerp = 0f;

    [Header("HANDLERS")]
    [SerializeField] ClawHandler clawHandler;
    [SerializeField] CameraManager camManager;
    [SerializeField] ButtonHandler buttonHandler;
    [SerializeField] BreakerPanel breakerPanel;

    [SerializeField] AudioSource audioPlayer;
    [SerializeField] AudioSource switchAxisSound;

    [SerializeField] Slider horizontalSensSlider;
    [SerializeField] Slider verticalSensSlider;


    bool moveVertical;
    bool hasMovedDown;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (camManager.camIndex != CameraManager.CameraPosition.ArcadeMachine) return;
        if (breakerPanel.allSwitchesOn == false)
        {
            return;
        }

        if (ylerp >= 0.1f) hasMovedDown = true;

        if (Input.GetMouseButtonDown(0) && ylerp < 0.1f)
        {
            buttonHandler.StartCoroutine(buttonHandler.downButton.IEPressButton());
            moveVertical = true;
        }
        else if (ylerp < 0.1f && hasMovedDown == true)
        {
            buttonHandler.StartCoroutine(buttonHandler.downButton.IEPressButton());
            moveVertical = false;
            hasMovedDown = false;
            switchAxisSound.pitch = Random.Range(0.9f, 1.0f);
            switchAxisSound.Play();
        }


        if (Input.GetMouseButtonDown(1))
        {
            if (!clawHandler.clawIsAnimating)
            {
                buttonHandler.StartCoroutine(buttonHandler.grabButton.IEPressButton());

                if (clawHandler.clawOpen)
                {
                    clawHandler.openingRoutine = StartCoroutine(clawHandler.IEcloseClaw());
                }
                else
                {
                    clawHandler.openingRoutine = StartCoroutine(clawHandler.IEopenClaw());
                }
            }
        }

        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        if (!moveVertical)
        {
            xlerp -= mouseY * Time.deltaTime * clawSpeed;
            zlerp += mouseX * Time.deltaTime * clawSpeed;

            if (mouseX != 0 || mouseY != 0)
            {
                buttonHandler.TurnJoyStick(new Vector2(mouseX, mouseY));
            }
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

        clawHandler.transform.position = new Vector3(
            Mathf.Lerp(clawBound1.position.x, clawBound2.position.x, xlerp),
            Mathf.Lerp(clawBoundY1.position.y, clawBoundY2.position.y, ylerp),
            Mathf.Lerp(clawBound1.position.z, clawBound2.position.z, zlerp)
        );
    }

    public void UpdateSensitivity()
    {
        clawYSensitivity = verticalSensSlider.value;
        clawSpeed = horizontalSensSlider.value;
    }

    public IEnumerator IEVolumeFade(float targetVolume, float duration)
    {
        float startVolume = audioPlayer.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            audioPlayer.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
            audioPlayer.pitch = Mathf.Lerp(startVolume, targetVolume, elapsed / duration); // Optional: also lower the pitch for a more dramatic effect
            yield return null;
        }

        audioPlayer.volume = targetVolume;
        audioPlayer.pitch = targetVolume;
    }
}
