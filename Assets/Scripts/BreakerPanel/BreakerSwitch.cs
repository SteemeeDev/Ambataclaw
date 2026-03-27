using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakerSwitch : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer renderer;
    [SerializeField] BoxCollider interactionBox;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip switchSound;
    [SerializeField] float animTime = 0.3f; 
    public bool isOn = true;

    // Triggered by BreakerPanel.cs
    public IEnumerator IEFlipSwitch()
    {
        audioSource.Play();

        float elapsed = 0f;
        while(elapsed < animTime)
        {
            elapsed += Time.deltaTime;
            if (isOn) renderer.SetBlendShapeWeight(0, elapsed / animTime * 100f);
            else renderer.SetBlendShapeWeight(0, (1 - elapsed / animTime) * 100f);
            yield return null;
        }

        isOn = !isOn;
    }
}
