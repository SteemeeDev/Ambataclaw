using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] CameraManager camManager;
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] Light flashLightLight;
    Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
    }
    RaycastHit hit;
    void Update()
    {
        if (camManager.camIndex != CameraManager.CameraPosition.Arcade)
        {
            flashLightLight.enabled = false;
            return;
        }
        flashLightLight.enabled = true;
        if (Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity)){
            transform.LookAt(hit.point);

            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                enemyManager.StartCoroutine(enemyManager.EnemySpotted());
            }
        }
    }
}
