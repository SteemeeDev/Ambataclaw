using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light lightSource;
    [SerializeField] AudioSource flickerSound;
    [SerializeField] float minTimeToFlicker = 10f;
    [SerializeField] float maxTimeToFlicker = 60f;

    float timeToFlicker;
    float timeSinceLastFlicker = 0;

    public bool flickering = true;

    private void Start()
    {
        timeToFlicker = Random.Range(minTimeToFlicker, maxTimeToFlicker);
    }

    // Update is called once per frame
    void Update()
    {
        if (!flickering) return;

        timeSinceLastFlicker += Time.deltaTime;

        if (timeSinceLastFlicker >= timeToFlicker)
        {
            StartCoroutine(IEFlickerLight(Random.Range(1, 4), false));
            timeSinceLastFlicker = 0;
        }
    }

    public IEnumerator IEFlickerLight(int flickerAmount, bool turnOff)
    {
        yield return new WaitForSeconds(Random.Range(0.01f, 0.8f));
        for (int i = 0; i < flickerAmount; i++)
        {
            if (flickerSound != null  && flickerSound.isActiveAndEnabled)
            {
                flickerSound.Play();
            }
            lightSource.enabled = false;
            yield return new WaitForSeconds(0.1f);
            lightSource.enabled = true;
            yield return new WaitForSeconds(0.3f);
        }
        if (turnOff)
        {
            yield return new WaitForSeconds(0.2f);
            lightSource.enabled = false;
        }
    }
}
