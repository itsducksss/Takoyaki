using UnityEngine;
using UnityEngine.SceneManagement;
public class PauseMenu : MonoBehaviour
{
    public GameObject container;
    private void Start()
    {
        container.SetActive(false);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            container.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;

        }
    }
    public void ResumeButton()
    {
        container.SetActive(false);
        Debug.Log("unpaused");
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
