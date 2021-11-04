using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Blunt,
    Sharp,
    Burn,
    Acid,
    Magic,
    Explosion,
}

public struct Armour
{

    public int bluntProtect { get; set; }
    public int sharpProtect { get; set; }
    public int burnProtect { get; set; }
    public int acidProtect { get; set; }
    public int magicProtect { get; set; }
    public int explosionProtect { get; set; }

    public Armour(int blunt = 0, int sharp = 0, int burn = 0,
        int acid = 0, int magic = 0, int explosion = 0)
    {
        bluntProtect = blunt;
        sharpProtect = sharp;
        burnProtect = burn;
        acidProtect = acid;
        magicProtect = magic;
        explosionProtect = explosion;
    }

    public int GetDamageAfterArmour(DamageType damageType, int damage, int penetration)
    {
        int protectionValue = 0;
        //Find the correct damage type.
        switch(damageType)
        {
            case DamageType.Blunt:
                protectionValue = bluntProtect;
                break;
            case DamageType.Sharp:
                protectionValue = sharpProtect;
                break;
            case DamageType.Burn:
                protectionValue = burnProtect;
                break;
            case DamageType.Acid:
                protectionValue = acidProtect;
                break;
            case DamageType.Magic:
                protectionValue = magicProtect;
                break;
            case DamageType.Explosion:
                protectionValue = explosionProtect;
                break;
        }
        //Penetration calculator
        float penetrationProtection = (100 - penetration) * 0.01f;
        //Return damage (Rounded up)
        return Mathf.Max((int)((damage + 0.95f) * penetrationProtection), 0);
    }

}
