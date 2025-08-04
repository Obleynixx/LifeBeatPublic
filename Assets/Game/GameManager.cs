using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] GameSequence[] gameSequences;

    [Header("References")]
    [SerializeField] BeatHeart beatHeart;
    [SerializeField] BeatMeter beatMeter;

    [SerializeField] AudioSource geralAudio;
    [SerializeField] AudioSource musicAudio;


    public int currentIndex;
    Coroutine _gameLoopCoroutine;

    public static Action OnGameEnd;

    private bool gameEnded;

    public bool oldAgeEnded;

    public int score;

    public void StartGame()
    {
        _gameLoopCoroutine = StartCoroutine(PlayGameSequenceLooper());
    }
    public IEnumerator PlayGameSequenceLooper()
    {
        //currentIndex = 0;
        while (currentIndex < gameSequences.Length)
        {
            string seqName = gameSequences[currentIndex].name;
            PlaySequence(currentIndex);
            if (seqName == "OldAgeEKG")
            {
                yield return new WaitUntil(() => oldAgeEnded);
            }
            else
            {
                yield return new WaitForSeconds(gameSequences[currentIndex].sequenceDuration);
            }
            currentIndex++;
        }
        //if (gameEnded)
        //    yield return null;
        //gameEnded = true;
        OnGameEnd?.Invoke();
    }
    public IEnumerator IncreaseGameSequenceBPM()
    {
        yield return null;
    }
    public void EndGame()
    {
        if (gameEnded)
            return;
        if (gameSequences[currentIndex].name == "OldAgeEKG")
        {
            oldAgeEnded = true;
            gameEnded = true;
            return;
        }
        StopGameLoop();
        gameEnded = true;
        OnGameEnd?.Invoke();
    }
    public void PlaySequence(int index)
    {
        GameSequence seq = gameSequences[index];
        Sequence sequence = DOTween.Sequence();
        Debug.Log("Starting Sequence: " + seq.name + " With bpm " + seq.sequenceBPM);
        if (seq.name == "Old Age")
        {
            sequence.Append(beatHeart.heart.DOFade(0, 1f).OnComplete(() =>
            {
                beatHeart.StopBeatingSequence();
                beatHeart.heart.sprite = seq.sequenceSprite;
                //beatHeart.heart.color = new Color(beatHeart.heart.color.r, beatHeart.heart.color.g, beatHeart.heart.color.b, 1);
                
                beatHeart.beatClip = seq.sequenceAudioClip;
                beatMeter.bpm = seq.sequenceBPM;
                beatMeter.bloodColor = seq.particleColor;
                if (seq.sequenceGeralAudioClip != null)
                {
                    geralAudio.PlayOneShot(seq.sequenceGeralAudioClip);
                }
                if (seq.sequenceMusicAudioClip != null)
                {
                    musicAudio.clip = seq.sequenceMusicAudioClip;
                    musicAudio.Play();
                }
            }
            ));
            sequence.Append(beatHeart.heart.DOFade(1, 2f));


            //beatHeart.ekgBeep.gameObject.SetActive(true);
            //beatHeart.ekgBeep.color = new Color(beatHeart.ekgBeep.color.r, beatHeart.ekgBeep.color.g, beatHeart.ekgBeep.color.b, 1);
            sequence.Play();
            return;
        }
        sequence.Append(beatHeart.heart.DOFade(0, 1f).OnComplete(() =>
        {
            beatHeart.heart.sprite = seq.sequenceSprite;
            beatHeart.beatClip = seq.sequenceAudioClip;
            beatMeter.bpm = seq.sequenceBPM;
            if (seq.helperSprite != null)
            {
                beatHeart.ekgBeep.sprite = seq.helperSprite;
            }
            beatMeter.bloodColor = seq.particleColor;
            if (seq.sequenceGeralAudioClip != null)
            {
                geralAudio.PlayOneShot(seq.sequenceGeralAudioClip);
            }
            if (seq.sequenceMusicAudioClip != null)
            {
                musicAudio.clip = seq.sequenceMusicAudioClip;
                musicAudio.Play();
                musicAudio.DOFade(1, 2f);
            }
        }
        ));
        sequence.Join(musicAudio.DOFade(0, 1f));
        sequence.Append(beatHeart.heart.DOFade(1, 2f));
        if (seq.name == "OldAgeEKG")
        {
            Sequence deathSequence = DOTween.Sequence();
            deathSequence.AppendInterval(10f);
            deathSequence.Append(DOTween.To(() => beatMeter.bpm, x => beatMeter.bpm = x, 250, 20).SetEase(Ease.Linear));
            deathSequence.Play().OnComplete(() => { beatMeter.artificialFailure = true; });
        }
        sequence.Play();

    }

    public void StopGameLoop()
    {
        if (_gameLoopCoroutine != null)
            StopCoroutine(_gameLoopCoroutine);
    }
    public string GetGameSequenceName()
    {
        return gameSequences[currentIndex].name;
    }
}
//II can use my game sequence to store things like what center image of each phase is
[Serializable]
public class GameSequence
{
    public string name;
    public float sequenceDuration;
    public float sequenceBPM;
    public Sprite sequenceSprite;
    public Sprite helperSprite;
    public AudioClip sequenceAudioClip;
    public AudioClip sequenceGeralAudioClip;
    public AudioClip sequenceMusicAudioClip;
    public Color particleColor;
}
