using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawHandler : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer clawRenderer;
    [SerializeField] float closeTime = 0.5f;
    public bool clawOpen;
    public bool clawIsAnimating;

    public Coroutine openingRoutine;

    float elapsed;
    public IEnumerator IEcloseClaw()
    {
        clawIsAnimating = true;
        clawOpen = false;

        elapsed = 0f;

        while (elapsed < closeTime)
        {
            elapsed += Time.deltaTime;

            float blend = Mathf.Lerp(0f, 100f, elapsed / closeTime);

            clawRenderer.SetBlendShapeWeight(0, blend);

            yield return null;
        }

        clawIsAnimating = false;
    }
    public IEnumerator IEopenClaw()
    {
        clawIsAnimating = true;
        clawOpen = true;

        elapsed = 1f;

        while (elapsed > 0)
        {
            elapsed -= Time.deltaTime;

            float blend = Mathf.Lerp(0f, 100f, elapsed / closeTime);

            clawRenderer.SetBlendShapeWeight(0, blend);

            yield return null;
        }

        clawIsAnimating = false;
    }
}
