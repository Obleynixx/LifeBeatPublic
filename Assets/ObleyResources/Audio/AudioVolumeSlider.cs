using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolumeSlider : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;

    void Start()
    {
        if (volumeSlider != null)
        {
            volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.75f);
            SetVolume(volumeSlider.value);
        }
        
    }

    public void SetVolume(float volume)
    {
        if (volume < 0.05f)
        {
            audioMixer.SetFloat("MasterVolume", -80f);
            return;
        }

        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
}
