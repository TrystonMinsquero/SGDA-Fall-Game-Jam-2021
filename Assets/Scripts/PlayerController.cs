using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public float dashDelay;
    public float dashForce;
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public Player player;
    [HideInInspector]
    public Vector2 lookDirection;

    private float nextDashTime;
    Controls controls;
    Vector2 movementInput;
    Vector2 lookInput;
    bool dashInput;
    bool shootInput;

    private void Awake()
    {
        controls = new Controls();
        nextDashTime = 0;
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<Player>();

    }

    private void Update()
    {
        //Move
        transform.Translate(new Vector3(movementInput.x, movementInput.y) * movementSpeed * Time.deltaTime, Space.World);

        //Look
        if(lookInput.sqrMagnitude > .1f)
        {
            transform.rotation = Quaternion.Euler(0,0,Mathf.Rad2Deg * Mathf.Atan2(lookInput.y, lookInput.x));
            lookDirection = lookInput;
        }
        else if(movementInput.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(movementInput.y, movementInput.x));
            lookDirection = movementInput;
        }

        //Dash
        if (dashInput)
            Dash();


        //Shoot
        if (shootInput)
            Shoot();

        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0;
    }

    private void Dash()
    {
        player.dashing = true;
        if (Time.time < nextDashTime)
            return;
        Debug.Log("Dash");
        nextDashTime = Time.time + dashDelay;
        transform.Translate(lookDirection * dashForce, Space.World);
        player.dashing = false;
        //rb.AddForce(lookDirection * dashForce);
        
    }

    public void Shoot()
    {
        player.Shoot(this);
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
