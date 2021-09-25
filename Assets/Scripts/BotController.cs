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
    public Rigidbody2D rb;
    private readonly int lowTimeNum = 8;

    void Start()
    {
        player = GetComponent<Player>();
        aiPath = GetComponent<AIPath>();
        pathfinder = GetComponent<AIDestinationSetter>();
        pathfinder.enabled = false;
        rb = GetComponent<Rigidbody2D>();
        target = null;

        SetUpStateMachine();
    }

    private void SetUpStateMachine()
    {
        stateMachine = new StateMachine();

        var findNPC = new FindTag(this, "NPC");
        var pathToNPC = new PathToTarget(this);
        var dashAttackNPC = new DashAttack(player, target);
        var findPlayer = new FindTag(this, "Player");
        var findPlayerClose = new FindTag(this, "Player");
        var pathToPlayer = new PathToTarget(this);
        var dashAttackPlayer = new DashAttack(player, target);
        var weaponAttackPlayer = new WeaponAttack(player, target);

        stateMachine.AddAnyTransition(findNPC, timeLowOrNoWeapAndNoNPC());
        stateMachine.AddTransition(findNPC, pathToNPC, NPCFound());
        stateMachine.AddTransition(pathToNPC, dashAttackNPC, targetInDashRange());
        stateMachine.AddTransition(dashAttackNPC, pathToNPC, dashNotReady());
        stateMachine.AddTransition(dashAttackNPC, findPlayer, timeNotLowOrHasWeapon());
        stateMachine.AddTransition(findPlayer, pathToPlayer, playerFound());
        stateMachine.AddTransition(pathToPlayer, dashAttackPlayer, targetInDashRange());
        stateMachine.AddTransition(pathToPlayer, weaponAttackPlayer, targetInWeaponRange());
        stateMachine.AddTransition(dashAttackPlayer, pathToPlayer, dashNotReady());
        stateMachine.AddAnyTransition(findPlayerClose, playerInDashRange());
        stateMachine.AddTransition(findPlayerClose, dashAttackPlayer, playerFound());

        Func<bool> timeLowOrNoWeapAndNoNPC() => () => (TimeLow() || !HasWeapon()) && !HaveNPCTarget() && !playerInRange(player.dashForce);
        Func<bool> timeNotLowOrHasWeapon() => () => !TimeLow() || HasWeapon();
        Func<bool> NPCFound() => () => HaveNPCTarget();
        Func<bool> targetInDashRange() => () => TargetInRange(player.dashForce);
        Func<bool> dashNotReady() => () => Time.time > player.nextDashTime;
        Func<bool> playerFound() => () => HavePlayerTarget();
        Func<bool> targetInWeaponRange() => () => HasWeapon() && TargetInRange(player.weapon.range);
        Func<bool> playerInDashRange() => () => playerInRange(player.dashForce);

        stateMachine.SetState(findNPC);
       
        
    }
    private bool TimeLow() { return player.timeRemaing < lowTimeNum; }
    private bool HaveNPCTarget() { return target != null && target.tag == "NPC"; }
    private bool HavePlayerTarget() { return target != null && target.tag == "Player"; }
    private bool TargetInRange(float range) { return (target.position - transform.position).magnitude < range; }
    private bool HasWeapon() { return player.weapon != null; }
    private bool playerInRange(float range)
    {
        Collider2D[] collidersHit = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D collider in collidersHit)
            if (collider.tag == "Player" && collider != this)
                return true;
        return false;
    }
    public void Seek(bool seek)
    {
        pathfinder.enabled = seek;
    }

    public void SetTarget(Transform target)
    {
        Debug.Log(target);
        this.target = target;
    }

    void FixedUpdate()
    {
        stateMachine.Tick();

        //Look
        if (player.lookDirection.sqrMagnitude > .1f)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(player.lookDirection.y, player.lookDirection.x));
        }
        else if (rb.velocity.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(rb.velocity.y, rb.velocity.x));
        }

        pathfinder.target = target;
        if (stateMachine.GetState().GetType() != typeof(FindTag))
            Debug.Log(target.name);
    }


    public void OnDrawGizmosSelected()
    {
        Collider2D[] collidersHit = Physics2D.OverlapCircleAll(transform.position, 10 * 1);
        Gizmos.DrawWireSphere(transform.position, 10);
    }


    public enum CurrentState
    {
    }
}
