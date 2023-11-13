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
        originalPosition = banner.transform.localPosition;

        // Start the coroutine for the banner movement
        StartCoroutine(MoveBanner());
    }

    internal async Task NewTurnAnimation()
    {
        completionSource = new();
        StartCoroutine(MoveBanner());
        await completionSource.Task;
        await Task.Delay(300);
    }

    private IEnumerator MoveBanner()
    {
        // Move the banner down
        yield return MoveTo(originalPosition - new Vector3(0f, moveDistance, 0f), moveDuration);
        yield return new WaitForSeconds(0.5f);
        // Move the banner back up to its original position
        yield return MoveTo(originalPosition, moveDuration);

        completionSource.SetResult(true);
    }

    private IEnumerator MoveTo(Vector3 targetPosition, float duration)
    {
        float elapsed = 0f;
        Vector3 startingPosition = banner.transform.localPosition;

        while (elapsed < duration)
        {
            // Interpolate the position over time
            banner.transform.localPosition = Vector3.Lerp(startingPosition, targetPosition, elapsed / duration);

            elapsed += Time.deltaTime;

            // Yielding null here means wait for the next frame
            yield return null;
        }

        // Ensure that the final position is exactly the target position
        banner.transform.localPosition = targetPosition;
    }
}
