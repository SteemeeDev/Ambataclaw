using System.Collections;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] Transform[] stage1Points;
    [SerializeField] Transform[] stage2Points;
    [SerializeField] Transform[] stage3Points;
    [SerializeField] Transform[] stage4Points;
    int travelStage = -1;


    [SerializeField] GameObject enemyRenderer;
    [SerializeField] AudioSource audioPlayer;
    [SerializeField] AudioClip spotSound;
    [SerializeField] AudioClip moveSound;

    [SerializeField] float maxTravelSpeed = 1f;
    float travelSpeed;
    public float travelPercentage = 0f;


    [SerializeField] float spotCooldown = 20f;
    float timeSinceLastSpot;

    private void Start()
    {
        travelSpeed = maxTravelSpeed;
    }

    private void Update()
    {
        travelPercentage += travelSpeed * Time.deltaTime;
      
        ChooseTravelStage();

        timeSinceLastSpot += Time.deltaTime;

        if (timeSinceLastSpot > spotCooldown && enemyRenderer.activeSelf == false)
        {
            enemyRenderer.SetActive(true);
        }
    }

    void ChooseTravelStage()
    {
        if (travelPercentage > 0f && travelPercentage < 25f && travelStage != 0)
        {
            travelStage = 0;
            transform.position = stage1Points[Random.Range(0, stage1Points.Length)].position;
        }
        else if (travelPercentage > 25f && travelPercentage < 50f && travelStage != 1)
        {
            travelStage = 1;
            transform.position = stage2Points[Random.Range(0, stage1Points.Length)].position;
        }
        else if (travelPercentage > 50f && travelPercentage < 75f && travelStage != 2)
        {
            travelStage = 2;
            transform.position = stage3Points[Random.Range(0, stage1Points.Length)].position;
            audioPlayer.PlayOneShot(moveSound);
        }
        else if (travelPercentage > 75f && travelPercentage < 100f && travelStage != 3)
        {
            travelStage = 3;
            transform.position = stage4Points[Random.Range(0, stage1Points.Length)].position;
            audioPlayer.PlayOneShot(moveSound);
        }
    }
    // Triggered by CameraManager.cs
    public IEnumerator EnemySpotted()
    {
        if (timeSinceLastSpot > spotCooldown)
        {
            timeSinceLastSpot = 0;
            enemyRenderer.SetActive(true);
            travelSpeed = 0;

            yield return new WaitForSeconds(1f);
            audioPlayer.PlayOneShot(spotSound);

            enemyRenderer.SetActive(false);
            travelSpeed = maxTravelSpeed;
            travelPercentage -= spotCooldown * maxTravelSpeed * 1.5f;
            travelPercentage = Mathf.Clamp(travelPercentage, 0f, 100f);

            travelStage = -1; // Make the enemy choose another stage
        }
    }
}
