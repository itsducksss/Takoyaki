using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class healthBar : MonoBehaviour
{
    private static float value;

    public Slider healthSlider;

    public TMP_Text healthText;

    public int health = 100;

    public int maxHealth = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = health + "/" + maxHealth;
        //fix this so that it calls the function which would be to update the health itself double check
        healthBar.value = (float)health / (float)maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "water")
        {
            health = health + 25;
        }

        else
        {
            health = health - 25;
        }
    }
}
