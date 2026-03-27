using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Transform[] travelPoints;
    [SerializeField] Renderer enemyRenderer;
    [SerializeField] CameraManager camManager;
    [SerializeField] float maxTravelSpeed = 1f;
    float travelSpeed;

    float travelPercentage = 0f;

    private void Start()
    {
        travelSpeed = maxTravelSpeed;
    }

    private void Update()
    {
        travelPercentage += travelSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(travelPoints[0].position, travelPoints[1].position, travelPercentage / 100f);
    }

    private IEnumerator OnBecameVisible()
    {
        enemyRenderer.enabled = true;
        travelSpeed = 0;

        yield return new WaitForSeconds(2f);

        enemyRenderer.enabled = false;
    }

    private void OnBecameInvisible()
    {
        travelPercentage *= 0.4f;
        travelSpeed = maxTravelSpeed;
    }
}
