using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoroutineHelper : MonoBehaviour
{
    public GameObject successIndication;
    

    public GameObject banner;
    public float moveDistance = 5f;  // The distance to move
    public float moveDuration = 2f;  // The duration of the movement
    public float fadeDuration = 1f;
    [Header("text")]
    public float textFadeDuration = 2f;
    public TextMeshProUGUI hintText;
    private Vector3 originalPosition;
    private TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();

    public void Start()
    {
        originalPosition = new Vector3(0, 600, 0);
    }

    public async Task NewTurnAnimation()
    {
        GameManager.Instance.mouseStateManager.allowedToClick = false;
        originalPosition = new Vector3(0, 600, 0);
        banner.transform.position = originalPosition;

        completionSource = new();
        StartCoroutine(MoveBanner());
        await completionSource.Task;
        await Task.Delay(1000);
        GameManager.Instance.mouseStateManager.allowedToClick = true;
    }


    private IEnumerator MoveBanner()
    {
        // Move the banner down
        yield return MoveTo(originalPosition - new Vector3(0f, moveDistance, 0f), originalPosition, moveDuration);
        yield return new WaitForSeconds(0.5f);
        // Move the banner back up to its original position
        yield return MoveTo(originalPosition, banner.transform.localPosition, moveDuration);

        completionSource.SetResult(true);
    }

    private IEnumerator MoveTo(Vector3 targetPosition, Vector3 currentPosition, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // Interpolate the position over time
            banner.transform.localPosition = Vector3.Lerp(currentPosition, targetPosition, elapsed / duration);

            elapsed += Time.deltaTime;

            // Yielding null here means wait for the next frame
            yield return null;
        }

        // Ensure that the final position is exactly the target position
        banner.transform.localPosition = targetPosition;
    }

    public void ResultSuccess(bool result)
    {
        successIndication.GetComponent<Image>().color = result ? Color.green : Color.red;
        StartCoroutine(nameof(Fade));
    }
    
    public void ShowHintText(string content)
    {
        StopCoroutine(nameof(TextFade));
        StartCoroutine(nameof(TextFade), content);
    }


    private IEnumerator Fade()
    {
        float elapsedTime = 0f;
        Image targetImage = successIndication.GetComponent<Image>();
        Color startColor = targetImage.color;

        while (elapsedTime < fadeDuration)
        {
            // Calculate the current alpha value based on the elapsed time
            float alpha = Mathf.Lerp(startColor.a, 0f, elapsedTime / fadeDuration);

            // Create a new color with the same RGB values and the calculated alpha
            Color newColor = new Color(startColor.r, startColor.g, startColor.b, alpha);

            // Apply the new color to the image
            targetImage.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        targetImage.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
    }

    private IEnumerator TextFade(string content)
    {
        float elapsedTime = 0f;
        hintText.text = content;

        Color startColor = Color.red;

        while (elapsedTime < textFadeDuration)
        {
            // Calculate the current alpha value based on the elapsed time
            float alpha = Mathf.Lerp(startColor.a/2, 0f, elapsedTime / textFadeDuration);

            // Create a new color with the same RGB values and the calculated alpha
            Color newColor = new Color(startColor.r, startColor.g, startColor.b, alpha);

            hintText.color = newColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        hintText.color = new Color(startColor.r, startColor.g, startColor.b, 0f);
    }
}
