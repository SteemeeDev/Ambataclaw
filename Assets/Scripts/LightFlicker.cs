using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [SerializeField] Light lightSource;
    [SerializeField] float minTimeToFlicker = 10f;
    [SerializeField] float maxTimeToFlicker = 60f;

    float timeToFlicker;
    float timeSinceLastFlicker = 0;

    private void Start()
    {
        timeToFlicker = Random.Range(minTimeToFlicker, maxTimeToFlicker);
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastFlicker += Time.deltaTime;

        if (timeSinceLastFlicker >= timeToFlicker)
        {
            StartCoroutine(IEFlickerLight(Random.Range(1, 4)));
            timeSinceLastFlicker = 0;
        }
    }

    IEnumerator IEFlickerLight(int flickerAmount)
    {
        for (int i = 0; i < flickerAmount; i++)
        {
            lightSource.enabled = false;
            yield return new WaitForSeconds(0.1f);
            lightSource.enabled = true;
            yield return new WaitForSeconds(0.3f);
        }
    }
}
