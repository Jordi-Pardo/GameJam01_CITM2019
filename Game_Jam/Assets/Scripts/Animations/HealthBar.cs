using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Animator anim;
    [HideInInspector]
    public Slider healthBar,backgroundBar;
    [HideInInspector]
    public Image healthBarImage, backgroundBarImage;

    public float currentHealth;
    public float speed;

    public Color[] colors = new Color[3];
    public Color[] backgroundColors = new Color[3];

    private Color currentColor,backCurrentColor;

    public void Start()
    {     
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        HandleBar();
        HandleColors();
    }
    public void HandleColors()
    {
        healthBarImage.color = currentColor;
        backgroundBarImage.color = backCurrentColor;
        if(healthBar.value > currentHealth * 0.7f)
        {
            currentColor = colors[0];
            backCurrentColor = backgroundColors[0];
        }
        else if(healthBar.value > currentHealth * 0.3f && healthBar.value < currentHealth * 0.7f)
        {
            currentColor = colors[1];
            backCurrentColor = backgroundColors[1];
        }
        else if(healthBar.value < currentHealth * 0.3f)
        {
            currentColor = colors[2];
            backCurrentColor = backgroundColors[2];
        }
    }
    public void HandleBar()
    {
        StartCoroutine(HealthEffect());
    }
    
    public void RestHealth(float amount)
    {
        currentHealth -= amount;
        anim.SetTrigger("effect");
    }

    IEnumerator HealthEffect()
    {

        if(healthBar.value != currentHealth)
        {
            
            healthBar.value = Mathf.Lerp(healthBar.value, currentHealth, Time.deltaTime * speed);

            yield return new WaitForSeconds(0.3f);



            if (backgroundBar.value != currentHealth || healthBar.value <= 0)
            {
                healthBar.value = currentHealth;
                backgroundBar.value = Mathf.Lerp(backgroundBar.value, currentHealth, Time.deltaTime * speed);
            }
            
        }



    }
}
