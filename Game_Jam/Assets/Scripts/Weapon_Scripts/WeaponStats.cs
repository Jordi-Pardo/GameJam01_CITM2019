using UnityEngine;

[System.Serializable]
public class WeaponStats
{
    public string Weapon_Name;

    public Mesh WeaponModel;

    public float Damage;

    public float Range;

    public int MaxAmmo;

    public int CurrentAmmo;

    public Sprite Icon;

    public Type WeaponType;

    public int ReloadAmmo;

    public float DelayOnShoot;

}

public enum Type
{
    Melee, LongRange
}