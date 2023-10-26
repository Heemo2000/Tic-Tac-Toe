using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerType
{
    None = -1,
    Human = 0,
    Computer = 1
}

enum Side
{
    Circle = 0,
    Cross = 1
}
public class TicTacToe : MonoBehaviour
{
    [SerializeField]private List<TextContainerRow> textContainers = new List<TextContainerRow>();
	[SerializeField]private Side humanSide = Side.Circle;
	[SerializeField]private Camera lookCamera;

	[SerializeField]private float detectDistance = 50f;
	[SerializeField]private LayerMask textContainerMask;

    private List<List<PlayerType>> _board;
	private int _functionCalls = 0;
    
	private PlayerType _winner = PlayerType.None;

	private bool _quitGame = false;

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

    bool Winning(PlayerType player)
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

     bool IsItATie()
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

     private void UpdateBoardGraphics()
	 {
		 //std::cout << "Human : O" << std::endl;
		 //std::cout << "Computer : X" << std::endl;



		 for (int i = 0; i < 3; i++)
		 {
			 for (int j = 0; j < 3; j++)
			 {
				 PlayerType current = _board[i][j];
				 switch(current)
                 {
					case PlayerType.None:
											textContainers[i].elements[j].SetText("");
											break;
                    case PlayerType.Human:
                                            if(humanSide == Side.Circle)
                                            {
                                                textContainers[i].elements[j].SetText("O");
                                            }
                                            else
                                            {
                                                textContainers[i].elements[j].SetText("X");
                                            }
                                            break;
                    case PlayerType.Computer:
                                            if(humanSide == Side.Circle)
                                            {
                                                textContainers[i].elements[j].SetText("X");
                                            }
                                            else
                                            {
                                                textContainers[i].elements[j].SetText("O");
                                            }
                                            break;
					
                 }
			 }
		 }
	 }

	 void BestMove()
	 {
		 _functionCalls = 0;
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
		if(moveX != -1 && moveY != -1)
		{
			_board[moveX][moveY] = PlayerType.Computer;
		}
		 
	 }

	 int Minimax(int depth, bool isMaximizing,int alpha, int beta)
	 {
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

        for(int i = 0; i < 3; i++)
        {
            List<PlayerType> row = new List<PlayerType>(){PlayerType.None, PlayerType.None, PlayerType.None};
            _board.Add(row);
        }
    }

	private void Start() {
		_winner = PlayerType.None;
	}
	private void Update() {
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
		if(Input.GetMouseButtonDown(0))
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

				Ray ray = lookCamera.ScreenPointToRay(Input.mousePosition);

				if(Physics.Raycast(ray,out RaycastHit hit, detectDistance, textContainerMask.value))
				{
					TextContainer textContainer = hit.transform.GetComponent<TextContainer>();

					for(int i = 0; i < textContainers.Count; i++)
					{
						var row = textContainers[i];
						int j = row.elements.FindIndex(0,row.elements.Count,(TextContainer container)=> textContainer.Equals(container));
						if(j != -1)
						{
							_board[i][j] = PlayerType.Human;
							
							BestMove();
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
				UpdateBoardGraphics();
					
			}
	}
}
