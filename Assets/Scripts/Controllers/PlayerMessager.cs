using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMessager : MonoBehaviour
{
    public void OnMove(InputValue value) {
        gameObject.SendMessage("PlayerMoved", value.Get<Vector2>());
    }
}
