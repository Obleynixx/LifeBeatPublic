using UnityEngine;
using UnityEngine.UI;

public class FlowController : MonoBehaviour
{

    [SerializeField] MenuController menuController;
    public float bloodFlowMax = 100f;
    public float bloodFlowLostSpeed = 5f;

    public Image bloodFlowImage;
    private float bloodFlow;

    private void Start()
    {
        bloodFlow = bloodFlowMax;
    }

    private void Update()
    {
        if (!menuController.finishedIntroduction)
            return;
        bloodFlow -= bloodFlowLostSpeed * Time.deltaTime;
        if (bloodFlow <= 0)
        {
            GameManager.Instance.EndGame();
        }
        bloodFlow = Mathf.Clamp(bloodFlow, 0, bloodFlowMax);
        bloodFlowImage.fillAmount = bloodFlow / bloodFlowMax;
    }

    public void AddFlow(float amount)
    {
        bloodFlow += amount;
    }

    public void RemoveFlow(float amount)
    {
        bloodFlow -= amount;
    }
}
