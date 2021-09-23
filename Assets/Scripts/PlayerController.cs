using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    Vector2 movementInput;

    private void Update()
    {
        transform.Translate(new Vector3(movementInput.x, movementInput.y) * movementSpeed * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();
}
