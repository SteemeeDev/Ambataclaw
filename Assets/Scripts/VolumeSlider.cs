using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;
    [SerializeField] AudioMixer mainMixer;

    public void UpdateVolume()
    {
        mainMixer.SetFloat("MASTERVOLUME", volumeSlider.value);
    }
}
