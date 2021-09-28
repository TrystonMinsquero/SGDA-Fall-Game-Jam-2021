using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{
    Controls controls;
    PlayerInput playerInput;

    private void Awake()
    {
        controls = new Controls();
        playerInput = GetComponent<PlayerInput>();
    }

    public void Join()
    {
         PlayerManager.instance.OnPlayerJoined(playerInput);

    }

    public void Leave()
    {
        PlayerManager.instance.OnPlayerLeft(playerInput);
    }

    public void OnJoin(InputAction.CallbackContext ctx) => Join();
    public void OnLeave(InputAction.CallbackContext ctx) => Leave();


    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
