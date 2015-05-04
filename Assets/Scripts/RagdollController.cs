using UnityEngine;
using System.Collections;

public class RagdollController : MonoBehaviour 
{
	[SerializeField] Rigidbody[] _ragdollPieces = null;

	public void Activate()
	{
		foreach( Rigidbody _ragdollPiece in _ragdollPieces )
		{
			_ragdollPiece.isKinematic = false;
		}
	}
}
