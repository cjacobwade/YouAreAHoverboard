using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeController : MonoBehaviour 
{
	Text _timerText = null;
	[SerializeField] float _gameTime = 180f;
	float _gameTimer = 0f;

	bool _gameOver = false;

	[SerializeField] GameObject[] _endDisableObjects = null;
	[SerializeField] Hoverboard _hoverBoard = null;

	[SerializeField] GameObject _endTextObj = null;

	void Awake()
	{
		_timerText = GetComponent<Text>();
	}

	void Update () 
	{
		_gameTimer += Time.deltaTime;

		if( _gameTimer > _gameTime )
		{
			if( !_gameOver )
			{
				EndGame();
				_gameOver = true;
			}
			else if( Input.anyKeyDown )
			{
				StartCoroutine( WaitAndQuit() );
			}
		}
		else
		{
			_timerText.text = "Time\n" + Mathf.RoundToInt( _gameTime - _gameTimer );
		}
	}

	void EndGame()
	{
		_timerText.text = "";

		foreach( GameObject _endDisableObj in _endDisableObjects )
		{
			_endDisableObj.SetActive( false );
		}

		_hoverBoard.enabled = false;
		_endTextObj.SetActive( true );
	}

	IEnumerator WaitAndQuit()
	{
		yield return new WaitForSeconds( 3f );

		Application.Quit();
	}
}
