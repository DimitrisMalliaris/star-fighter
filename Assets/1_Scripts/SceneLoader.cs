using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] int startMenuSceneBuildIndex = 0;
    [SerializeField] int gameOverSceneBuildIndex = 1;
    [SerializeField] int gameSceneBuildIndex = 2;
    [SerializeField] float gameOverDelay = 1f;

    public void LoadNextScene()
    {
        var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        var maxSceneIndex = SceneManager.sceneCount;

        if (nextSceneIndex >= maxSceneIndex)
        {
            Debug.Log(startMenuSceneBuildIndex);
            SceneManager.LoadScene(startMenuSceneBuildIndex);
        }
        else
        {
            Debug.Log(nextSceneIndex);
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    public void LoadGameOver()
    {
        Debug.Log(gameOverSceneBuildIndex);
        StartCoroutine(GameOverTransition());
    }

    IEnumerator GameOverTransition()
    {
        yield return new WaitForSeconds(gameOverDelay);
        SceneManager.LoadScene(gameOverSceneBuildIndex);
    }

    public void LoadGameScene()
    {
        Debug.Log(gameSceneBuildIndex);
        SceneManager.LoadScene(gameSceneBuildIndex);

        var gameSession = FindObjectOfType<GameSession>();
        if (gameSession)
        {
            gameSession.ResetGame();
        }
    }

    public void LoadStartMenu()
    {
        Debug.Log(startMenuSceneBuildIndex);
        SceneManager.LoadScene(startMenuSceneBuildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
