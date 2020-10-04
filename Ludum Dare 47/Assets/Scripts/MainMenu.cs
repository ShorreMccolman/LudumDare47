using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;
    void Awake()
    {
        Instance = this;
    }

    public void OnPlayGamePressed(int gameID)
    {
        GameController.Instance.StartNewGame(gameID);
    }
}
