using UnityEngine;
using System.Collections;

public class ActivateRagdoll : MonoBehaviour 
{
	[SerializeField] RagdollController ragdollController;

	void OnCollisionEnter( Collision col )
	{
		if( col.gameObject.GetComponent<Hoverboard>() )
		{
			ragdollController.Activate();
		}
	}
}
