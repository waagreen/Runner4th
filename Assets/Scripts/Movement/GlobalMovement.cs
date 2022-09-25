using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalMovement : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    [SerializeField] private GameObject _manager;

    [Header("Speed parameters")]
    [SerializeField][Range(0.01f, 1f)] private float accelerationRate;
    [SerializeField][Range(10f, 300f)] protected float maxRunSpeed;
    [SerializeField] protected Renderer sphereFeedback;
    [SerializeField] protected Transform PlayerTransform;

    [Header("State Colors")]
    public Gradient baseStateGradient;
    public Gradient highStateGradient;
    public Gradient maxStateGradient;
    
    public const float kMinSpeed = 2f;
    public float distance = 0;
    private static bool wasAlreadySpawned = false;
    
    public VelocityState CurrentState => GetSpeedState();
    protected float runAcceleration = 1f;
    private float _actualSpeed = kMinSpeed;
    public float ActualSpeed => _actualSpeed;
    
    private void Start() {
        
        if (!wasAlreadySpawned)
        {
            DontDestroyOnLoad(gameManager);
            DontDestroyOnLoad(this.gameObject);
            wasAlreadySpawned = true;
        }
        else
        {
            DestroyImmediate(gameManager);
            DestroyImmediate(this.gameObject);
		}

        

    }

    protected void FixedUpdate()
    {
		if (PlayerTransform == null) PlayerTransform = GameObject.Find("Main_Character").GetComponent<Transform>();
        
		//move forward
		if (OnSlope(PlayerTransform))
        {
			runAcceleration = Mathf.Sqrt((accelerationRate * 3f) * Time.fixedDeltaTime);
		}
        else runAcceleration = Mathf.Sqrt(accelerationRate * Time.fixedDeltaTime); 
        
        _actualSpeed += runAcceleration / 1.5f;
        distance += _actualSpeed * Time.deltaTime;

		if (sphereFeedback == null) sphereFeedback = GameObject.Find("Speed_Feedback").GetComponent<MeshRenderer>();

		if (CurrentState == VelocityState.Maximun)
        { 
            _actualSpeed = maxRunSpeed;
            sphereFeedback.material.color = maxStateGradient.colorKeys[1].color;
        }
        else if (CurrentState == VelocityState.High) sphereFeedback.material.color = highStateGradient.colorKeys[1].color;
        else  if (CurrentState == VelocityState.Base) sphereFeedback.material.color = baseStateGradient.colorKeys[1].color;
    }

    
    
    public VelocityState GetSpeedState()
    {
        //Switch btween the states of velocity
        VelocityState inState;

        if(ActualSpeed >= maxRunSpeed) inState = VelocityState.Maximun;
        else if(ActualSpeed >= maxRunSpeed / 2f) inState = VelocityState.High;
        else if(ActualSpeed < kMinSpeed) inState = VelocityState.Idle;
        else inState = VelocityState.Base;

        return inState;
    }

    public void ReduceSpeed() => _actualSpeed = _actualSpeed / 2f;
    public void ReloadGame()
    {   
        // restartScreen.SetActive(false);
        var currentScene = SceneManager.GetActiveScene();   
        SceneManager.LoadScene(currentScene.buildIndex);
	}
    
    private bool OnSlope(Transform t)
    {
        RaycastHit slopeHit;

        if(Physics.Raycast(t.position, Vector3.down, out slopeHit, transform.localScale.x / 2 + 0.5f))
        {
            return slopeHit.normal != Vector3.up ? true : false;
        }
        else return false;
    }

}
