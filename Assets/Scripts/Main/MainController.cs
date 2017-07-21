using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public bool sanitization_done = false;
    public bool corn_done = false;
    public bool yeast_done = false;
    public bool boil_done = false;

    public int score = 0;

    private void Start()
    {
        RunNext();
    }

    public void RunNext()
    {
        if (!sanitization_done)
        {
            SceneManager.LoadScene("Sanitization", LoadSceneMode.Additive);
        }
        else if (!corn_done)
        {
            SceneManager.UnloadSceneAsync("Sanitization");
            SceneManager.LoadScene("Corn_Sort", LoadSceneMode.Additive);
        }
        else if (!yeast_done)
        {
            SceneManager.LoadScene("Yeast_Eater", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("Corn_Sort");
        }
        else if (!boil_done)
        {
            SceneManager.LoadScene("Boil", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("Yeast_Eater");
        }
        else
        {
            SceneManager.LoadScene("Town", LoadSceneMode.Additive);
            SceneManager.UnloadSceneAsync("Boil");

            sanitization_done = false;
            corn_done = false;
            yeast_done = false;
            boil_done = false;
        }
    }
}
