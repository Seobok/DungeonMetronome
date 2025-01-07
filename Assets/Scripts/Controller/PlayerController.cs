using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    /// <summary>
    /// Player의 Input을 처리하는 스크립트
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        private InputActions _inputActions;
        private SpriteRenderer _spriteRenderer;

        
        private void Awake()
        {
            //Player Input 활성화 및 바인딩
            _inputActions = new InputActions();
            _inputActions.Enable();
            _inputActions.Player.Move.performed += MoveOnPerformed;
            
            //Player Sprite
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        
        /// <summary>
        /// Player Move Input을 처리하는 스크립트
        /// </summary>
        /// <param name="context"> PlayerInput을 Vector2의 형태로 전달 </param>
        private void MoveOnPerformed(InputAction.CallbackContext context)
        {
            Vector2 movement = context.ReadValue<Vector2>();
            
            //입력받은 방향으로 정면 전환
            if (movement.x > 0)
            {
                _spriteRenderer.flipX = false;
                movement.y = 0;
            }
            else if (movement.x < 0)
            {
                _spriteRenderer.flipX = true;
                movement.y = 0;
            }
        }
    }
}