using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class AnimalInfo
{
	public RadAnimalPopup radAnimalPopup;
	public string[] goodDialogue;
	public string[] badDialogue;
}

public class ScoreManager : SingletonBehaviour<ScoreManager> 
{
	[SerializeField] Text _scoreText = null;
	[SerializeField] Text _comboText = null;

	[SerializeField] AnimalInfo[] _animalInfos = null;
	RadAnimalPopup _currentPopUp = null;
	[SerializeField] int _minPopupScore = 1000;

	[SerializeField] DialogueText _dialogueText = null;

	[SerializeField] AudioSource _cashAudio = null;

	int _currentScore = 0;
	int _currentCombo = 1;

	void UpdateScore()
	{
		_scoreText.text = _currentScore.ToString();
	}

	void UpdateCombo()
	{
		if( _currentCombo == 1 )
		{
			_comboText.text = "Nah";
		}
		else
		{
			_comboText.text = _currentCombo.ToString() + "X";
		}
	}

	public void AddScore( int addAmount )
	{
		_currentScore += addAmount * _currentCombo;
		UpdateScore();

		SoundManager.Play2DSound( _cashAudio );

		if( addAmount >= _minPopupScore )
		{
			if( _currentPopUp && _currentPopUp.IsOut() )
			{
				_currentPopUp.ForceSlideOut();
				_dialogueText.ForceSlideOut();
				_currentPopUp = null;
			}

			AddCombo( 1 );

			int animalIndex = Random.Range( 0, _animalInfos.Length );
			_currentPopUp = _animalInfos[animalIndex].radAnimalPopup;
			_currentPopUp.PlayPopIn();

			int textArrLength = _animalInfos[animalIndex].goodDialogue.Length;
			_dialogueText.SetText( _animalInfos[animalIndex].goodDialogue[Random.Range( 0, textArrLength )] );
			_dialogueText.PlayPopIn();
		}
	}

	public void AddCombo( int addAmount )
	{
		_currentCombo += addAmount;
		UpdateCombo();
	}

	public void KillCombo()
	{
		_currentCombo = 1;
		UpdateCombo();
	}
}
