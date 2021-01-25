// SID: 1903490
// Date: 11/12/2020

using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    #region Instance Fields

    [Tooltip("Should the game start on load?")]
    [SerializeField] bool startOnLoad = false;

    [Space]
    [Tooltip("The intial speed (u/s) of the game when it starts.")] [Range(0f, 25f)]
    [SerializeField] float initialSpeed = 5f;

    [Tooltip("The max speed (u/s) that the game can progress at.")] [Range(0f, 25f)]
    [SerializeField] float maxSpeed = 10f;

    [Space]
    [Tooltip("How fast the game speeds up (u/s/s).")]
    [SerializeField] float acceleration = 0.025f;

    [Space]
    [Tooltip("Multiplier minimum (inclusive).")] [Range(0f, 10f)]
    [SerializeField] float multiplierMin = 1f;

    [Tooltip("Multiplier maximum (inclusive).")] [Range(0f, 10f)]
    [SerializeField] float multiplierMax = 2f;

    [Tooltip("The rate at which the multiplier decreases at (/s).")]
    [SerializeField] float multiplierDecayRate = 0.1f;

    #endregion

    #region Events

    [Header("Events")]
    [Tooltip("Called when the game begins.")]
    public UnityEvent GameBeginEvent;

    [Tooltip("Called when the player loses.")]
    public UnityEvent GameOverEvent;

    [Tooltip("Called when the game pauses.")]
    public UnityEvent PauseEvent;

    [Tooltip("Called when the game unpauses.")]
    public UnityEvent ResumeEvent;

    #endregion

    #region Properties

    private static float _distance = 0f;
    private static int _points = 0;
    private static float _multiplier = 1f;

    /// <summary>
    /// Is the game paused?
    /// </summary>
    private static bool IsPaused { get; set; }

    /// <summary>
    /// Is the game being played?
    /// </summary>
    private static bool IsInGame { get; set; }

    /// <summary>
    /// Is the game running?
    /// </summary>
    public static bool IsPlaying => !IsPaused && IsInGame;

    /// <summary>
    /// The current speed of the game.
    /// </summary>
    public static float Speed { get; private set; }

    /// <summary>
    /// The acceleration of the game.
    /// </summary>
    public static float Acceleration => instance.acceleration;  

    /// <summary>
    /// The combined score of distance and points. <br/>
    /// NOTE: Is automatically totalled as distance and points change.
    /// </summary>
    public static float Score { get; private set; }

    /// <summary>
    /// The multiplier to affect additions to score.
    /// </summary>
    public static float Multiplier 
    { 
        get => _multiplier; 
        set {
            _multiplier = value;
            _multiplier = Mathf.Clamp(_multiplier, instance.multiplierMin, instance.multiplierMax);
        } 
    }

    /// <summary>
    /// Is the multiplier at max?
    /// </summary>
    public static bool IsMaxMultiplier => Multiplier >= instance.multiplierMax;

    /// <summary>
    /// How far the game has gone. <br/>
    /// NOTE: Updates score taking into account the multiplier.
    /// </summary>
    public static float Distance 
    { 
        get => _distance; 
        set {
            Score += (value - _distance) * Multiplier;
            _distance = value;
        }
    }

    /// <summary>
    /// The total points collected from pickups.  <br/>
    /// NOTE: Updates score taking into account the multiplier.
    /// </summary>
    public static int Points 
    { 
        get => _points; 
        set {
            Score += (value - _points) * Multiplier;
            _points = value;
        }
    }

    #endregion

    private static bool restart;

    #region Object Setup

    private static GameController instance;

    private void OnValidate()
    {
        initialSpeed = Mathf.Clamp(initialSpeed, 0f, maxSpeed);
        multiplierMax = Mathf.Clamp(multiplierMax, multiplierMin, 10f);
    }

    private void Awake()
    {
        // Set the instance
        instance = this;
    }

    #endregion

    private void Start()
    {
        IsPaused = false;
        IsInGame = false;
        Time.timeScale = 1f;

        // Begin the game if restarting or startOnLoad is enabled
        if (startOnLoad || restart)
        {
            Begin();
        }
    }

    private void Update()
    {      
        if (IsPlaying)
        {
            Distance += Speed * Time.deltaTime;

            // Increase the speed gradually while playing until it reaches max speed
            if (Speed < maxSpeed)
            {
                Speed += acceleration * Time.deltaTime;
            }
            
            // Gradually decrease the multiplier
            if (Multiplier > multiplierMin)
            {
                Multiplier -= Time.deltaTime * multiplierDecayRate;
            }
        }
    }

    /// <summary>
    /// Stops the game.
    /// </summary>
    private static void Stop()
    {
        Time.timeScale = 0f;
        IsInGame = false;
    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    private static void Pause()
    {
        Time.timeScale = 0f;
        IsPaused = true;
        instance.PauseEvent.Invoke();
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    private static void Resume()
    {
        Time.timeScale = 1f;
        IsPaused = false;
        IsInGame = true;
        instance.ResumeEvent.Invoke();
    }

    /// <summary>
    /// Begins the game.
    /// </summary>
    public static void Begin()
    {
        // Reset stats
        Points = 0;
        Speed = instance.initialSpeed;
        Distance = 0f;
        ResetMultiplier();

        // Score must be reset last otherwise resetting the Distance and Points makes it negative
        Score = 0f;

        IsInGame = true;
        instance.GameBeginEvent.Invoke();
    }

    /// <summary>
    /// Signifies that the player has lost.
    /// </summary>
    public static void GameOver()
    {
        Stop();

        instance.GameOverEvent.Invoke();
    }

    /// <summary>
    /// Restarts the game bypassing the Main Menu.
    /// </summary>
    public static void Restart()
    {
        Stop();

        restart = true;
        SceneLoader.ReloadLevel();
    }

    /// <summary>
    /// Returns the game to the main menu.
    /// </summary>
    public static void MainMenu()
    {
        Stop();

        restart = false;
        SceneLoader.ReloadLevel();
    }

    /// <summary>
    /// Toggle the game pause state.
    /// </summary>
    public static void TogglePause()
    {
        SetPause(!IsPaused);
    }

    /// <summary>
    /// Sets the pause state of the game.
    /// </summary>
    /// <param name="state">Whether to pause the game or not.</param>
    public static void SetPause(bool state)
    {
        if (!IsInGame) 
        {
            return;
        }
        
        if (state)
        {
            // Game is playing, pause it
            Pause();
            
            Debug.Log("Game Paused");
        }
        else
        {
            // Game is paused, resume it
            Resume();

            Debug.Log("Game Resumed");
        }      
    }

    /// <summary>
    /// Resets the multiplier.
    /// </summary>
    public static void ResetMultiplier()
    {
        Multiplier = instance.multiplierMin;
    }
}
