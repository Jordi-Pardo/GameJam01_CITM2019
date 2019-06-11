using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtons : MonoBehaviour
{
    public AudioSource sound;
    public void StartGame(int sceneNum)
    {
        StartCoroutine(StartGameDelayed(sceneNum));
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator StartGameDelayed(int num)
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadSceneAsync(num);
    }
}
