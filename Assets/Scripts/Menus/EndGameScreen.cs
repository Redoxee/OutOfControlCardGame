using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndGameScreen : MonoBehaviour
{
    [SerializeField]
    private BorderComponent RetryButton = null;

    [SerializeField]
    private TextMeshPro scoreLabel = null;

    // Start is called before the first frame update
    void Start()
    {
        MainManager mainManager = MainManager.Instance;
        if (mainManager == null)
        {
            return;
        }

        this.scoreLabel.text = $"Final Score : {mainManager.finalScore}";

        this.RetryButton.OnPressed += this.OnRetryPressed;
    }

    private void OnDisable()
    {
        this.RetryButton.OnPressed -= this.OnRetryPressed;
    }

    private void OnRetryPressed(BorderComponent button, bool ison)
    {
        MainManager mainManager = MainManager.Instance;
        if (mainManager == null)
        {
            return;
        }

        mainManager.RequestRestart();
    }
}
