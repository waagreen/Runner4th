using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IncredibleCode{
    public interface IDataMovement
    {
        void Jump(InputAction.CallbackContext context);
    }

    public interface ISlide
    {
        void Sliding(InputAction.CallbackContext context);
    }

}
