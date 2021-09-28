using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    public static PlayerInput[] players = new PlayerInput[4];
    public static int playerCount;

    public PlayerInput[] _players = new PlayerInput[4];
    public int _playerCount;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
            instance = this;
        playerCount = 0;
    }

    // Start is called before the first frame update
    void Update()
    {
        _players = players;
        _playerCount = playerCount;
    }

    public static bool Contains(PlayerInput _player)
    {
        foreach (PlayerInput player in players)
            if (player == _player)
                return true;
        return false;
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        AddPlayer(playerInput);
        LobbyManager.instance.UpdateJoinBoxes();
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        Remove(playerInput);
        LobbyManager.instance.UpdateJoinBoxes();
    }

    public static int NextPlayerSlot()
    {
        for (int i = 0; i < players.Length; i++)
            if (players[i] == null)
                return i;
        return -1;
    }

    public static void AddPlayer(PlayerInput playerInput)
    {
        if (Contains(playerInput))
            return;
        if (playerCount < 0 || playerCount >= players.Length)
        {
            Destroy(playerInput.gameObject);
            return;
        }
        Debug.Log("Player Joined: " + playerInput.name);
        players[NextPlayerSlot()] = playerInput;
        
        playerCount++;
    }



    public static void Remove(PlayerInput playerInput)
    {
        for(int i = 0; i < playerCount; i++)
        {
            if (players[i] == playerInput)
            {
                players[i] = null;
                Debug.Log("Player Left: " + playerInput.name);
                Destroy(playerInput.gameObject);
                playerCount--;
            }
                
        }
    }
}
