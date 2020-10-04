using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MatchStageInfo
{
    public readonly int Width;
    public readonly int Height;
    public readonly int Colors;

    public int Target { get { return Width * Height / 2; } }

    public MatchStageInfo(int width, int height, int colors)
    {
        Width = width;
        Height = height;
        Colors = colors;
    }
}

public struct TileMatchState
{
    public readonly bool HasPotentialMatch;
    public readonly MatchTile Tile;

    public TileMatchState(MatchTile tile)
    {
        HasPotentialMatch = tile != null;
        Tile = tile;
    }
}

public class MatchGame : Game
{
    public static MatchGame Instance;
    void Awake()
    { Instance = this; }

    [SerializeField] private GameObject MatchTilePrefab;
    [SerializeField] private GameObject StartButton;

    List<MatchTile> Tiles = new List<MatchTile>();
    MatchTile _lastUncoveredTile;

    TileMatchState _matchState;
    int _currentStage;
    int _targetScore;
    int _currentScore;

    public override void GenerateGame()
    {
        CleanGrid();

        _currentStage = -1;
        _targetScore = 0;
        _currentScore = 0;
        _matchState = new TileMatchState();
        StartButton.SetActive(true);
    }

    public void GenerateNextLevel()
    {
        CleanGrid();

        _currentStage++;
        Debug.LogError("Generating stage " + _currentStage);

        if(_currentStage == GameConstants.MatchGameStageInfo.Count)
        {
            HasCompletedGame = true;
            GameController.Instance.EndGame();
            return;
        }

        MatchStageInfo Stage = GameConstants.MatchGameStageInfo[_currentStage];

        SetupTileGrid(Stage);
        ColorizeTiles(Stage);
        InitScore(Stage);
    }

    void CleanGrid()
    {
        for(int i=0;i<Tiles.Count;i++)
        { 
            Destroy(Tiles[i].gameObject);
        }

        Tiles.Clear();
        
        _lastUncoveredTile = null;
    }

    void SetupTileGrid(MatchStageInfo stage)
    {
        int gridWidth = stage.Width;
        int gridHeight = stage.Height;

        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                GameObject tile = Instantiate(MatchTilePrefab);
                RectTransform trans = tile.GetComponent<RectTransform>();

                trans.parent = this.transform;
                trans.anchoredPosition = new Vector2((i - gridWidth / 2) * trans.sizeDelta.x, (j - gridHeight / 2) * trans.sizeDelta.y);

                Tiles.Add(tile.GetComponent<MatchTile>());
            }
        }
    }

    void ColorizeTiles(MatchStageInfo stage)
    {
        int colorCount = stage.Colors;

        List<Color> allColors = GameConstants.MatchGameColorList;
        IExtensions.Shuffle<Color>(allColors);

        List<Color> colors = new List<Color>();
        for (int i = 0; i < colorCount; i++)
            colors.Add(allColors[i]);

        int curColor = 0;
        List<Color> tileColors = new List<Color>();
        for(int i=0;i<Tiles.Count / 2;i++)
        {
            tileColors.Add(colors[curColor]);
            tileColors.Add(colors[curColor]);

            curColor = (curColor + 1) % colorCount;
        }

        IExtensions.Shuffle<Color>(tileColors);

        for(int i=0;i<Tiles.Count;i++)
        {
            Tiles[i].Setup(tileColors[i]);
        }
    }

    void InitScore(MatchStageInfo stage)
    {
        _currentScore = 0;
        _targetScore = stage.Target;
    }

    void UpdateScore()
    {
        _currentScore++;

        if (_currentScore == _targetScore)
        {
            GenerateNextLevel();
        }
    }

    public bool OnTileFlip(MatchTile tile)
    {
        if(_matchState.HasPotentialMatch)
        {
            StartCoroutine(VerifyTiles(_matchState.Tile, tile));
            _matchState = new TileMatchState();
        }
        else
        {
            _matchState = new TileMatchState(tile);
        }
        return true;
    }

    IEnumerator VerifyTiles(MatchTile first, MatchTile second)
    {
        bool isMatch = first.Color == second.Color;

        yield return new WaitForSeconds(0.6f);

        if (isMatch)
        {
            first.ConfirmMatch();
            second.ConfirmMatch();

            _lastUncoveredTile = null;

            UpdateScore();
        }
        else
        {
            first.Reset();
            second.Reset();
            _lastUncoveredTile = second;
        }
    }

    public void OnStartPressed()
    {
        StartButton.SetActive(false);
        GenerateNextLevel();
    }

    public void OnQuitPressed()
    {
        GameController.Instance.EndGame();
    }

    public override void OnGameEnd()
    {
        if (HasCompletedGame)
            PlayerPrefs.SetInt("MatchGame", 1);
    }
}
