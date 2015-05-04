using UnityEngine;
using System.Collections;

public class GiraffePhysics : MonoBehaviour 
{
	[SerializeField] Rigidbody _head = null;
	[SerializeField] Transform _headTransform = null;
	//Vector3 _headRotationOffset = Vector3.zero;
	//[SerializeField] float _normalizedHeadDist = 75f;
	[SerializeField] float _headFollowForce = 50f;
	[SerializeField] float _headUpForce = 50f;
	Vector3 _lookDirOffset = new Vector3( -19.5f, -170f, -145f );

	[SerializeField] Rigidbody _belly = null;
	[SerializeField] Transform _bellyTransform = null;
	[SerializeField] float _bellyUpForce = 50f;
	[SerializeField] float _jumpHoldForce = 200f;
	[SerializeField] float _jumpReleaseForce = 200f;

	[SerializeField] Rigidbody[] _knees = null;
	Transform[] _kneeTransforms;
	[SerializeField] float _kneeUpForce = 5f;
	[SerializeField] float _kneeGroundCheckHeight = 2f;

	[SerializeField] MinMaxF _legJumpTimeRange = new MinMaxF( 2f, 5f );

	[SerializeField] Rigidbody[] _feet = null;
	[SerializeField] float _feetDownForce = 5f;
	[SerializeField] float _randomFeetForce = 35f;

	[SerializeField] LayerMask _groundLayer = 0;

	Vector3 mousePos = Vector3.zero;

	void Awake()
	{
		_kneeTransforms = new Transform[_knees.Length];
		for( int i = 0; i < _knees.Length; i++ )
		{
			_kneeTransforms[i] = _knees[i].GetComponent<Transform>();
		}

		//_headRotationOffset = _bellyTransform.eulerAngles - _headTransform.eulerAngles;
		StartCoroutine( RandomLegJump() );
	}

	void Update()
	{
		if( Input.GetButton( "Jump" ) )
		{
			_belly.AddForce( Vector3.down * _jumpHoldForce );
		}

		if( Input.GetButtonUp( "Jump" ) )
		{
			_belly.AddForce( -_bellyTransform.up * _jumpReleaseForce );

			foreach( Rigidbody knee in _knees )
			{
				knee.AddForce( -_bellyTransform.up * _jumpReleaseForce );
			}
		}

		if( Input.GetKey( KeyCode.S ) )
		{
			_knees[0].AddForce( _kneeTransforms[0].up + _bellyTransform.forward  * _kneeUpForce );
		}

		if( Input.GetKey( KeyCode.A ) )
		{
			_knees[1].AddForce( _kneeTransforms[1].up + _bellyTransform.forward * _kneeUpForce );
		}

		if( Input.GetKey( KeyCode.Q ) )
		{
			_knees[2].AddForce( _kneeTransforms[2].up + _bellyTransform.forward * _kneeUpForce );
		}

		if( Input.GetKey( KeyCode.W ) )
		{
			_knees[3].AddForce( _kneeTransforms[3].up + _bellyTransform.forward * _kneeUpForce );
		}
	}

	void FixedUpdate()
	{
		HeadChores();
		BellyChores();
		LegChores();
	}

	void HeadChores()
	{
		_head.AddForce( Vector3.up * _headUpForce );
		
//		Vector3 headRot = _bellyTrasform.eulerAngles - _headRotationOffset;
//		_headTransform.rotation = Quaternion.Euler( headRot );

		mousePos = Camera.main.ScreenToViewportPoint( Input.mousePosition );
		mousePos.x -= 0.5f;
		mousePos.y -= 0.5f;
		mousePos.z = 0f;

		_head.velocity = mousePos * _headFollowForce;
		Debug.Log( mousePos );


		Vector3 lookDir = _headTransform.position + mousePos * 100f;
		lookDir = Quaternion.Euler( _lookDirOffset ) * lookDir;
	}

	void BellyChores()
	{
		_belly.AddForce( Vector3.up * _bellyUpForce );
	}

	void LegChores()
	{
		for( int i = 0; i < _knees.Length; i++ )
		{
			if( Physics.Raycast( new Ray( _kneeTransforms[i].position, Vector3.down ), _kneeGroundCheckHeight, (int)_groundLayer ) )
			{
				_knees[i].AddForce( _kneeUpForce * Vector3.up );
			}
		}

		for( int i = 0; i < _knees.Length; i++ )
		{
			_feet[i].AddForce( _feetDownForce * Vector3.down );
		}
	}

	IEnumerator RandomLegJump()
	{
		while( true )
		{
			float legJumpTime = _legJumpTimeRange.Random();

			yield return new WaitForSeconds( legJumpTime );

			int randomLegIndex = Random.Range( 0, _knees.Length );
			_feet[randomLegIndex].AddForce( Vector3.up * _randomFeetForce );
		}
	}
}
