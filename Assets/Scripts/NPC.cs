using UnityEngine;

[CreateAssetMenu(fileName = "new NPC")]
public class NPC : ScriptableObject
{
    public Sprite image;
    public AnimationClip idle;
    public AnimationClip dash;
    public AnimationClip charge;
    public AnimationClip walk;
    public AnimationClip left;
    public AnimationClip right;
    public bool hasWeapon;

    public void SwitchAnimations(Animator anim)
    {
        AnimatorOverrideController aoc = new AnimatorOverrideController(anim.runtimeAnimatorController);
        aoc["Idle"] = idle;
        aoc["Walk"] = walk;
        aoc["Dash"] = dash;
        aoc["Charge"] = charge;
        aoc["Left"] = left;
        aoc["Right"] = right;
    }

    public static void SwitchAnimations(Animator newAnim, Animator oldAnim)
    {
        AnimatorOverrideController newAoc = new AnimatorOverrideController(newAnim.runtimeAnimatorController);
        AnimatorOverrideController oldAoc = new AnimatorOverrideController(oldAnim.runtimeAnimatorController);
        newAoc["Idle"] = oldAoc["Idle"];
        newAoc["Walk"] = oldAoc["Walk"];
        newAoc["Dash"] = oldAoc["Dash"];
        newAoc["Charge"] = oldAoc["Charge"];
        newAoc["Left"] = oldAoc["Left"];
        newAoc["Right"] = oldAoc["Right"];
    }

}
