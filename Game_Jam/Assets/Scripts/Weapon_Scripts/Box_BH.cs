using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Box_BH : MonoBehaviour
{

    public GameObject PickUpToInstance;
    public GameObject GFX;
    public ParticleSystem System;
    public float BoxHealth = 50;
    bool Dying = false;

    public SpawnBox_Info Spawn;

    public Weapon_Scripteable[] RandomPool;

    public HealthBar Barra;
    public Slider UU, AA;

    //Cal borrar la barra despres de la animacio
    private void Awake()
    {
        UU.maxValue = BoxHealth;
        UU.value = BoxHealth;
        Barra.currentHealth = BoxHealth;

        AA.maxValue = BoxHealth;
        AA.value = BoxHealth;

    }

    public void TakeDamage(float Damage)
    {
        if (!Dying)
        {
            if (BoxHealth - Damage <= 0)
            {
                GameObject Instancia = Instantiate(PickUpToInstance, gameObject.transform.position + new Vector3(0, .5f, 0), Quaternion.identity) as GameObject;
                int RandomNum = Random.Range(0, RandomPool.Length);
                Debug.Log(RandomNum);
                Weapon_PickUp IN = Instancia.GetComponent<Weapon_PickUp>();
                IN.WeaponHolding = RandomPool[RandomNum].stats;
                IN.Enable();
                System.Play();
                GFX.SetActive(false);
                Dying = true;
                Barra.RestHealth(Damage);
                Destroy(this.gameObject, System.main.duration - 0.05f);
            }
            else
            {
                BoxHealth -= Damage;
                Barra.RestHealth(Damage);
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DamageEnemy")
        {
            //Porvisional per probar el dany
            TakeDamage(Inventory_Info.instance.HoldersWeapon[0].SlotWeapon.Damage);
            Debug.Log(BoxHealth);

        }
    }



}
