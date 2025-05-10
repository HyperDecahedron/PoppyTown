using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Moral_Meter : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private Text floatingText; 

    private int maxReputation = 100;
    private int currentReputation = 40;

    private Coroutine floatingTextCoroutine;

    private void Start()
    {
        UpdateBar();

        floatingText.gameObject.SetActive(false);
    }

    public void UpdateReputation(int increment)
    {
        int potentialReputation = currentReputation + increment;

        if (potentialReputation >= 0 && potentialReputation <= maxReputation)
        {
            currentReputation = potentialReputation;
            ShowFloatingText(increment);
            UpdateBar();
        }
    }

    private void UpdateBar()
    {
        if (fillImage != null)
        {
            float fillAmount = (float)currentReputation / maxReputation;
            fillImage.fillAmount = fillAmount;
        }
    }

    private void ShowFloatingText(int increment)
    {
        if (floatingText != null)
        {
            floatingText.text = (increment > 0 ? "+" : "") + increment.ToString();
            floatingText.color = (increment < 0) ? Color.white : new Color32(0xC8, 0x00, 0x15, 0xFF);
            floatingText.gameObject.SetActive(true);

            if (floatingTextCoroutine != null)
            {
                StopCoroutine(floatingTextCoroutine);
            }

            floatingTextCoroutine = StartCoroutine(HideFloatingTextAfterDelay(3f));
        }
    }

    private IEnumerator HideFloatingTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        floatingText.gameObject.SetActive(false);
    }
}
