using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

using Gyroscope = UnityEngine.InputSystem.Gyroscope;

public enum PlayerType
{
    None = -1,
    Human = 0,
    Computer = 1
}
public enum Side
{
    Circle = 0,
    Cross = 1
}

public enum Difficulty
{
	None = -1,
	Easy = 0,
	Medium = 1,
	Hard = 2
}

public class TicTacToe : MonoBehaviour
{
    [SerializeField]private List<TextContainerRow> textContainers = new List<TextContainerRow>();
	[SerializeField]private Side humanSide = Side.Circle;
	
	[SerializeField]private float detectDistance = 50f;
	[SerializeField]private LayerMask textContainerMask;

	

	[Header("Settings for difficulty:")]
	[Range(0.0f,1.0f)]
	[SerializeField]private float easyRandomMoveProb = 0.9f;

	[Range(0.0f,1.0f)]
	[SerializeField]private float medRandomMoveProb = 0.5f;

	[Range(0.0f,1.0f)]
	[SerializeField]private float hardRandomMoveProb = 0.2f;

	[Header("Settings for device orientation(only mobile): ")]
	[Min(0f)]
	[SerializeField]private float rotationSpeed = 20f;
	[SerializeField]private Vector2 minAngle = Vector2.zero;
	[SerializeField]private Vector2 maxAngle = Vector2.zero;

    private List<List<PlayerType>> _board;

	private PlayerInput _playerInput;
	private int _maxDepth = int.MinValue;
	private int _functionCalls = 0;
	private PlayerType _winner = PlayerType.None;
	private bool _quitGame = false;
	private Difficulty _difficulty = Difficulty.None;
	private Vector2 _mousePosition = Vector2.zero;
	private Vector2 _targetDeviceOrientation = Vector2.zero;
	private Vector2 _currentDeviceOrientation = Vector2.zero;

	public void OnScreenClick(InputAction.CallbackContext context)
	{
		Debug.Log("OnScreenClick is called");
		
		if(_quitGame)
		{
			return;
		}

		bool pressed = context.ReadValue<float>() == 1;

		if(pressed)
		{
				if(!(_winner == PlayerType.None && CountVacants() > 0))
				{
					switch(_winner)
					{
						case PlayerType.Human: Debug.Log("Player Wins");
											   break;
						
						case PlayerType.Computer: Debug.Log("Computer Wins");
												  break;
					}
					return;
				}

				//Debug.Log("Mouse position using old system: " + Input.mousePosition);

				Debug.Log("Mouse Position using new system: " + _mousePosition);
				Ray ray = _playerInput.camera.ScreenPointToRay(_mousePosition);

				if(Physics.Raycast(ray, out RaycastHit hit, detectDistance, textContainerMask.value))
				{
					TextContainer textContainer = hit.transform.GetComponent<TextContainer>();

					for(int i = 0; i < textContainers.Count; i++)
					{
						var row = textContainers[i];
						int j = row.elements.FindIndex(0,row.elements.Count,(TextContainer container)=> textContainer.Equals(container));
						if(j != -1 && _board[i][j] == PlayerType.None)
						{
							_board[i][j] = PlayerType.Human;
							PrintRequiredText(_board[i][j],textContainer);
							switch(_difficulty)
							{
								case Difficulty.Easy:
													 ChooseMove(easyRandomMoveProb);
													 break;
								
								case Difficulty.Medium:
													 ChooseMove(medRandomMoveProb);
													 break;
													
								case Difficulty.Hard:
													 ChooseMove(hardRandomMoveProb);
													 break;
							}
							
							textContainer.OnClick?.Invoke();
							break;
						}
					}
				}

				if (Winning(PlayerType.Human))
			 	{
					_winner = PlayerType.Human;
			 	}
			 	else if (Winning(PlayerType.Computer))
			 	{
					_winner = PlayerType.Computer;
			 	}
				
		}	
	}

	public void OnGetScreenPosition(InputAction.CallbackContext context)
	{
		Debug.Log("OnGetScreenPosition is called");
		_mousePosition = context.ReadValue<Vector2>();
	}

