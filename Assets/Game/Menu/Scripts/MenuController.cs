using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Image heart;
    public Image background;

    public GameObject gameWrapper;
    public TextMeshProUGUI startText;
    public GameObject volumeGameObject;
    public Slider volumeSlider;
    public Image volumeIcon;
    public Image sliderCircle;
    public Image sliderFill;
    public Image sliderBackground;

    private bool gameStarted;
    public bool finishedIntroduction;
    [ContextMenu("StartGame")]
    public void StartGame()
    {

        heart.gameObject.SetActive(true);
        heart.transform.localScale = Vector3.zero;
        gameWrapper.SetActive(true);
        Sequence startSequence = DOTween.Sequence();
        startSequence.Append(startText.DOFade(0, 1f).OnComplete(() => { volumeGameObject.SetActive(false); }));

        //Volume Slider fadeout

        volumeSlider.interactable = false;

        startSequence.Join(volumeIcon.DOFade(0,1f));
        startSequence.Join(sliderCircle.DOFade(0,1f));
        startSequence.Join(sliderFill.DOFade(0,1f));
        startSequence.Join(sliderBackground.DOFade(0,1f));
        startSequence.Join(background.DOColor(Color.black,1f));


        startSequence.Append(heart.transform.DOScale(Vector3.one, 2f));
        startSequence.Append(background.DOFade(0,2f)).OnComplete(() => { gameObject.SetActive(false); finishedIntroduction = true; });
        startSequence.Join(heart.DOFade(0,1.5f));

        startSequence.Play();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)&&!gameStarted)
        {
            StartGame();
            GameManager.Instance.StartGame();
            gameStarted = true;
        }
    }
}
