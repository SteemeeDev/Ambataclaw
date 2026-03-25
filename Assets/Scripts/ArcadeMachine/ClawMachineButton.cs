using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawMachineButton : MonoBehaviour
{
    Vector3 startPos;
    bool isAnimating = false;

    private void Start()
    {
        startPos = transform.localPosition;
    }

    public IEnumerator IEPressButton()
    {
        if (isAnimating == true) yield break;
        isAnimating = true;
        Vector3 targetPos = transform.localPosition + new Vector3(0, -0.01f, 0);

        float elapsedTime = 0f;
        float pressDuration = 0.2f;
        while (elapsedTime < pressDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startPos, targetPos, elapsedTime / pressDuration);
            yield return null;
        }

        StartCoroutine(IELetGoOfButton());
    }

    public IEnumerator IELetGoOfButton()
    {
        Vector3 newStartPos = transform.localPosition;

        float elapsedTime = 0f;
        float pressDuration = 0.1f;
        while (elapsedTime < pressDuration)
        {
            elapsedTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(newStartPos, startPos, elapsedTime / pressDuration);
            yield return null;
        }
        isAnimating = false;
    }
}
