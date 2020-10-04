using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    void Awake()
    { if(Instance == null) Instance = this; }

    Game _activeGame;
    
    public void StartNewGame(int gameID)
    {
        if(_activeGame != null)
        {
            return;
        }

        StartCoroutine(LoadGameScene(gameID));
    }

    IEnumerator LoadGameScene(int gameID)
    {
        yield return SceneManager.LoadSceneAsync(gameID);

        yield return new WaitForEndOfFrame();

        if (gameID == 0)
        {

        }
        else
        {
            _activeGame = FindObjectOfType<Game>();
            if (_activeGame == null)
                Debug.LogError("Could not find game in scene with ID " + gameID);
            else
            {
                Debug.LogError("Generating game with ID " + gameID);
                _activeGame.GenerateGame();
            }
        }
    }

    public void EndGame()
    {
        if (_activeGame == null)
            return;

        _activeGame.OnGameEnd();

        StartCoroutine(LoadGameScene(0));
    }
}
