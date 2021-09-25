using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Camera cam;

    [HideInInspector]
    public Rigidbody2D rb;
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
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();
        cam = GetComponentInChildren<Camera>();

    }

    private void Update()
    {
        //Move
        transform.Translate(new Vector3(movementInput.x, movementInput.y) * player.movementSpeed * Time.deltaTime, Space.World);
        //Look
        if(lookInput.sqrMagnitude > .1f)
        {
            transform.rotation = Quaternion.Euler(0,0,Mathf.Rad2Deg * Mathf.Atan2(lookInput.y, lookInput.x));
            player.lookDirection = lookInput;
        }
        else if(movementInput.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(movementInput.y, movementInput.x));
            player.lookDirection = movementInput;
        }

        //Dash
        if (dashInput)
            player.Dash();


        //Shoot
        if (shootInput)
            player.Shoot();

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;

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
