using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public bool HasCompletedGame { get; protected set; }

    public virtual void GenerateGame()
    {

    }

    public virtual void ResetGame()
    {

    }

    public virtual void OnGameEnd()
    {

    }
}
