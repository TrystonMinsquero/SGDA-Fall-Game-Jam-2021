using Pathfinding;
using UnityEngine;
using System;

public class BotController : MonoBehaviour
{
    StateMachine stateMachine;
    Player player;
    AIPath aiPath;
    AIDestinationSetter pathfinder;
    CurrentState currentState;

    public Transform target;
    private readonly int lowTimeNum = 8;

    void Start()
    {
        player = GetComponent<Player>();
        aiPath = GetComponent<AIPath>();
        pathfinder = GetComponent<AIDestinationSetter>();
        pathfinder.enabled = false;
        target = null;

        SetUpStateMachine();
    }

    private void SetUpStateMachine()
    {
        stateMachine = new StateMachine();

        var findNPC = new FindTag(this, "NPC");
        var pathWithinDashRange = new PathToTarget(player, pathfinder, target);
        var dashAttack = new DashAttack(player, target);
        var findPlayer = new FindTag(this, "Player");
        var pathWithinWeaponRange = new PathToTarget(player, pathfinder, target, player.weapon);
        var weaponAttack = new WeaponAttack(player, target, player.weapon);

        stateMachine.AddAnyTransition(findNPC, timeLowAndNoNPC());
        stateMachine.AddTransition(findNPC, pathWithinDashRange, NPCFound());
        stateMachine.AddTransition(pathWithinDashRange, dashAttack, targetinDashRange());
        stateMachine.AddTransition(dashAttack, pathWithinDashRange, dashNotReady());
        stateMachine.AddTransition(dashAttack, findPlayer, timeNotLow());
        stateMachine.AddTransition(findPlayer, pathWithinWeaponRange, playerFound());
        stateMachine.AddTransition(pathWithinWeaponRange, weaponAttack, targetInWeaponRange());

        Func<bool> timeLowAndNoNPC() => () => player.timeRemaing < lowTimeNum && !HaveNPCTarget();
        Func<bool> timeNotLow() => () => player.timeRemaing > lowTimeNum;
        Func<bool> NPCFound() => () => HaveNPCTarget();
        Func<bool> targetinDashRange() => () => TargetInRange(player.dashForce);
        Func<bool> dashNotReady() => () => Time.time > player.nextDashTime;
        Func<bool> playerFound() => () => HavePlayerTarget();
        Func<bool> targetInWeaponRange() => () => TargetInRange(player.weapon.range);

        stateMachine.SetState(findNPC);
       
        
    }
    private bool HaveNPCTarget() { return target != null && target.GetComponent<NPC>() != null; }
    private bool HavePlayerTarget() { return target != null && target.GetComponent<Player>() == null; }
    private bool TargetInRange(float range) { return (target.position - transform.position).magnitude < range; }
    public 

    void FixedUpdate()
    {
        stateMachine.Tick();
        pathfinder.target = target;
        if (stateMachine.GetState().GetType() != typeof(FindTag))
            Debug.Log(target.name);
    }


    public enum CurrentState
    {
    }
}
