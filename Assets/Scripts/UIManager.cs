using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverText;
    public GameObject _GameOverText => _gameOverText;

    [SerializeField] private GameObject _youWonText;
    public GameObject _YouWonText => _youWonText;

    public void GameOverTextHide()
    {
        _gameOverText.SetActive(false);
    }

    public void YouWonTextHide()
    {
        _youWonText.SetActive(false);
    }

    public void GameOverTextShow()
    {
        _gameOverText.SetActive(true);
    }

    public void YouWonTextShow()
    {
        _youWonText.SetActive(true);
    }

    public void Restart()
    {
        GameOverTextHide();
        YouWonTextHide();
    }
}
