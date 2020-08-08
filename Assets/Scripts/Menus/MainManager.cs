using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    private static MainManager instance = null;
    public static MainManager Instance
    {
        get
        {
            return MainManager.instance;
        }
    }

    public int finalScore = 0;
    public MetaRules GameConfiguration = null;

    private int GameSceneBuildIndex = 1;
    private int EndGameSceneIndex = 2;

    public static void LoadMainSceneIfNecessary()
    {
        Debug.Log("Loading main scene");
        bool isMainSceneLoaded = false;
        int numberOfScene = UnityEngine.SceneManagement.SceneManager.sceneCount;
        for (int index = 0; index < numberOfScene; ++index)
        {
            UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(index);
            if (scene.buildIndex == 0)
            {
                isMainSceneLoaded = true;
                break;
            }
        }

        if (!isMainSceneLoaded)
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(0, UnityEngine.SceneManagement.LoadSceneMode.Additive);
        }
    }

    private void Awake()
    {
        MainManager.instance = this;

        bool isGameSceneLoaded = false;
        int numberOfScene = UnityEngine.SceneManagement.SceneManager.sceneCount;
        for (int index = 0; index < numberOfScene; ++index)
        {
            UnityEngine.SceneManagement.Scene scene = UnityEngine.SceneManagement.SceneManager.GetSceneAt(index);
            if (scene.buildIndex == this.GameSceneBuildIndex)
            {
                isGameSceneLoaded = true;
                break;
            }
        }

        if (!isGameSceneLoaded)
        {
            this.LoadGameScene();
        }
    }

    private void LoadGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(this.GameSceneBuildIndex, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    public void UnloadGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(this.GameSceneBuildIndex);
    }

    public void LoadEndGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(this.EndGameSceneIndex, UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    public void UnloadEndGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(this.EndGameSceneIndex);
    }

    public void NotifyEndGame(int score)
    {
        this.finalScore = score;
        this.UnloadGameScene();
        this.LoadEndGameScene();
    }

    public void RequestRestart()
    {
        this.finalScore = 0;
        this.UnloadEndGameScene();
        this.LoadGameScene();
    }
}
