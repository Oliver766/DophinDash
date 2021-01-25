// SID: 1903490
// Date: 11/12/2020

using System.Collections;
using UnityEngine;

public class EnvironmentController : MonoBehaviour
{
    [Tooltip("The amount the view is bent horizontally (Negative is left, Positive is right).")] [Range(-0.05f, 0.05f)]
    [SerializeField] private float horizontalBend = 0f;

    [Tooltip("The amount the view is bent vertically (Negative is down, Positive is up).")] [Range(-0.05f, 0.05f)]
    [SerializeField] private float verticalBend = -0.0025f;

    [Tooltip("The time (secs) it takes for the animation to complete.")] [Range(0.1f, 3f)]
    [SerializeField] private float bendSpeed = 1f;

    private bool isBendingVertical = false;
    private bool isBendingHorizontal = false;
    private float waitTime = 15f;

    private void OnValidate()
    {
        SetVerticalBend(verticalBend);
        SetHorizontalBend(horizontalBend);
    }

    private void Start()
    {
        SetHorizontalBend(0f);
        SetVerticalBend(verticalBend);
    }

    private void Update()
    {
        if (!GameController.IsPlaying)
        { 
            return;
        }

        if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
        else
        {
            HorzontalBend(Random.Range(-0.005f, 0.005f));
            waitTime = Random.Range(12f, 25f);
        }
    }

    /// <summary>
    /// Bend the view vertically.
    /// </summary>
    /// <param name="amount">The amount to bend by.</param>
    public void VerticalBend(float amount)
    {
        if (!isBendingVertical)
        {
            StartCoroutine(SmoothTransitionVertical(verticalBend, amount, bendSpeed));
        }
    }

    /// <summary>
    /// Bend the view horizontally.
    /// </summary>
    /// <param name="amount">The amount to bend by.</param>
    public void HorzontalBend(float amount)
    {
        if (!isBendingHorizontal)
        {
            StartCoroutine(SmoothTransitionHorizontal(horizontalBend, amount, bendSpeed));
        }
    }

    /// <summary>
    /// Set the horizontal bend without animation.
    /// </summary>
    /// <param name="amount">The value to set it to.</param>
    public void SetHorizontalBend(float amount)
    {
        horizontalBend = Mathf.Clamp(amount, -0.05f, 0.05f);
        Shader.SetGlobalFloat("world_bend_x_amount", horizontalBend);
    }

    /// <summary>
    /// Set the vertical bend without animation.
    /// </summary>
    /// <param name="amount">The value to set it to.</param>
    public void SetVerticalBend(float amount)
    {
        verticalBend = Mathf.Clamp(amount, -0.05f, 0.05f);
        Shader.SetGlobalFloat("world_bend_y_amount", verticalBend);
    }

    #region Coroutines

    private IEnumerator SmoothTransitionVertical(float from, float to, float speed)
    {
        isBendingVertical = true;

        float t = 0;

        while (t <= 1)
        {
            float amount = Mathf.Lerp(from, to, t);
            SetVerticalBend(amount);
            t += Time.deltaTime * speed;

            yield return null;
        }

        isBendingVertical = false;
    }

    private IEnumerator SmoothTransitionHorizontal(float from, float to, float speed)
    {
        isBendingHorizontal = true;

        float t = 0;

        while (t <= 1)
        {
            float amount = Mathf.Lerp(from, to, t);
            SetHorizontalBend(amount);
            t += Time.deltaTime * speed;

            yield return null;
        }

        isBendingHorizontal = false;
    }

    #endregion
}
