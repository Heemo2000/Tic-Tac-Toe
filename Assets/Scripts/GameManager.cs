using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]private Button easyButton;
    [SerializeField]private Button mediumButton;
    [SerializeField]private Button hardButton;
    [SerializeField]private TicTacToe ticTacToe;
    [SerializeField]private Canvas initialCanvas;
    public void StartGame()
    {
        initialCanvas.gameObject.SetActive(false);
        ticTacToe.gameObject.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        easyButton.onClick.AddListener(StartGame);
        mediumButton.onClick.AddListener(StartGame);
        hardButton.onClick.AddListener(StartGame);

        easyButton.onClick.AddListener(()=> ticTacToe.SetDifficulty(Difficulty.Easy));
        mediumButton.onClick.AddListener(()=> ticTacToe.SetDifficulty(Difficulty.Medium));
        hardButton.onClick.AddListener(()=> ticTacToe.SetDifficulty(Difficulty.Hard));
    }


    private void OnDestroy() 
    {
        easyButton.onClick.RemoveAllListeners();
        mediumButton.onClick.RemoveAllListeners();
        hardButton.onClick.RemoveAllListeners();    
    }
}
