using UnityEngine;
using UnityEngine.EventSystems;

public class AudioClicker : MonoBehaviour, IPointerUpHandler
{
    public AudioSource testAudioSource;
    public AudioClip testClip;
    public void OnPointerUp(PointerEventData eventData)
    {
        testAudioSource.clip = testClip;
        testAudioSource.Play();
    }
}
