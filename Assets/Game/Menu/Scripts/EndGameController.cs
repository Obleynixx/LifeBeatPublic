using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameController : MonoBehaviour
{

    [SerializeField] Image blackImage;
    [SerializeField] Image heart;
    [SerializeField] TextMeshProUGUI ripText;
    [SerializeField] TextMeshProUGUI creditsText;
    [SerializeField] TextMeshProUGUI thxplayingText;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject gameTransform;
    private void OnEnable()
    {
        GameManager.OnGameEnd += StartEnd;
    }
    private void OnDisable()
    {
        GameManager.OnGameEnd -= StartEnd;
    }
    [ContextMenu("Start End")]
    void StartEnd() 
    {
        scoreText.text = "Beats Lived: " + GameManager.Instance.score; 
        Sequence seq = DOTween.Sequence();
        blackImage.gameObject.SetActive(true);
        heart.gameObject.SetActive(true);
        seq.Append(blackImage.DOFade(1,2f).OnComplete(() => { gameTransform.SetActive(false); }));
        seq.Append(heart.transform.DOScale(0,1f).OnComplete(() => { heart.gameObject.SetActive(false); }));
        seq.Append(ripText.DOFade(1f,2f));
        seq.Join(scoreText.DOFade(1f, 2f));
        seq.AppendInterval(2f);
        seq.Append(ripText.DOFade(0, 2f));
        seq.Join(scoreText.DOFade(0f, 2f));
        seq.Append(creditsText.DOFade(1, 2f));
        seq.AppendInterval(2f);
        seq.Append(creditsText.DOFade(0, 2f));
        seq.Append(thxplayingText.DOFade(1, 2f));
        seq.AppendInterval(2f);
        seq.Append(thxplayingText.DOFade(0, 2f));
        seq.AppendInterval(2f);
        seq.OnComplete(() => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); });
    }
}
