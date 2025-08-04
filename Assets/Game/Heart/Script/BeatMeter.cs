using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.Events;

public class BeatMeter : MonoBehaviour
{
    [SerializeField] BeatHeart beatHeart;
    [SerializeField] FlowController flowController;
    [SerializeField] float minAngleToBeat = 0.89f;
    [SerializeField] float maxAngleToBeat = 0.081f;
    public GameObject bloodParticleVFX;
    public Transform bloodParticlePos;

    public Color bloodColor;
    
    public Transform beepPointer;
    public float bpm = 120f;
    
    private float beatProgress;

    private float beatStartTime;
    private float beatDuration;

    private Tween rotationTween;

    public bool artificialFailure = false;

    private void OnEnable()
    {
        beatHeart.OnHeartBeat.AddListener(OnHeartBeat);
    }
    private void OnDisable()
    {
        beatHeart.OnHeartBeat.RemoveListener(OnHeartBeat);
    }
    private void Start()
    {
        UpdateBpm();
    }

    private void OnHeartBeat()
    {
      if (IsInGreenZoneNormalized()&&!artificialFailure)
        {
            GameManager.Instance.score++;
            ParticleSystem bloodParticle = Instantiate(bloodParticleVFX, bloodParticlePos.position, bloodParticlePos.rotation).GetComponent<ParticleSystem>();
            var main = bloodParticle.main;
            main.startColor = bloodColor;
            Destroy(bloodParticle.gameObject, 3);
            flowController.AddFlow(25f);
        }else
        {
            //flowController.RemoveFlow(10f);
        }
    }
    
    private void SetupTween()
    {
        rotationTween?.Kill();

        float secondsPerBeat = 60f / bpm;
        rotationTween = beepPointer
            .DORotate(new Vector3(0, 0, -360f), secondsPerBeat, RotateMode.LocalAxisAdd)
            .SetEase(Ease.Linear)
            .SetLoops(-1);
    }
    private void Update()
    {
        if (rotationTween == null || !Mathf.Approximately(((Tweener)rotationTween).Duration(), 60f / bpm))
            SetupTween();

        float rawZ = beepPointer.localEulerAngles.z;
        beatProgress = Mathf.Repeat(rawZ / 360f, 1f);
    }

    bool IsInGreenZoneNormalized()
    {
        return beatProgress >= minAngleToBeat || beatProgress <= maxAngleToBeat;
    }

    [ContextMenu("UpdateBPM")]
    void UpdateBpm()
    {
        float phase = 0f;
        if (beatDuration > 0.01f)
        {
            float timeSinceStart = Time.time - beatStartTime;
            phase = (timeSinceStart % beatDuration) / beatDuration;
        }
        rotationTween?.Kill();
        rotationTween = null;

        beatDuration = 60f / bpm;


        beatStartTime = Time.time - (phase * beatDuration);
    }
    void OnDestroy()
    {
        rotationTween?.Kill();
    }
}
