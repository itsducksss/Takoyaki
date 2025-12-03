using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public Slider healthBar;
    public float healthAmount = 100f;

    private const float coef = 0.05f;
    public Collider[] hitColliders;
    private const int maxColliders = 10;
    public bool isWatered;

    public TMP_Text healthText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {                               
        hitColliders = new Collider[maxColliders];
    }

    // Update is called once per frame
    void Update()
    {
        if (isWatered == true)                                                                                                                                                                                                                                                                                
        {
            healthAmount += coef * 0.5f;
            Debug.Log("Gulp...gulp...gulp");

            if (healthAmount > 100f)
            {
                healthAmount = 100f;
                Debug.Log("Drowwnning in so much water");
            }
        }
        else
        {
            Debug.Log("THOIUSTRY WAAAWWTER");
            healthAmount -= coef * 0.05f;
        }


        //healthAmount -= coef * 0.05f; //(Change back to 0.05)
        healthBar.value = (float)healthAmount / 100f;

        healthText.text = healthAmount + "/" + 100;

        if (healthAmount <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(25);  //will prob not need this
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            Heal(25);       //water bottle pick-up
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            isWatered = true;
            //healthAmount = healthAmount + 25;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            isWatered = false;
            //healthAmount = healthAmount + 25;
        }

    }
    public void TakeDamage(float damage)
    { 
        healthAmount -= damage;
        healthBar.value = healthAmount / 100f;
    }



public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healingAmount = Mathf.Clamp(healthAmount, 0, 100);

    }
}
