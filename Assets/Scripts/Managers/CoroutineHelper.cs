using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{
    public GameObject banner;
    public float moveDistance = 5f;  // The distance to move
    public float moveDuration = 2f;  // The duration of the movement

    private Vector3 originalPosition;
    private TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();

    public void Start()
    {
        originalPosition = new Vector3(0, 600, 0);
    }

    public async Task NewTurnAnimation()
    {
        originalPosition = new Vector3(0, 600, 0);
        banner.transform.position = originalPosition;

        completionSource = new();
        StartCoroutine(MoveBanner());
        await completionSource.Task;
        await Task.Delay(1000);
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
}
