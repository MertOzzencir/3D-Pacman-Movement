using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static event Action<bool> OnLeftMouse;
    public static event Action<bool> OnRightMouse;

    private InputActions inputMain;
    void Awake()
    {
        inputMain = new InputActions();
    }



    public Vector2 MovementNormalized()
    {
        return inputMain.Player.Move.ReadValue<Vector2>();
    }

    private void MouseRightClicked(InputAction.CallbackContext context)
    {
        bool isPressed = false;
        if (context.started)
            isPressed = true;
        if (context.canceled)
            isPressed = false;
        OnRightMouse?.Invoke(isPressed);
    }

    private void MouseLeftClicked(InputAction.CallbackContext context)
    {
        bool isPressed= false;
        if (context.started)
            isPressed = true;
        if (context.canceled)
            isPressed = false;

        OnLeftMouse?.Invoke(isPressed);
    }
    void OnEnable()
    {
        inputMain.Enable();
        inputMain.Player.MouseLeftButton.started += MouseLeftClicked;
        inputMain.Player.MouseLeftButton.canceled += MouseLeftClicked;
        inputMain.Player.MouseRightButton.started += MouseRightClicked;
        inputMain.Player.MouseRightButton.canceled += MouseRightClicked;
    }
    void OnDisable()
    {
        inputMain.Disable();
          inputMain.Player.MouseLeftButton.started -= MouseLeftClicked;
        inputMain.Player.MouseLeftButton.canceled -= MouseLeftClicked;
        inputMain.Player.MouseRightButton.started -= MouseRightClicked;
        inputMain.Player.MouseRightButton.canceled -= MouseRightClicked;
    }
}
