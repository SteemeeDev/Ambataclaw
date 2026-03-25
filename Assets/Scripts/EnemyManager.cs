using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    [SerializeField] Transform[] travelPoints;
    [SerializeField] float travelSpeed = 1f;

    float travelPercentage = 0f;

    private void Update()
    {
        travelPercentage += travelSpeed * Time.deltaTime;
        transform.position = Vector3.Lerp(travelPoints[0].position, travelPoints[1].position, travelPercentage / 100f);
    }
}
