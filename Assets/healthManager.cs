using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class healthManager : MonoBehaviour
{
    public Slider healthBar;
    public float healthAmount = 100f;

    private const float coef = 0.05f;

    [SerializeField] Transform _groundPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthAmount -= coef * 0.05f;
        healthBar.value = healthAmount / 100f;

        if (healthAmount <= 0)
        { 
            healthBar.gameObject.SetActive(false);
        }
        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    TakeDamage(25);
        //}
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Water")
    //    {
    //        healthAmount = healthAmount + 25;
    //    }
    //}
    //public void TakeDamage(float damage)
    //{ 
    //    healthAmount -= damage;
    //    healthBar.value = healthAmount / 100f;
    //}

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healingAmount = Mathf.Clamp(healthAmount, 0, 100);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Physics.BoxCast(_groundPoint.position, Vector3.one, Vector3.forward, out RaycastHit hit, Quaternion.identity, 2f);
        Physics.BoxCast(_groundPoint.position, Vector3.one * .5f, Vector3.down, out RaycastHit hit, Quaternion.identity);
        Collider collider = hit.collider;

        if (other.gameObject.tag == "Water")
        {
            healthAmount = healthAmount + 25;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_groundPoint.position, Vector3.one); //for the check for ground()
    }
}
