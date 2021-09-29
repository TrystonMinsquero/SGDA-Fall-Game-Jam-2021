using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUI : MonoBehaviour
{
    Controls controls;
    [HideInInspector]
    public PlayerInput playerInput;
    public Behaviour[] behaviours;
    

    private void Awake()
    {
        controls = new Controls();
        playerInput = GetComponent<PlayerInput>();
        DontDestroyOnLoad(this);

    }


    public void Enable()
    {
        foreach (Behaviour component in behaviours)
            component.enabled = true;

        Player player = GetComponent<Player>();
        player.AssignComponents();
        player.sr.enabled = true;
        player.weaponHandler.weaponSR.enabled = true;
        player.weaponHandler.flashSR.enabled = true;
        GetComponent<PlayerController>().EnableControls(true);
        this.enabled = false;
    }
    public void Disable()
    {
        GetComponent<PlayerController>().EnableControls(false);
        foreach (Behaviour component in behaviours)
            component.enabled = false;
        GetComponent<Player>().sr.enabled = false;
        GetComponent<Player>().weaponHandler.weaponSR.enabled = false;
        GetComponent<Player>().weaponHandler.flashSR.enabled = false;

        this.enabled = true;
    }

    //UI Actions
    public void Join()
    {
        if (PlayerManager.inLobby)
            PlayerManager.instance.OnPlayerJoined(playerInput);

    }

    public void Leave()
    {
        if (PlayerManager.inLobby)
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
