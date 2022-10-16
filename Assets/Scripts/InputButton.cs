using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputButton : MonoBehaviour
{
    public static float verticalInput;

    public enum State
    {
        None,
        Down,
        Up
    }

    private State state = State.None;

    private void Update()
    {
        if (state == State.None)
        {
            verticalInput = 0f;
        }
        else if (state == State.Up)
        {
            verticalInput = 1f;
        }
        else if (state == State.Down)
        {
            verticalInput = -1f;
        }
    }

    public void OnMoveUpButtonPressed()
    {
        state = State.Up;
    }

    public void OnMoveUpButtonUp()
    {
        if(state == State.Up)
        {
            state = State.None;
        }
    }

    public void OnMoveDownButtonPressed()
    {
        state = State.Down;
    }

    public void OnMoveDownButtonUp()
    {
        if (state == State.Down)
        {
            state = State.None;
        }
    }
}
