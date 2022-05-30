using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] Animator transition;
    [SerializeField] float transitionTime = 1f;

    public static LevelManager instance;
    void Start()
    {
        instance = this;
    }

    public void NextLevel()
    {
        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        if(nextScene > SceneManager.sceneCountInBuildSettings - 1)
        {
            StartCoroutine(LoadLevel(0));
        }
        else{
            StartCoroutine(LoadLevel(nextScene));
        }
    }

    public void RestartLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(levelIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
