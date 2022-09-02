using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityStates
{
    public enum State { Base, High, Maximun };

    public State Speed;

    public float SetSpeed(State state, float speed)
    {
        //Switch btween the states of velocity
        switch (state)
        {
            case State.Base:
                speed *= 1f;
                break;
            case State.High:
                speed *= 1.5f;
                break;
            case State.Maximun:
                speed *= 2.5f;
                break;
            default:
                break;
        }
        return speed;
    }

}
