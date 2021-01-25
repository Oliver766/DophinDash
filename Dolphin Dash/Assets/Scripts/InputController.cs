// SID: 1903490
// Date: 11/12/2020

using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityBoolEvent : UnityEvent<bool> {}

[System.Serializable]
public class UnityIntEvent : UnityEvent<int> {}

public class InputController : MonoBehaviour
{
    public UnityEvent PauseEvent;
    public UnityEvent JumpEvent;
    public UnityBoolEvent DiveEvent;
    public UnityIntEvent LaneSwitchEvent;

    private InputControls controls;

    private void Awake()
    {
        controls = new InputControls();

        // Subscribe to the actions
        // Dive needs to know when held so subscribe to performed and cancelled
        controls.Player.Dive.performed += _ => DiveEvent.Invoke(true);
        controls.Player.Dive.canceled += _ => DiveEvent.Invoke(false);

        // Only need to know when pressed, subscribe to performed
        controls.Player.Pause.performed += _ => PauseEvent.Invoke();
        controls.Player.Jump.performed += _ => JumpEvent.Invoke();
        controls.Player.SwitchLeft.performed += _ => LaneSwitchEvent.Invoke(-1);
        controls.Player.SwitchRight.performed += _ => LaneSwitchEvent.Invoke(1);
    }

    // Enable and disable the input
    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
}
