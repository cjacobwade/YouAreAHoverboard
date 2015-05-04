using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogueText : MonoBehaviour 
{
	Animator _animator = null;
	Text _text = null;
	[SerializeField] float _waitTime = 3f;
	
	bool _isOut = false;
	
	void Awake()
	{
		_animator = GetComponent<Animator>();
		_text = GetComponentInChildren<Text>();
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

	public void SetText( string text )
	{
		_text.text = text;
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
