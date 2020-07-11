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

    private void Awake()
    {
        MainManager.instance = this;

        this.LoadGameScene();
    }

    private void LoadGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene", UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    public void UnloadGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("GameScene");
    }

    public void LoadEndGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("EndGameScene", UnityEngine.SceneManagement.LoadSceneMode.Additive);
    }

    public void UnloadEndGameScene()
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("EndGameScene");
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
