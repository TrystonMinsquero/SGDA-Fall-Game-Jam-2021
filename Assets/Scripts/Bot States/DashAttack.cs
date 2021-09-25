using UnityEngine;

public class DashAttack : IState
{
    private readonly Player player;
    private readonly Transform target;

    public DashAttack(Player player, Transform target)
    {
        this.player = player;
        this.target = target;
    }

    public void OnEnter()
    {
        Debug.Log("Dashing");
        player.lookDirection = (target.position - player.transform.position).normalized;
        player.Dash();

    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
    }
}
