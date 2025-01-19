using DG.Tweening;
using Map;
using Unit.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controller
{
    /// <summary>
    /// Player의 Input을 처리하는 스크립트
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        public Knight Knight { get; set; }
        
        
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
            
            //Character
            Knight = GetComponent<Knight>();

            if (Camera.main) Camera.main.transform.SetParent(transform);
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
            }
            else if (movement.x < 0)
            {
                _spriteRenderer.flipX = true;
            }
            
            int nextXPos = Knight.PosX + (int)movement.x;
            int nextYPos = Knight.PosY + (int)movement.y;
            Tile nextTile = Knight.CurRoom.GetTile(nextXPos, nextYPos);

            if (nextTile)
            {
                Knight.PosX = nextTile.X;
                Knight.PosY = nextTile.Y;
                Knight.CurRoom = nextTile.Room;
                transform.DOMove(nextTile.transform.position, 0.2f).SetEase(Ease.InOutCubic);
                
            }
        }
    }
}
