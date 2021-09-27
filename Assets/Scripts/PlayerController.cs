using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Camera cam;

    [HideInInspector]
    public Player player;
    [HideInInspector]
    Controls controls;
    Vector2 movementInput;
    Vector2 lookInput;
    bool dashInput;
    bool shootInput;

    private void Awake()
    {
        controls = new Controls();
        player = GetComponent<Player>();
        cam = GetComponentInChildren<Camera>();

    }

    private void FixedUpdate()
    {
        //Move
        player.Move(movementInput);
    }

    private void Update()
    {
        //Look
        player.Look(lookInput);

        //Dash
        if (dashInput)
            player.StartDash();


        //Shoot
        if (shootInput)
            player.Shoot();


        cam.transform.rotation = Quaternion.Euler(Vector3.zero);

    }

    
    //Get Inputs
    public void OnMove(InputAction.CallbackContext ctx) => movementInput = ctx.ReadValue<Vector2>();

    public void OnLook(InputAction.CallbackContext ctx) => lookInput = ctx.ReadValue<Vector2>();

    public void OnDash(InputAction.CallbackContext ctx) => dashInput = ctx.action.triggered;

    public void OnShoot(InputAction.CallbackContext ctx) => shootInput = ctx.action.triggered;

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
