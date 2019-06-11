using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy_IA : MonoBehaviour
{

    public NavMeshAgent Agent;
    public GameObject target;
    public Animator anim;

    public static float BaseDamage = 5;
    public static float BaseHealth = 50;

    public ParticleSystem STA;

    public float Damage;
    public float Health;

    public HealthBar Bar;

    public Slider AA, UU;

    private void Start()
    {
        Agent = this.gameObject.GetComponent<NavMeshAgent>();
        target = Inventory_Info.instance.gameObject;

        Damage = BaseDamage;
        Health = BaseHealth;

        AA.maxValue = Health;
        AA.value = AA.maxValue;

        UU.maxValue = Health;
        UU.value = UU.maxValue;

    }

    private void Update()
    {

        if (Agent.velocity != Vector3.zero)
        {

            anim.SetBool("isRunning", true);
            anim.SetBool("isAttacking", false);

        }
        else
        {

            anim.SetBool("isRunning", false);
            //Atacka
            anim.SetBool("isAttacking", true);

        }

        Agent.SetDestination(target.transform.position);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DamageEnemy")
        {

            float Damage = Inventory_Info.instance.HoldersWeapon[0].SlotWeapon.Damage;

            if (Health - Damage <= 0)
            {
                //Kill Player
                RoundManager.instancia.ZombiesAlive--;
                STA.transform.parent = null;
                Destroy(STA.transform.root.gameObject, STA.main.duration);
                Destroy(gameObject);
            }
            else
            {
                Health -= Damage;
            }

            Bar.RestHealth(Damage);

            STA.Play();

        }

    }


}
