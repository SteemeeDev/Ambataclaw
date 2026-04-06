using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Transform[] travelPoints;
    [SerializeField] GameObject enemyRenderer;
    [SerializeField] float maxTravelSpeed = 1f;
    float travelSpeed;

    public float travelPercentage = 0f;

    private void Start()
    {
        travelSpeed = maxTravelSpeed;
    }

    private void Update()
    {
        travelPercentage += travelSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(travelPoints[0].position, travelPoints[1].position, travelPercentage / 100f);
    }

    // Triggered by CameraManager.cs
    public IEnumerator EnemySpotted()
    {
        enemyRenderer.SetActive(true);
        travelSpeed = 0;

        yield return new WaitForSeconds(1f);

        enemyRenderer.SetActive(false);
        travelPercentage *= 0.4f;
        travelSpeed = maxTravelSpeed;
    }
}
