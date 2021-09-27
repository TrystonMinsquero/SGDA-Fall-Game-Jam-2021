using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;

public class Player : MonoBehaviour
{
    public float deathTime_MAX;
    public float dashDamage;
    public float movementSpeedInit;
    public float dashDistance;
    public float dashChargeTime;
    public float dashSpeed;
    public HealthBar healthBar;

    private SpriteRenderer sr;
    private Animator anim;
    private Rigidbody2D rb;
    [HideInInspector]
    public WeaponHandler weapon;

    [HideInInspector]
    public Vector2 lookDirection;
    private Vector3 healthBarPos;
    [HideInInspector]
    public float timeRemaing;
    [HideInInspector]
    public float movementSpeed;
    private float deathTime;
    private bool charging;
    private bool charged;
    private bool dashing;

    private void Start()
    {
        weapon = null;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        weapon = GetComponentInChildren<WeaponHandler>();
        deathTime = Time.time + deathTime_MAX;
        healthBarPos = healthBar.transform.position - transform.position;
        movementSpeed = movementSpeedInit;
    }

    private void Update()
    {
        if(Time.time > deathTime) 
            Die();
        timeRemaing = deathTime - Time.time;

        healthBar.SetHealth(timeRemaing / deathTime_MAX);
        healthBar.transform.position = transform.position + healthBarPos;

        if (charged)
        {
            StartCoroutine(Dash());
        }

        rb.angularVelocity = 0;
    }

    public void Move(Vector2 input)
    {
        if (!dashing && !charging )
        {
            rb.velocity = new Vector2(input.x, input.y) * movementSpeed;
            rb.drag = input.sqrMagnitude > 0 ? 0 : 1;
        }
    }

    public void Look(Vector2 input)
    {
        if (!dashing)
        {
            Vector2 movementDirection = rb.velocity.normalized;
            //Look
            if (input.sqrMagnitude > .1f)
            {
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(input.y, input.x) + 90);
                lookDirection = input;
            }
            else if (movementDirection.sqrMagnitude > 0 && !charging && !charged)
            {
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(movementDirection.y, movementDirection.x) + 90);
                lookDirection = movementDirection;
            }

        }
    }

    public void Shoot()
    {
        if(weapon != null && !charging && !dashing)
        {
            weapon.Shoot(this);
        }
    }

    public void StartDash()
    {
        if (dashing || charging && !charged)
            return;
        Debug.Log("Dash");
        StartCoroutine(ChargeDash(dashChargeTime));

    }

    private IEnumerator ChargeDash(float chargeTime)
    {
        charging = true;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(chargeTime);
        charged = true;
        charging = false;
    }
    private IEnumerator Dash()
    {
        rb.drag = 0;
        charged = false;
        dashing = true;

        Vector3 startPos = transform.position;
        Debug.Log(lookDirection);
        rb.velocity = lookDirection * dashSpeed; //initial velocity added
        float maxDashTime = (dashDistance / rb.velocity.magnitude) + .3f; //Estimated
        float timeStarted = Time.time;
        while((transform.position - startPos).magnitude < dashDistance && Time.time - timeStarted < maxDashTime)
            yield return null;
        Debug.Log("Actual Time: " + (Time.time - timeStarted));
        Debug.Log("Estimated Time: " + maxDashTime);
        rb.velocity = Vector2.zero;
        dashing = false;
        movementSpeed = movementSpeedInit;
    }

    public void TakeOver(NPC_Controller npc_c)
    {
        NPC npc = npc_c.npc;
        Debug.Log("Take Over " + npc.name);
        sr.sprite = npc.image;
        anim = npc.anim;
        weapon.SwitchWeapons(npc_c.weaponHandler);
        deathTime = Time.time + deathTime_MAX;
        Destroy(npc_c.gameObject);

    }

    public void TakeOver(Player player)
    {
        Debug.Log("Take Over " + player.name);
        if (Time.time < player.deathTime - dashDamage)
        {
            player.TakeDamage(dashDamage);
            return;
        }
        weapon.SwitchWeapons(player.weapon);
        sr.sprite = player.sr.sprite;
        anim = player.anim;
        deathTime = Time.time + deathTime_MAX;
        player.Die();

    }

    public Vector2 GetVelocity() { return rb.velocity; }

    public void TakeDamage(float damage)
    {
        if (damage <= 0)
            Die();
        else
            deathTime -= damage;
    }

    private void Die()
    {
        Debug.Log("Die");
        Destroy(this.gameObject);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (dashing)
        {
            if (collision.gameObject.CompareTag("Player") && collision.gameObject != this.gameObject)
            {
                TakeOver(collision.gameObject.GetComponent<Player>());
            }
            else if (collision.gameObject.CompareTag("NPC"))
            {
                TakeOver(collision.gameObject.GetComponent<NPC_Controller>());
            }
        }
            
    }
}
