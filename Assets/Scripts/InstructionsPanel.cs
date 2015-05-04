using UnityEngine;
using System.Collections;

public class InstructionsPanel : MonoBehaviour 
{
	void Update () 
	{
		if( Input.anyKeyDown )
		{
			StartCoroutine( DisableInstructionPanel() );
		}
	}

	IEnumerator DisableInstructionPanel()
	{
		yield return new WaitForSeconds( 3f );
		gameObject.SetActive( false );
	}
}