	public void OnDeviceRotate(InputAction.CallbackContext context)
	{
		Debug.Log("OnDeviceRotation was called");
		Vector3 angularVelocity = context.ReadValue<Vector3>();
		
		_targetDeviceOrientation.x = Mathf.Clamp(_currentDeviceOrientation.x + angularVelocity.x, minAngle.x, maxAngle.x);
		_targetDeviceOrientation.y = Mathf.Clamp(_currentDeviceOrientation.y + angularVelocity.y, minAngle.y, maxAngle.y);

		Debug.Log("Target Device Rotation : " + _targetDeviceOrientation);
		

	}
	public void SetDifficulty(Difficulty difficulty)
	{
		_difficulty = difficulty;
	}

    private int CountVacants()
    {
        int result = 0;
		 for (int i = 0; i < 3; i++)
		 {
			 for (int j = 0; j < 3; j++)
			 {
				 if (_board[i][j] == PlayerType.None)
				 {
					 result++;
				 }
			 }
		 }

		 return result;
    }

    private bool Winning(PlayerType player)
	 {
		 bool result = false;
		 //Horizontal
		 if (
			 (_board[0][0] == player && _board[0][1] == player && _board[0][2] == player) ||
			 (_board[1][0] == player && _board[1][1] == player && _board[1][2] == player) ||
			 (_board[2][0] == player && _board[2][1] == player && _board[2][2] == player)
			 )
		 {
			 result = true;
		 }
		 //Vertical
		 else if (
			 (_board[0][0] == player && _board[1][0] == player && _board[2][0] == player) ||
			 (_board[0][1] == player && _board[1][1] == player && _board[2][1] == player) ||
			 (_board[0][2] == player && _board[1][2] == player && _board[2][2] == player)
			 )
		 {
			 result = true;
		 }

		 //Diagonals
		 else if (
			 (_board[0][0] == player && _board[1][1] == player && _board[2][2] == player) ||
			 (_board[0][2] == player && _board[1][1] == player && _board[2][0] == player)
			 )
		 {
			 result = true;
		 }

		 return result;
	 }

    private bool IsItATie()
	 {
		 if (Winning(PlayerType.Human))
		 {
			 return false;
		 }

		 else if (Winning(PlayerType.Computer))
		 {
			 return false;
		 }

		 return CountVacants() == 0;
	 }

	private void PrintRequiredText(PlayerType playerType, TextContainer textContainer)
	{
		switch(playerType)
        {
			case PlayerType.None:
									textContainer.SetText("");
									break;
        	case PlayerType.Human:
        	                        if(humanSide == Side.Circle)
        	                        {
        	                            textContainer.SetText("O");
        	                        }
        	                        else
        	                        {
        	                            textContainer.SetText("X");
        	                        }
        	                        break;
        	case PlayerType.Computer:
        	                        if(humanSide == Side.Circle)
        	                        {
        	                            textContainer.SetText("X");
        	                        }
        	                        else
        	                        {
        	                            textContainer.SetText("O");
        	                        }
        	                        break;

        }

		textContainer.OnClick?.Invoke();
	}
    

	private void ChooseMove(float randomMoveProbability)
	{
		if(CountVacants() == 0)
		{
			return;
		}
		Random.InitState((int)System.DateTime.Now.Ticks);

		float probability = Random.value;
		Debug.Log("Probability: " + probability);

		if(probability <= randomMoveProbability)
		{
			RandomMove();
		}
		else
		{
			BestMove();
		}
	}

