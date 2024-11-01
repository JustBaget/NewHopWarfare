using UnityEngine.SceneManagement;
using UnityEngine;

public class Restart : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        RestartThing();
    }

    void RestartThing()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}