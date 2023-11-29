using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum GameState
{
    None,
    GameRunning,
    Paued,
    End
}
[System.Serializable]
public class DifficultyConfig
{
    public Button button;
    public GameObject background;
    public Color color;
}
public class GameManager : GenericSingleton<GameManager>
{
    [SerializeField]private DifficultyConfig easyConfig;
    [SerializeField]private DifficultyConfig mediumConfig;
    [SerializeField]private DifficultyConfig hardConfig;

    [SerializeField]private TicTacToe ticTacToe;
    [SerializeField]private GridLookHandler lookHandler;

    [SerializeField]private Canvas mainMenuCanvas;
    [SerializeField]private Canvas settingsCanvas;
    [SerializeField]private Canvas difficultyCanvas;
    [SerializeField]private Canvas pausedCanvas;
    [SerializeField]private Canvas gameplayCanvas;
    [SerializeField]private Canvas gameOverCanvas;
    [SerializeField]private Button[] uiButtons;
    [SerializeField]private Button pauseButton;
    [SerializeField]private Slider sfxVolSlider;
    [SerializeField]private Button[] restartButtons;
    [SerializeField]private TMPro.TMP_Text winningStatusText;

    private GameState _currentState = GameState.None;

    public GameState CurrentState { get => _currentState; }

    public void StartGame(Difficulty difficulty)
    {

        mainMenuCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(false);
        difficultyCanvas.gameObject.SetActive(false);
        pausedCanvas.gameObject.SetActive(false);
        mainMenuCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
        gameplayCanvas.gameObject.SetActive(true);
        
        ticTacToe.gameObject.SetActive(true);

        switch(difficulty)
        {
            case Difficulty.None: Debug.Log("No Difficulty Selected");
					 			  break;
            case Difficulty.Easy:
                                 ticTacToe.Initialize(Difficulty.Easy);
                                 easyConfig.background.SetActive(true);
                                 //Debug.Log("Easy Difficulty Selected");
                                 SoundManager.Instance.PlayMusic(SoundType.EasyLevelMusic);
                                 lookHandler.SetColor(easyConfig.color);
                                 break;

            case Difficulty.Medium:
                                   ticTacToe.Initialize(Difficulty.Medium);
                                   mediumConfig.background.SetActive(true);
                                   //Debug.Log("Medium Difficulty Selected");
								   SoundManager.Instance.PlayMusic(SoundType.MediumLevelMusic);
                                   lookHandler.SetColor(mediumConfig.color);
                                   break;
            
            case Difficulty.Hard:
                                 ticTacToe.Initialize(Difficulty.Hard);
                                 hardConfig.background.SetActive(true);
                                 //Debug.Log("Hard Difficulty Selected");
								 SoundManager.Instance.PlayMusic(SoundType.HardLevelMusic);
                                 lookHandler.SetColor(hardConfig.color);
                                 break;
            
            default:
					//Debug.Log("Something wrong happened while playing music!");
					break;
        }

        _currentState = GameState.GameRunning;
    }



    public void BackToMain()
    {
        gameOverCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(false);
        difficultyCanvas.gameObject.SetActive(false);
        pausedCanvas.gameObject.SetActive(false);
        gameplayCanvas.gameObject.SetActive(false);
        mainMenuCanvas.gameObject.SetActive(true);
        SoundManager.Instance.PlayMusic(SoundType.MainMenuTheme);

        ticTacToe.gameObject.SetActive(false);

        switch(ticTacToe.CurrentDifficulty)
        {
            case Difficulty.Easy:
                                 easyConfig.background.SetActive(false);
                                 break;
            case Difficulty.Medium:
                                 mediumConfig.background.SetActive(false);
                                 break;
            case Difficulty.Hard:
                                 hardConfig.background.SetActive(false);
                                 break;
        }
        _currentState = GameState.None;
    }
    public void EndGame(PlayerType playerType)
    {
        mainMenuCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(false);
        difficultyCanvas.gameObject.SetActive(false);
        pausedCanvas.gameObject.SetActive(false);
        gameplayCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(true);

        switch(playerType)
		{
			case PlayerType.None:  winningStatusText.text = "IT'S A TIE";
								   break;
			case PlayerType.Human: winningStatusText.text = "YOU WIN!!";
								   break;
			case PlayerType.Computer: winningStatusText.text = "YOU LOSE!!";
								   break;
		}
        
        _currentState = GameState.End;
    }

    public void PauseGame()
    {
        mainMenuCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(false);
        difficultyCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
        gameplayCanvas.gameObject.SetActive(false);
        pausedCanvas.gameObject.SetActive(true);
        SoundManager.Instance.PauseMusic();
        _currentState = GameState.Paued;
    }

    public void ResumeGame()
    {
        mainMenuCanvas.gameObject.SetActive(false);
        settingsCanvas.gameObject.SetActive(false);
        difficultyCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
        pausedCanvas.gameObject.SetActive(false);
        gameplayCanvas.gameObject.SetActive(true);
        SoundManager.Instance.ResumeMusic();
        _currentState = GameState.GameRunning;
    }

    public void RestartGame()
    {
        StartGame(ticTacToe.CurrentDifficulty);
    }

    // Start is called before the first frame update
    void Start()
    {
        /*
        easyConfig.button.onClick.AddListener(()=> ticTacToe.Initialize(Difficulty.Easy));
        mediumConfig.button.onClick.AddListener(()=> ticTacToe.Initialize(Difficulty.Medium));
        hardConfig.button.onClick.AddListener(()=> ticTacToe.Initialize(Difficulty.Hard));
        */
        
        easyConfig.button.onClick.AddListener(()=> StartGame(Difficulty.Easy));
        mediumConfig.button.onClick.AddListener(()=> StartGame(Difficulty.Medium));
        hardConfig.button.onClick.AddListener(()=> StartGame(Difficulty.Hard));

        foreach(Button uiButton in uiButtons)
        {
            uiButton.onClick.AddListener(()=> SoundManager.Instance.PlaySFX(SoundType.ButtonClick));
        }

        sfxVolSlider.onValueChanged.AddListener((float value)=> SoundManager.Instance.PlaySFX(SoundType.ButtonClick));

        foreach(Button button in restartButtons)
        {
            button.onClick.AddListener(RestartGame);
        }

        easyConfig.background.SetActive(false);
        mediumConfig.background.SetActive(false);
        hardConfig.background.SetActive(false);

        settingsCanvas.gameObject.SetActive(false);
        difficultyCanvas.gameObject.SetActive(false);
        pausedCanvas.gameObject.SetActive(false);
        gameplayCanvas.gameObject.SetActive(false);
        gameOverCanvas.gameObject.SetActive(false);
        mainMenuCanvas.gameObject.SetActive(true);

        _currentState = GameState.None;   
        ticTacToe.gameObject.SetActive(true);
        ticTacToe.gameObject.SetActive(false);     
    }


    private void OnDestroy() 
    {
        easyConfig.button.onClick.RemoveAllListeners();
        mediumConfig.button.onClick.RemoveAllListeners();
        hardConfig.button.onClick.RemoveAllListeners();

        foreach(Button uiButton in uiButtons)
        {
            uiButton.onClick.RemoveAllListeners();
        }
        sfxVolSlider.onValueChanged.RemoveAllListeners();

        foreach(Button button in restartButtons)
        {
            button.onClick.RemoveAllListeners();
        }  
    }
}
