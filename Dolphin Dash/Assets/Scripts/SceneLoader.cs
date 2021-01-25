// SID: 1903490
// Date: 11/12/2020

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Tooltip("The length of the entrance/exit transition.")]
    [SerializeField] private float transitionLength = 1f;
    
    [Tooltip("The default scene to load when none is specified.")]
    [SerializeField] private string defaultScene = "Game";

    [Header("References")]
    [SerializeField] private Animator animator = null;

    private static readonly string loadingLevelName = "Loading";
    private static bool isLoading;

    /// <summary>
    /// The current level.
    /// </summary>
    public static string CurrentLevel { get; private set; }

    /// <summary>
    /// The loading level.
    /// </summary>
    public static string LoadingLevel { get; private set; }

    /// <summary>
    /// Loads a specific level.
    /// </summary>
    /// <param name="level">The name of the level to load.</param>
    public static void LoadLevel(string level)
    {
        if (isLoading)
        {
            // Prevent multiple levels loading
            return;
        }

        CurrentLevel = SceneManager.GetActiveScene().name;
        LoadingLevel = level;
        SceneManager.LoadScene(loadingLevelName, LoadSceneMode.Additive);
    }

    /// <summary>
    /// Reloads the current level.
    /// </summary>
    public static void ReloadLevel()
    {
        if (string.IsNullOrEmpty(CurrentLevel))
        {
            CurrentLevel = SceneManager.GetActiveScene().name;
        }

        LoadLevel(CurrentLevel);
    }

    private void Start()
    {
        StartCoroutine(StartLoading(LoadingLevel));
    }

    IEnumerator StartLoading(string level)
    {
        isLoading = true;

        if (string.IsNullOrEmpty(level))
        {
            LoadingLevel = level = defaultScene;
        }

        // Wait for transition
        yield return new WaitForSecondsRealtime(transitionLength);

        // Unload the current scene if it exists
        if (!string.IsNullOrEmpty(CurrentLevel))
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        }

        // Load the new scene
        AsyncOperation async = SceneManager.LoadSceneAsync(level, LoadSceneMode.Additive);
        
        while (!async.isDone)
        {
            // Wait until the new scene has loaded
            yield return null;
        }

        // Set the new scene as active to avoid the player seeing any lighting changes
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(level));
        CurrentLevel = level;
        LoadingLevel = null;

        // Wait for animation to finish
        animator.SetTrigger("Loaded");
        yield return new WaitForSecondsRealtime(transitionLength);

        // Unload loading screen
        isLoading = false;
        SceneManager.UnloadSceneAsync(loadingLevelName); 
    }
}
