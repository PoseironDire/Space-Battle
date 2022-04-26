using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    [HideInInspector] public Vector2 moveInput; public void OnMove(InputAction.CallbackContext ctx) => moveInput = ctx.ReadValue<Vector2>();
    [HideInInspector] public Vector2 liftInput; public void OnLift(InputAction.CallbackContext ctx) => liftInput = ctx.ReadValue<Vector2>();
    [HideInInspector] public Vector2 lookInput; public void OnLook(InputAction.CallbackContext ctx) => lookInput = ctx.ReadValue<Vector2>();
    [HideInInspector] public bool useInput; public void OnUse(InputAction.CallbackContext ctx) => useInput = ctx.action.triggered;
}