	 private void RandomMove()
	 {
		Debug.Log("RandomMove called");
		Random.InitState((int)System.DateTime.Now.Ticks);

		int moveX = Random.Range(0,3);
		int moveY = Random.Range(0,3);

		while(_board[moveX][moveY] != PlayerType.None)
		{
			Debug.Log("RandomMove running");
			moveX = Random.Range(0,3);
			moveY = Random.Range(0,3);
		}

		_board[moveX][moveY] = PlayerType.Computer;
		PrintRequiredText(_board[moveX][moveY], textContainers[moveX].elements[moveY]);
	 }
	 private void BestMove()
	 {
		 _functionCalls = 0;
		 _maxDepth = 0;
		 int bestScore = int.MinValue;
		 int moveX = -1;
		 int moveY = -1;

		 for (int i = 0; i < 3; i++)
		 {
			 for (int j = 0; j < 3; j++)
			 {
				 if (_board[i][j] == PlayerType.None)
				 {
					 _board[i][j] = PlayerType.Computer;
					 int score = Minimax(0, false,int.MinValue,int.MaxValue);
					 _board[i][j] = PlayerType.None;
					 if (score > bestScore)
					 {
						 bestScore = score;
						 moveX = i;
						 moveY = j;
					 }
				 }
			 }
		 }

		Debug.Log("Minimax Function Calls: " + _functionCalls);
		Debug.Log("Max Depth: " + _maxDepth);
		if(moveX != -1 && moveY != -1)
		{
			_board[moveX][moveY] = PlayerType.Computer;
			PrintRequiredText(_board[moveX][moveY], textContainers[moveX].elements[moveY]);
		}
		 
	 }

	 int Minimax(int depth, bool isMaximizing,int alpha, int beta)
	 {
		 _maxDepth = Mathf.Max(_maxDepth, depth);
		 _functionCalls++;
		 if (Winning(PlayerType.Human))
		 { 
			 return -10;
		 }
		 else if (Winning(PlayerType.Computer))
		 { 
			 return 10;
		 }
		 else if (IsItATie())
		 {
			 return 0;
		 }

		 int bestScore = -1;
		 if (isMaximizing)
		 {
			 bestScore = int.MinValue;
			 for (int i = 0; i < 3; i++)
			 {
				 for (int j = 0; j < 3; j++)
				 {
					 if (_board[i][j] == PlayerType.None)
					 {
						 _board[i][j] = PlayerType.Computer;
						 int score = Minimax(depth + 1, false,alpha,beta);
						 bestScore = Mathf.Max(bestScore, score);
						 _board[i][j] = PlayerType.None;

						 alpha = Mathf.Max(alpha, bestScore);
						 if (beta <= alpha)
						 {
							 break;
						 }
					 }
				 }
			 }
			 return bestScore;
		 }
		 else
		 {
			 bestScore = int.MaxValue;
			 for (int i = 0; i < 3; i++)
			 {
				 for (int j = 0; j < 3; j++)
				 {
					 if (_board[i][j] == PlayerType.None)
					 {
						 _board[i][j] = PlayerType.Human;
						 int score = Minimax(depth + 1, true, alpha, beta);
						 bestScore = Mathf.Min(bestScore, score);
						 _board[i][j] = PlayerType.None;

						 beta = Mathf.Min(beta, bestScore);
						 if (beta <= alpha)
						 {
							 break;
						 }
					 }
				 }
			 }
			 return bestScore;
		 }
		 
	 }

	 private void Awake() {
        _board = new List<List<PlayerType>>();
		_playerInput = GetComponent<PlayerInput>();

        for(int i = 0; i < 3; i++)
        {
            List<PlayerType> row = new List<PlayerType>(){PlayerType.None, PlayerType.None, PlayerType.None};
            _board.Add(row);
        }
    }

	private void Start() {
		_winner = PlayerType.None;
		InputSystem.EnableDevice(Gyroscope.current); 
	}

	private void Update() 
	{
		_currentDeviceOrientation = Vector2.Lerp(_currentDeviceOrientation, _targetDeviceOrientation,rotationSpeed * Time.deltaTime);
		transform.rotation = Quaternion.Euler(_currentDeviceOrientation);
		if(_quitGame)
		{
			return;
		}
		if(_winner != PlayerType.None || CountVacants() == 0)
		{
			switch(_winner)
			{
				case PlayerType.None:  Debug.Log("It's a tie");
									   break;
				case PlayerType.Human: Debug.Log("Player Wins");
									   break;
				
				case PlayerType.Computer: Debug.Log("Computer Wins");
										  break;
			}
			_quitGame = true;
			return;
		}

	}
}
