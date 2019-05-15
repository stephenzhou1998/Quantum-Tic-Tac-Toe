using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager2 : MonoBehaviour
{
    public void pvp()
    {
        SceneManager.LoadScene("PVP");
    }

    public void pvb()
    {
        SceneManager.LoadScene("PVB");
    }

    public void quit()
    {
        Application.Quit();
    }
}
