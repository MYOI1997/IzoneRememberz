using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    public void GoMain()
    {
        SceneManager.LoadScene("Main");
    }

    public void GoFactory()
    {
        SceneManager.LoadScene("Factory");
    }

    public void GoBattle()
    {
        SceneManager.LoadScene("StageSelect");
    }

    public void GoCompilation()
    {
        SceneManager.LoadScene("Compilation");
    }
}
