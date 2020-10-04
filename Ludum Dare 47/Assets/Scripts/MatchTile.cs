using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchTile : MonoBehaviour
{
    private static Color _unflippedColor = Color.white;

    [SerializeField] private Image Icon;

    Color _originalColor;

    Color _currentColor;
    public Color Color { get { return _currentColor; } private set { _currentColor = value; } }

    bool _isFlipped;

    public void Setup(Color color)
    {
        _originalColor = color;
        Reset();
    }

    public void Reset()
    {
        Icon.color = _unflippedColor;
        _currentColor = _originalColor;
        _isFlipped = false;
    }

    public void Flip()
    {
        if (_isFlipped)
            return;

        bool shouldFlip = MatchGame.Instance.OnTileFlip(this);

        if (shouldFlip)
        {
            Icon.color = _currentColor;
            _isFlipped = true;
        }
    }

    public void ConfirmMatch()
    {
        gameObject.SetActive(false);
    }

    public void Cheat(Color color)
    {
        _currentColor = color;
    }
}
