// SID: 1903490
// Date: 11/12/2020

using System.Collections;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Tooltip("The time it takes to switch lanes (secs).")]
    [SerializeField] private float switchTime = 1f;

    [Space]
    [Tooltip("The time it takes to dive (secs).")]
    [SerializeField] private float diveTime = 1f;

    [Space]
    [Tooltip("The length of the jump (secs).")]
    [SerializeField] private float jumpTime = 1f;

    [Tooltip("The time for the jump anticipation animation (secs).")]
    [SerializeField] private float jumpAnticipation = 0.3f;

    [Tooltip("The time for the jump anticipation animation from the dive position (secs).")]
    [SerializeField] private float diveJumpAnticipation = 0.5f;

    [Header("Positions")]
    [Tooltip("The lane positions in order from left to right.")]
    [SerializeField] private Transform[] lanes = null;
    [SerializeField] private Transform jumpHeight = null;
    [SerializeField] private Transform diveDepth = null;

    [Header("References")]
    [SerializeField] private PreyDetection preyDetection = null;

    private Animator animator;
    private int currentLane;
    private bool isLerpingHorizontal;
    private bool isLerpingVertical;
    private bool wantsToDive;
    private bool isDived;
    private bool isJumping;

    #region Object Setup

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("Switch Speed", 1f / switchTime);
        animator.SetFloat("Dive Speed", 1f / diveTime);
        animator.SetFloat("Jump Speed", 1f / jumpTime);

        // Set starting lane as the middle lane
        currentLane = Mathf.FloorToInt(lanes.Length * 0.5f);
        transform.position = lanes[currentLane].position;
    }

    private void OnValidate()
    {
        // Editor only
        // Update animator if values change during play
        if (animator)
        {
            animator.SetFloat("Switch Speed", 1f / switchTime);
            animator.SetFloat("Dive Speed", 1f / diveTime);
            animator.SetFloat("Jump Speed", 1f / jumpTime);
        }          
    }

    #endregion

    private void Update()
    {
        if (!GameController.IsPlaying)
        {
            return;
        }

        if (wantsToDive && !isDived && !isLerpingVertical && !isJumping)
        {
            // Wants to dive and isn't already moving vertically
            StartCoroutine(SmoothLerpVertical(transform.position.y, diveDepth.position.y, switchTime));
            animator.SetBool("Dive", true);
            isDived = true;
        }

        if (!wantsToDive && isDived && !isLerpingVertical && !isJumping)
        {
            // Doesn't want to dive and isn't already moving vertically
            StartCoroutine(SmoothLerpVertical(transform.position.y, lanes[0].position.y, switchTime));
            animator.SetBool("Dive", false);
            isDived = false;
        }
    }

    /// <summary>
    /// Switches lane in a given direction.
    /// </summary>
    /// <param name="direction">The lane to switch to. -1 for left, 1 for right.</param>
    public void OnLaneSwitch(int direction)
    {
        if (isLerpingHorizontal || !GameController.IsPlaying)
        {
            return;
        }
        
        if (direction == -1 && currentLane > 0)
        {
            // Switch lane left
            StartCoroutine(SmoothLerpHorizontal(transform.position.x, lanes[--currentLane].position.x, switchTime));
            animator.SetTrigger("Left");
        }
        else if (direction == 1 && currentLane < lanes.Length - 1)
        {
            // Switch lane right
            StartCoroutine(SmoothLerpHorizontal(transform.position.x, lanes[++currentLane].position.x, switchTime));
            animator.SetTrigger("Right");
        }
    }

    /// <summary>
    /// Toggles the dive state of the dolphin.
    /// </summary>
    public void OnDive()
    {
        wantsToDive = !wantsToDive;
    }

    /// <summary>
    /// Sets the dive state of the dolphin.
    /// </summary>
    /// <param name="dive">The state to set to.</param>
    public void OnDive(bool dive)
    {
        wantsToDive = dive;
    }

    /// <summary>
    /// Makes the dolphin jump.
    /// </summary>
    public void OnJump()
    {
        if (isJumping || isLerpingVertical || !GameController.IsPlaying)
        {
            return;
        }

        animator.SetFloat("Jump Anticipation Speed", 1f / (isDived ? diveJumpAnticipation : jumpAnticipation));
        animator.SetTrigger("Jump");
        StartCoroutine(Jump());
    }

    /// <summary>
    /// Makes the dolphin eat the prey.
    /// </summary>
    public void OnEat()
    {
        preyDetection.Eat();
    }

    #region Coroutines

    private IEnumerator SmoothLerpHorizontal(float from, float to, float time)
    {
        isLerpingHorizontal = true;

        float t = 0;

        while (t < 1f)
        {
            transform.position = new Vector3(EaseInOut(from, to, t), transform.position.y, transform.position.z);
            t += Time.deltaTime / time;
            yield return null;
        }

        isLerpingHorizontal = false;
    }

    private IEnumerator SmoothLerpVertical(float from, float to, float time)
    {
        isLerpingVertical = true;

        float t = 0;

        while (t < 1f)
        {
            transform.position = new Vector3(transform.position.x, EaseInOut(from, to, t), transform.position.z);
            t += Time.deltaTime / time;
            yield return null;
        }

        isLerpingVertical = false;
    }

    private IEnumerator Jump()
    {
        isJumping = true;
        float t = 0;

        float depth = isDived ? diveDepth.position.y : lanes[0].position.y;

        // Anticipation
        while (t < 1)
        {
            transform.position = new Vector3(transform.position.x, EaseIn(depth, lanes[0].position.y, t), transform.position.z);
            t += Time.deltaTime / (isDived ? diveJumpAnticipation : jumpAnticipation);
            yield return null;
        }

        t = 0;

        // Jump
        while (t < 1)
        {
            float y = t < 0.5f ? EaseOut(lanes[0].position.y, jumpHeight.position.y, t * 2f) : EaseIn(jumpHeight.position.y, lanes[0].position.y, (t - 0.5f) * 2f);
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
            t += Time.deltaTime / jumpTime;
            yield return null;
        }

        t = 0;

        // Update dive value & state if it changes during jump
        isDived = wantsToDive;
        animator.SetBool("Dive", isDived);
        depth = isDived ? diveDepth.position.y : lanes[0].position.y;
        animator.SetFloat("Jump Anticipation Speed", 1f / (isDived ? diveJumpAnticipation : jumpAnticipation));

        // Landing
        while (t < 1)
        {
            transform.position = new Vector3(transform.position.x, EaseOut(lanes[0].position.y, depth, t), transform.position.z);
            t += Time.deltaTime / (isDived ? diveJumpAnticipation : jumpAnticipation);
            yield return null;
        }

        isJumping = false;
    }

    #endregion

    #region Ease Functions

    /// <summary>
    /// Quadratic ease in and out function.
    /// </summary>
    /// <param name="start">The starting value.</param>
    /// <param name="end">The finish value.</param>
    /// <param name="value">The progress percentage.</param>
    /// <returns>The percentage value eased in and out.</returns>
    private float EaseInOut(float start, float end, float value)
    {
        float t = value < 0.5f ? 2 * Mathf.Pow(value, 2) : 1 - Mathf.Pow(-2 * value + 2, 2) * 0.5f;
        return Mathf.Lerp(start, end, t);
    }

    /// <summary>
    /// Cubic ease in function.
    /// </summary>
    /// <param name="start">The starting value.</param>
    /// <param name="end">The finish value.</param>
    /// <param name="value">The progress percentage.</param>
    /// <returns>The percentage value eased in.</returns>
    private float EaseIn(float start, float end, float value)
    {
        float t = Mathf.Pow(value, 3);
        return Mathf.Lerp(start, end, t);
    }

    /// <summary>
    /// Cubic ease out function.
    /// </summary>
    /// <param name="start">The starting value.</param>
    /// <param name="end">The finish value.</param>
    /// <param name="value">The progress percentage.</param>
    /// <returns>The percentage value eased out.</returns>
    private float EaseOut(float start, float end, float value)
    {
        float t = 1 - Mathf.Pow(1 - value, 3);
        return Mathf.Lerp(start, end, t);
    }

    #endregion

}
