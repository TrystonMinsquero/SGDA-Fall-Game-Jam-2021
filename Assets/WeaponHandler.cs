using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    public Weapon weapon;

    private SpriteRenderer weaponSR;
    private SpriteRenderer flashSR;
    private Animator weaponAnim;
    private Animator flashAnim;

    void Start()
    {
        Set();
    }

    public void SwitchWeapons(WeaponHandler weaponHandler)
    {
        if (weaponHandler.weapon == null)
            this.weapon = null;
        else
            weapon = weaponHandler.weapon;
        Set();
    }

    public void Set()
    {
        weaponSR = GetComponent<SpriteRenderer>();
        flashSR = GetComponentInChildren<SpriteRenderer>();
        weaponAnim = GetComponent<Animator>();
        flashAnim = GetComponentInChildren<Animator>();
        if (weapon == null)
        {
            weaponSR.sprite = null;
            flashSR = null;
            weaponAnim = null;
            flashAnim = null;
        }
        else
        {
            weaponSR.color = weapon.color;
            weaponSR.sprite = weapon.sprite;
            flashSR.sprite = weapon.flashSprite;
            weaponAnim = weapon.anim;
            flashAnim = weapon.flashAnim;
            weapon.Reset();
        }
    }

    public void Shoot(Player player)
    {
        if(weapon != null)
            weapon.Shoot(player);
        //muzzle flash start

    }

    public List<Projectile> GetProjectiles()
    {
        return weapon.projectiles;
    }



}
