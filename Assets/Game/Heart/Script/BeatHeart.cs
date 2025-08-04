using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class BeatHeart : MonoBehaviour
{
    public SpriteRenderer heart;
    public SpriteRenderer ekgBeep; //workaround
    public float heartAnimScale = 1.5f;
    public float heartAnimDuration = 0.5f;
    public AudioSource audioSource;
    public AudioClip beatClip;
    //I need to change scale here so animation keeps relative while person gets older
    bool isBeating;
    public UnityEvent OnHeartBeat;

    public Sprite nextSprite;

    Sequence beatSequence;

    [ContextMenu("Beat")]
    public void Beat()
    {
        if (beatClip == null)
        {
            return;
        }
        if (isBeating)
            return;
        if (beatSequence != null)
        {
            beatSequence.Kill();
            beatSequence = null;
        }
        if (GameManager.Instance.GetGameSequenceName() == "OldAgeEKG")
        {
            ekgBeep.gameObject.SetActive(true);
            ekgBeep.sortingOrder = 4;
            beatSequence = DOTween.Sequence();
            beatSequence.Append(ekgBeep.DOFade(1, heartAnimDuration));
            beatSequence.Append(ekgBeep.DOFade(0, heartAnimDuration / 1.5f));
            beatSequence.OnComplete(() => { isBeating = false; });
            beatSequence.Play();
            isBeating = true;
            audioSource.PlayOneShot(beatClip);
            OnHeartBeat?.Invoke();
            return;
        }
        else if (GameManager.Instance.GetGameSequenceName() == "Adolescence School")
        {
            ekgBeep.gameObject.SetActive(true);
            ekgBeep.sortingOrder = 2;
        }
        else if (ekgBeep.gameObject.activeSelf != false)
        {
            ekgBeep.gameObject.SetActive(false);
            ekgBeep.sprite = nextSprite;
        }
        heart.transform.localScale = Vector3.one;

        beatSequence = DOTween.Sequence();

        beatSequence.Append(heart.transform.DOScale(Vector3.one * heartAnimScale, heartAnimDuration));

        beatSequence.Append(heart.transform.DOScale(Vector3.one, heartAnimDuration / 1.5f));
        beatSequence.OnComplete(() => { isBeating = false; });
        beatSequence.Play();
        isBeating = true;
        audioSource.PlayOneShot(beatClip);
        OnHeartBeat?.Invoke();
    }
    public void StopBeatingSequence()
    {
        if (beatSequence != null)
        {
            beatSequence.Kill();
            beatSequence = null;
        }
    }
    public void OnMouseDown()
    {
        Beat();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Beat();
        }
    }
}
