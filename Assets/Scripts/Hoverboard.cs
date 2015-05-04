using UnityEngine;
using System.Collections;

public class Hoverboard : MonoBehaviour 
{
	Transform _transform = null;
	Rigidbody _rigidbody = null;

	Transform _camTransform = null;

	[SerializeField] float _hoverHeight = 1f;
	[SerializeField] float _castOffsetDist = 0.5f;

	bool _canFlip = false;
	[SerializeField] float _flipForce = 40f;

	[SerializeField] LayerMask _groundLayer = 0;
	[SerializeField] float _groundCheckTime = 0.2f;

	[SerializeField] float _moveSpeed = 25f;
	[SerializeField] float _turnSpeed = 25f;
	[SerializeField] float _rollSpeed = 5;

	[SerializeField] float _pitchSpeed = 15f;
	[SerializeField] float _minPitchTime = 0.7f;

	[SerializeField] float _maxSpeed = 10f;

	float _airTimer = 0f;
	[SerializeField] float _minLandingTime = 0.6f;

	[SerializeField] ParticleSystem _smokeEmitter = null;
	[SerializeField] float _smokeEmissionMod = 2f;

	[SerializeField] ParticleSystem _sparkEmitter = null;
	[SerializeField] int _sparkEmitNum = 35;
	[SerializeField] float _minSparkTime = 0.7f;

	[SerializeField] AudioSource _musicSource = null;
	AudioSource _music = null;

	void Awake () 
	{
		_music = SoundManager.Play2DSound( _musicSource );

		_transform = GetComponent<Transform>();
		_rigidbody = GetComponent<Rigidbody>();

		_camTransform = Camera.main.GetComponent<Transform>();
	}

	void Update()
	{
		if( _canFlip )
		{
			if( Input.GetKeyDown( KeyCode.Q ) )
			{
				_rigidbody.AddForceAtPosition( Vector3.up * _flipForce, _transform.position + _camTransform.right/2f );
			}

			if( Input.GetKeyDown( KeyCode.E ) )
			{
				_rigidbody.AddForceAtPosition( Vector3.up * _flipForce, _transform.position - _camTransform.right/2f );
			}
		}
	}

	void FixedUpdate () 
	{
		MotorControl();

		_smokeEmitter.emissionRate = _rigidbody.velocity.magnitude * _smokeEmissionMod;
		if( Input.GetAxis( "Vertical" ) > WadeUtils.SMALLNUMBER )
		{
			_smokeEmitter.Emit( 1 );
		}
	}

	void MotorControl()
	{
		float rollInput = Input.GetKey( KeyCode.Q ) ? 1f : 0f;
		rollInput += Input.GetKey( KeyCode.E ) ? -1f : 0f;

		RaycastHit hitInfoDown;
		if( Physics.Raycast( new Ray( _transform.position, -_transform.up ), out hitInfoDown, _hoverHeight, _groundLayer ) )
		{
			if( _airTimer > _minSparkTime )
			{
				_sparkEmitter.transform.LookAt( hitInfoDown.normal );
				//_sparkEmitter.Emit( _sparkEmitNum );
			}

			if( _airTimer > _minLandingTime )
			{
				float landDot = Vector3.Dot( hitInfoDown.normal, _transform.up );
				if( landDot > 0.95f )
				{
					ScoreManager.instance.AddScore( 1000 );
				}
				else if( landDot > 0.9f )
				{
					ScoreManager.instance.AddScore( 750 );
				}
			}

			_airTimer = 0f;
		}
		else
		{
			if( _airTimer > _minPitchTime )
			{
				_rigidbody.AddTorque( _transform.right * Input.GetAxis( "Vertical" ) * _pitchSpeed );
			}

			_airTimer += Time.deltaTime;
		}

		if( Physics.Raycast( new Ray( _transform.position, _transform.right ), 2f, _groundLayer ) ||
		    Physics.Raycast( new Ray( _transform.position, -_transform.right ), 2f, _groundLayer ) ||
		    Physics.Raycast( new Ray( _transform.position, _transform.up ), 1f, _groundLayer ))
		{
			ScoreManager.instance.KillCombo();
			_airTimer = 0f;
			_canFlip = true;
		}
		else
		{
			_canFlip = false;
		}

		if( _airTimer < _groundCheckTime )
		{
			_rigidbody.AddForce( _transform.forward * Input.GetAxis( "Vertical" ) * _moveSpeed );
		}

		_rigidbody.AddTorque( _transform.up * Input.GetAxis( "Horizontal" ) * _turnSpeed );
		_rigidbody.AddTorque( _transform.forward * rollInput * _rollSpeed );
		
		if( _rigidbody.velocity.magnitude > _maxSpeed )
		{
			float yVelocity = _rigidbody.velocity.y;
			
			Vector3 curVelocity = _rigidbody.velocity;
			curVelocity.y = 0f;
			
			curVelocity = curVelocity.normalized * _maxSpeed;
			curVelocity.y = yVelocity;
			
			_rigidbody.velocity = curVelocity;
		}
	}

	void OnDrawGizmos()
	{
		if( !_transform )
		{
			_transform = GetComponent<Transform>();
		}

		Gizmos.color = Color.white;
		Gizmos.DrawLine( _transform.position, _transform.position - _transform.up * _hoverHeight );

		Vector3 frontCastPos = _transform.position + _transform.forward * _castOffsetDist;
		Gizmos.DrawLine( frontCastPos, frontCastPos - _transform.up * _hoverHeight );

		Vector3 backCastPos = _transform.position - _transform.forward * _castOffsetDist;
		Gizmos.DrawLine( backCastPos, backCastPos - _transform.up * _hoverHeight );
	}
}
