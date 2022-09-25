using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class GlobalDataSO : ScriptableObject
{
	[Header("Speed parameters")]
	[SerializeField][Range(0.01f, 1f)] private float _accelerationRate;
	[SerializeField][Range(10f, 300f)] private float _maxRunSpeed;
	[SerializeField] private float _distance = 0;
	[SerializeField] private float _runAcceleration = 1f;
	[SerializeField] private float _actualSpeed = kMinSpeed;

	[Header("Spawn position")]
	[SerializeField] private Vector3 _spawnPos;
	[SerializeField] private GameObject _playerObject;

	[Header("State Colors")]
	[SerializeField] private Gradient _baseStateGradient;
	[SerializeField] private Gradient _highStateGradient;
	[SerializeField] private Gradient _maxStateGradient;
	[SerializeField] private Renderer SphereFeedback;

	[Header("UI")]
	[SerializeField] private Canvas _restartScreen;

	public static float kMinSpeed = 2f;
	public VelocityState CurrentState => GetSpeedState();


	#region Properties
	public float AccelerationRate { get => _accelerationRate; set => _accelerationRate = value; }
	public float MaxRunSpeed { get => _maxRunSpeed; }
	public float Distance { get => _distance; }
	public float RunAcceleration { get => _runAcceleration; set => _runAcceleration = value; }
	public float ActualSpeed { get => _actualSpeed; set => _actualSpeed = value; }
	public Gradient BaseStateGradient { get => _baseStateGradient; set => _baseStateGradient = value; }
	public Gradient HighStateGradient { get => _highStateGradient; set => _highStateGradient = value; }
	public Gradient MaxStateGradient { get => _maxStateGradient; set => _maxStateGradient = value; }
	public Canvas RestartScreen { get => _restartScreen; }
	#endregion


	public bool OnSlope(GameObject g)
	{
		RaycastHit slopeHit;

		if (Physics.Raycast(g.gameObject.transform.position, Vector3.down, out slopeHit, g.gameObject.transform.localScale.y / 2 + 0.5f))
		{
			return slopeHit.normal != Vector3.up ? true : false;
		}
		else return false;
	}

	public void UpdateSpeed(bool condition)
	{
		Debug.Log(condition);
		//move forward
		if (condition)
		{
			_runAcceleration = Mathf.Sqrt((_accelerationRate * 3f) * Time.fixedDeltaTime);
		}
		else _runAcceleration = Mathf.Sqrt(_accelerationRate * Time.fixedDeltaTime);

		_actualSpeed += _runAcceleration / 1.5f;

		_distance += _actualSpeed * Time.deltaTime;
	}

	public VelocityState GetSpeedState()
	{
		//Switch btween the states of velocity
		VelocityState inState;

		if (ActualSpeed >= _maxRunSpeed) inState = VelocityState.Maximun;
		else if (ActualSpeed >= _maxRunSpeed / 2f) inState = VelocityState.High;
		else if (ActualSpeed < kMinSpeed) inState = VelocityState.Idle;
		else inState = VelocityState.Base;

		return inState;
	}

	public void ResetGame()
	{
		_distance = 0;
		_runAcceleration = 1;
		_actualSpeed = kMinSpeed;
		_playerObject.transform.position = _spawnPos;	
		Camera.main.fieldOfView = 40f;
		
	}

	public void ReduceSpeed()
	{
		_actualSpeed = _actualSpeed / 2f;
	}

	public void ReloadScene()
	{
		var currentScene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(currentScene.buildIndex);
	}
}
