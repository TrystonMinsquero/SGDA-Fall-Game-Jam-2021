using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public float dashDelay;
    public float dashForce;
    Camera cam;

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
        cam = GetComponentInChildren<Camera>();

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

        cam.transform.rotation = Quaternion.Euler(Vector3.zero);

    }

    private void Dash()
    {
        if (Time.time < nextDashTime)
            return;
        Debug.Log("Dash");
        nextDashTime = Time.time + dashDelay;
        transform.Translate(lookDirection * dashForce, Space.World);
        Collider2D[] collidersHit = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x / 2);
        foreach(Collider2D collider in collidersHit)
        {
            if (collider.CompareTag("Player"))
            {
                player.TakeOver(collider.GetComponent<Player>());
                break;
            }
            else if (collider.CompareTag("NPC"))
            {
                player.TakeOver(collider.GetComponent<NPC_Controller>());
                break;
            }
        }
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
