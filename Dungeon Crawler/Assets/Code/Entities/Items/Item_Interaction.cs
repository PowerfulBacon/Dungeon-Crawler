using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Code handling the interaction between items and the worlds.
/// Mostly when players have picked up the items and are attacking with them or using them.
/// </summary>
public partial class Item : Entity
{

    //======== ATTACK STUFF =========

    public DamageType damageType { get; set; } = DamageType.Blunt;
    public int force { get; set; } = 0;

    //In seconds
    public float attackTime { get; set; } = 0.3f;

    private static Quaternion defaultQuaternion = Quaternion.Euler(0, 0, 0);
    private static Quaternion rotatedQuaternion = Quaternion.Euler(70, 0, 0);

    /// <summary>
    /// Called when mob (M) attempted to attack with the item.
    /// </summary>
    public virtual void OnAttack(Mob m)
    {
        
    }

    /// <summary>
    /// Virtual since certain things like bows may override this to have different animation sequences.
    /// </summary>
    /// <param name="m"></param>
    /// <returns></returns>
    protected virtual IEnumerator AttackAnimationSequence(Mob m)
    {
        yield return RotateAnimate(defaultQuaternion, rotatedQuaternion, attackTime);
        OnAttack(m);
        yield return RotateAnimate(rotatedQuaternion, defaultQuaternion, 0.05f);
    }

    public virtual IEnumerator RotateAnimate(Quaternion start, Quaternion end, float totalTime)
    {
        float animationTime = 0;
        while(animationTime < totalTime)
        {
            transform.localRotation = Quaternion.Slerp(defaultQuaternion, rotatedQuaternion, animationTime / totalTime);
            animationTime += Time.deltaTime;
            yield return false;
        }
        //For safety
        transform.localRotation = end;
    }

    public virtual void StartAttack(Mob m)
    {
        StartCoroutine(AttackAnimationSequence(m));
        OnAttack(m);
    }


    //======== End Attack Stuff =========

    //Called when item is used by holder (Inventory > Use)
    public virtual void OnUse(Mob m)
    {
    }

    /// <summary>
    /// Returns a list of the dropdown options to display when right clicked on.
    /// </summary>
    /// <returns></returns>
    public virtual DropdownOption[] GetInteractionOptions()
    {
        return new DropdownOption[] {
            new DropdownOption("Equip", EquipItem),
            new DropdownOption("Examine", ExamineItem),
            new DropdownOption("Drop", DropItem),
        };
    }

    /// <summary>
    /// Callback to equip the item
    /// </summary>
    private void EquipItem()
    {
        Player.myPlayer.inventory.InsertIntoHotbar(Player.myPlayer.inventory.hotbarNum, inventorySlot);
        //Player.myPlayer.SetHeldItem(this, false);
    }

}
