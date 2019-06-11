using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory_Info : MonoBehaviour
{

    public bool CanAtack = true;

    public Weapon_Scripteable InitialWeapon;

    //Pots agafar una arma dues vegades?
    //public List<WeaponStats> InInventoryWeapons = new List<WeaponStats>();

    //public WeaponStats EquipedWeapon;

    //public WeaponStats Test;

    public Player_Motor motor;

    public int WeaponSlotInUse = 0;

    public KeyCode[] ShootClick = {KeyCode.Mouse0, KeyCode.Mouse1};

    public Image HUDSprite;

    public static Inventory_Info instance;

    public Image[] Slots;

    public Slots_Info[] HoldersWeapon;

    public Text Ammotext;

    public GameObject[] MeshArmes;

    public GameObject bulletPrefab;


    //Fer que si no te mesh, son els punys
    //Fer que puguis cambiar de armes

    private void Awake()
    {

        instance = this;
        motor = gameObject.GetComponent<Player_Motor>();
        //Test.Weapon_Name = string.Empty;
        //Test.WeaponModel = null;
        //Test.Damage = 0;
        //Test.Range = 0;
        //Test.MaxAmmo = 0;
        //Test.Icon = null;
        //Test.CurrentAmmo = 0;
        //Test.WeaponType = Type.Melee;
        //Test.ReloadAmmo = 0;


        //InInventoryWeapons.Add(InitialWeapon.stats);
        HoldersWeapon[0].SlotWeapon = AddInfo(InitialWeapon.stats, HoldersWeapon[0].SlotWeapon);
        HUDSprite.sprite = HoldersWeapon[0].SlotWeapon.Icon;
        UpdateAmmotxt();
        //EquipedWeapon = HoldersWeapon[0].SlotWeapon;
    }

    private void Update()
    {

        if (CanAtack)
        {

            if(HoldersWeapon[0].SlotWeapon.WeaponType == Type.LongRange)
            {
                Player_Motor.instance.anim.SetBool("isHoldingWeapon", true);
            }
            else
            {
                Player_Motor.instance.anim.SetBool("isHoldingWeapon", false);
            }

            if (Input.anyKey)
            {
                for (int i = 1; i <= HoldersWeapon.Length; i++)
                {
                    if (Input.GetKeyDown("" + i) && HoldersWeapon[i].SlotWeapon.Weapon_Name != string.Empty)
                    {
                        Debug.Log(i);
                        WeaponStats Change = HoldersWeapon[i].SlotWeapon;
                        HoldersWeapon[i].SlotWeapon = HoldersWeapon[0].SlotWeapon;
                        HoldersWeapon[0].SlotWeapon = Change;

                        HUDSprite.sprite = HoldersWeapon[0].SlotWeapon.Icon;
                        Slots[i].sprite = HoldersWeapon[i].SlotWeapon.Icon;

                        foreach (GameObject item in MeshArmes)
                        {

                            if(HoldersWeapon[0].SlotWeapon.WeaponModel != null && item.GetComponent<MeshFilter>().name == HoldersWeapon[0].SlotWeapon.WeaponModel.name)
                            {
                                item.SetActive(true);
                            }
                            else
                            {
                                item.SetActive(false);
                            }

                        }

                        UpdateAmmotxt();

                    }
                }
            } 


            if (Input.GetKeyDown(ShootClick[0]))
            {
                if (HoldersWeapon[0].SlotWeapon.WeaponType == Type.Melee)
                {
                    if (HoldersWeapon[0].SlotWeapon.Weapon_Name == "Fists")
                    {
                        //Atack cos a cos
                        InvokeRepeating("MeleeHit", 0, HoldersWeapon[0].SlotWeapon.DelayOnShoot);
                        UpdateAmmotxt();
                    }

                    if (HoldersWeapon[0].SlotWeapon.Weapon_Name == "Sword")
                    {
                        //Atack cos a cos
                        InvokeRepeating("SwordHit", 0, HoldersWeapon[0].SlotWeapon.DelayOnShoot);
                        UpdateAmmotxt();
                    }
                }
                else
                {
                    //Atack a distancia
                    Shoot();
                    UpdateAmmotxt();
                }
            }
            else
            {
                CancelInvoke("MeleeHit");
                motor.anim.SetBool("isAttacking", false);
                CancelInvoke("SwordHit");
                motor.anim.SetBool("swordAttack", false);

            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }

        }

    }

    public void AddWeapon(WeaponStats _WeaponToAdd)
    {
        bool DontAdd = false;

        foreach (Slots_Info WS in HoldersWeapon)
        {
            if(_WeaponToAdd.Weapon_Name == WS.SlotWeapon.Weapon_Name)
            {
                DontAdd = true;
            }
        }

        if(DontAdd == false)
        {
            int i = 0;
            foreach (Slots_Info WS in HoldersWeapon)
            {
                if(WS.SlotWeapon.Damage == 0)
                {
                    //WS.SlotWeapon = _WeaponToAdd;
                    WS.SlotWeapon = AddInfo(_WeaponToAdd, WS.SlotWeapon);
                    Slots[i].sprite = _WeaponToAdd.Icon;
                    break;
                }
                i++;
            }
            UpdateAmmotxt();
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PickUp")
        {
            AddWeapon(other.GetComponent<Weapon_PickUp>().WeaponHolding);
            Destroy(other.gameObject);
            UpdateAmmotxt();
        }
    }

    void ChangeWeaponSlot()
    {

    }

    void MeleeHit()
    {
        motor.anim.SetBool("isAttacking", true);
    }

    void SwordHit()
    {
        motor.anim.SetBool("swordAttack", true);
    }

    void Shoot()
    {

        //La municio es modifica en els prefabs tambe lel

        if (HoldersWeapon[0].SlotWeapon.CurrentAmmo > 0)
        {
            HoldersWeapon[0].SlotWeapon.CurrentAmmo--;
            GameObject bulletPosition = GameObject.FindGameObjectWithTag("holder");
            bulletPosition.GetComponent<ParticleSystem>().Play();
            GameObject Bullet = Instantiate(bulletPrefab, bulletPosition.transform.position, bulletPosition.transform.rotation) as GameObject;
            Bullet.GetComponent<BulletMovement>().Spawn = bulletPosition.transform;
            Bullet.GetComponent<BulletMovement>().LifeTime = HoldersWeapon[0].SlotWeapon.Range;
            Bullet.transform.LookAt(bulletPosition.transform);

        }
        else
        {
            Reload();
        }


        UpdateAmmotxt();

    }

    void Reload()
    {
        if (HoldersWeapon[0].SlotWeapon.MaxAmmo >= HoldersWeapon[0].SlotWeapon.ReloadAmmo - HoldersWeapon[0].SlotWeapon.CurrentAmmo)
        {
            //Fer que la R serveixi per recarregar
            int Refill = HoldersWeapon[0].SlotWeapon.ReloadAmmo - HoldersWeapon[0].SlotWeapon.CurrentAmmo;
            HoldersWeapon[0].SlotWeapon.CurrentAmmo += Refill;
            HoldersWeapon[0].SlotWeapon.MaxAmmo -= Refill;
        }
        UpdateAmmotxt();
    }

    void UpdateAmmotxt()
    {
        WeaponStats First = HoldersWeapon[0].SlotWeapon;
        if (First.WeaponType == Type.LongRange)
        {
            Ammotext.text = "Ammo: " + First.CurrentAmmo + " / " + First.MaxAmmo;
        }
        else
        {
            Ammotext.text = "";
        }
    }

    WeaponStats AddInfo(WeaponStats Input, WeaponStats Output)
    {

        //WeaponStats Output = Test;

        Output.Weapon_Name = Input.Weapon_Name;
        Output.WeaponModel = Input.WeaponModel;
        Output.Damage = Input.Damage;
        Output.Range = Input.Range;
        Output.MaxAmmo = Input.MaxAmmo;
        Output.Icon = Input.Icon;
        Output.CurrentAmmo = Input.CurrentAmmo;
        Output.WeaponType = Input.WeaponType;
        Output.ReloadAmmo = Input.ReloadAmmo;
        Output.DelayOnShoot = Input.DelayOnShoot;

        return Output;

    }

}


