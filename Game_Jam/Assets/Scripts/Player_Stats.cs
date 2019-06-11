using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player_Stats : MonoBehaviour
{


    public float Health;

    public static Player_Stats instancia;

    public Slider HealthSlider;

    public float InitialHealth;
    public ParticleSystem systemPR;

    private void Awake()
    {
        HealthSlider.maxValue = Health;
        HealthSlider.value = Health;
        InitialHealth = Health;
        instancia = this;
    }

    private void Update()
    {
        HealthSlider.value = Health;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "DamagePlayer")
        {

            Enemy_IA IA = other.transform.root.GetComponent<Enemy_IA>();
            float Damage = IA.Damage;

            Debug.Log("Hit");

            if(Health - Damage <= 0)
            {
                
                Debug.Log("Kill");
                SceneManager.LoadSceneAsync(0);
            }
            else
            {
                Health -= Damage;
            }

            HealthSlider.value = Health;
            systemPR.Play();

        }
    }



}
