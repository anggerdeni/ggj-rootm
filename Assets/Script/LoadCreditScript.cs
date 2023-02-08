using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadCreditScript : MonoBehaviour
{
    public Animator transition;
    public void LoadMainMenu()
    {
        StartCoroutine(LoadLevel(3));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(levelIndex);
    }
}
