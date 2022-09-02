using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityStates
{
    public enum State { Base, High, Maximun };

    public float SetSpeed(State state, float speed)
    {
        //Switch btween the states of velocity
        switch (state)
        {
            case State.Base:
                speed = 100f;
                break;
            case State.High:
                speed = 200f;
                break;
            case State.Maximun:
                speed = 300f;
                break;
            default:
                break;
        }
        return speed;
    }

}
