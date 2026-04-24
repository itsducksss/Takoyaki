using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public Slider healthBar;
    public float healthAmount = 100f;

    //private const float coef = 0.05f; <--- not needed anymore
    public Collider[] hitColliders;
    private const int maxColliders = 10;
    public bool isWatered;
    public float gainPerSecond = 1f;
    public float lossPerSecond = 0.5f;
    public float multiplier = 1f;

    public TMP_Text healthText;

    void Awake()
    {
        hitColliders = new Collider[maxColliders];
    }

    // Update is called once per frame
    void Update()
    {
        if (isWatered)                                                                                                                                                                                                                                                                                
        {
            healthAmount += gainPerSecond * Time.deltaTime * multiplier;
            Debug.Log("Gulp...gulp...gulp");

            if (healthAmount > 100f)
            {
                healthAmount = 100f;
                Debug.Log("Drowwnning in so much water");
            }
        }
        else
        {
            //Debug.Log("THOIUSTRY WAAAWWTER");
            healthAmount -= lossPerSecond * Time.deltaTime * multiplier;
        }


        //healthAmount -= coef * 0.05f; //(Change back to 0.05)
        healthBar.value = healthAmount / 100f;

        healthText.text = $"{healthAmount:N1}/100";

        if (healthAmount <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(5);  //will prob not need this
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
