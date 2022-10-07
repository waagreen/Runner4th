using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.Events;

public enum VelocityState : int
{ 
    Idle = -1,
    Base = 0,
    High = 1, 
    Maximun = 2, 
    Immovable = 3,
}

public class GlobalMovement : MonoBehaviour, ISaveble
{
    [SerializeField] private PlayerGameData gameData;

    [Header("Speed parameters")]
    [SerializeField][Range(0.01f, 1f)] private float accelerationRate;
    [SerializeField][Range(10f, 50f)] protected float maxRunSpeed;
    [SerializeField] protected Transform PlayerTransform;

    [Header("State Colors")]
    public Gradient[] velocityGradients;

    public PlayerGameData GameData => gameData;
    public VelocityState CurrentState => GetSpeedState();
    
    public const float kMinSpeed = 2f;
    public float distance = 0;
    public float CurrentSpeed => _currentlSpeed;
    
    private UnityEvent deathEvent => DataManager.Loader.OnPlayerDeath;
    private float runAcceleration = 1f;
    private float _currentlSpeed = kMinSpeed;

    private void Awake()
    {
        LoadJsonData(this);
        deathEvent.AddListener(SetSpeedToZero);
    }

    protected void FixedUpdate()
    {
        if(CurrentState == VelocityState.Idle) deathEvent.Invoke();
        else
        {
            //move forward
            _currentlSpeed += runAcceleration / 1.5f;
            distance += _currentlSpeed * Time.deltaTime;
        }

        runAcceleration = OnSlope(PlayerTransform) ? Mathf.Sqrt((accelerationRate * 6f) * Time.fixedDeltaTime) : runAcceleration = Mathf.Sqrt(accelerationRate * Time.fixedDeltaTime);

		if (CurrentState == VelocityState.Maximun) _currentlSpeed = maxRunSpeed;
    }
    
    public VelocityState GetSpeedState()
    {
        //Switch btween the states of velocity
        VelocityState inState;

        if(CurrentSpeed >= maxRunSpeed) inState = VelocityState.Maximun;
        else if(CurrentSpeed >= maxRunSpeed / 2f) inState = VelocityState.High;
        else if(CurrentSpeed < kMinSpeed) inState = VelocityState.Idle;
        else inState = VelocityState.Base;

        return inState;
    }

    private void SetSpeedToZero() => _currentlSpeed = 0f;
    public void ReduceSpeed() => _currentlSpeed /= 2f;
    
    private bool OnSlope(Transform t)
    {
        if(t == null) return false;
        
        RaycastHit slopeHit;

        if(Physics.Raycast(t.position, Vector3.down, out slopeHit, transform.localScale.x / 2 + 0.5f))
        {
            return slopeHit.normal != Vector3.up ? true : false;
        }
        else return false;
    }

    private static void SaveJsonData(GlobalMovement gMove)
    {
        SaveData sd = new SaveData();
        gMove.PopulateSaveData(sd);

        if(FileManager.WriteToFile("SaveData.dat", sd.ToJson()))
        {
            Debug.Log("Save Succesful");
        }
    }

    private static void LoadJsonData(GlobalMovement gMove)
    {
        if(FileManager.LoadFromFile("SaveData.dat", out var json))
        {
            SaveData sd = new SaveData();
            sd.LoadFromJson(json);

            gMove.LoadFromSaveData(sd);
            Debug.Log("Load Succesful");
        }
    }

    public void PopulateSaveData(SaveData a_SaveData)
    {
        if(distance > gameData.currentBestDistance) gameData.currentBestDistance = Mathf.RoundToInt(distance);

        a_SaveData.myCoins = gameData.totalCoins;
        a_SaveData.bestDistance = gameData.currentBestDistance;
    }
    
    public void LoadFromSaveData(SaveData a_SaveData)
    {
        gameData.totalCoins = a_SaveData.myCoins;
        gameData.currentBestDistance = a_SaveData.bestDistance;
    }

    private void OnDestroy()
    {
        SaveJsonData(this);
        deathEvent.RemoveListener(SetSpeedToZero);
    }
}
