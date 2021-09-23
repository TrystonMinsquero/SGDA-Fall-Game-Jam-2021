using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;

    Controls controls;
    Vector2 movementInput;
    Vector2 lookInput;
    bool dashInput;
    bool shootInput;

    private void Awake()
    {
        controls = new Controls();
    }

    private void Update()
    {
        //Move
        transform.Translate(new Vector3(movementInput.x, movementInput.y) * movementSpeed * Time.deltaTime, Space.World);

        //Look
        if(lookInput.sqrMagnitude > .1f)
        {
            transform.rotation = Quaternion.Euler(0,0,Mathf.Rad2Deg * Mathf.Atan2(lookInput.y, lookInput.x));
        }

    }


    //Get Inputs
    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();

    public void OnLook(InputAction.CallbackContext ctx) => lookInput = ctx.ReadValue<Vector2>();

    public void OnDash(InputAction.CallbackContext ctx) => dashInput = ctx.ReadValue<bool>();

    public void OnShoot(InputAction.CallbackContext ctx) => shootInput = ctx.ReadValue<bool>();

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
