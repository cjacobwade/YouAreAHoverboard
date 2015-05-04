using UnityEngine;
using System.Collections;

public class RadAnimalPopup : MonoBehaviour 
{
	Animator _animator = null;
	[SerializeField] float _waitTime = 3f;

	bool _isOut = false;

	void Awake()
	{
		_animator = GetComponent<Animator>();
	}

	public void PlayPopIn()
	{
		StartCoroutine( PlayPopInRoutine() );
	}

	public void ForceSlideOut()
	{
		StopCoroutine( PlayPopInRoutine() );

		_animator.Play( "SlideOut" );
	}

	public bool IsOut()
	{
		return _isOut;
	}

	IEnumerator PlayPopInRoutine()
	{
		_animator.Play( "SlideIn" );
		_isOut = true;

		yield return new WaitForSeconds( _waitTime );

		_animator.Play( "SlideOut" );
		_isOut = false;
	}
}
