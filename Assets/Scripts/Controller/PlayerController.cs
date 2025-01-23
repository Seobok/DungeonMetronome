using System;
using DG.Tweening;
using Map;
using Unit.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using Workflows;

namespace Controller
{
    /// <summary>
    /// Player의 Input을 처리하는 스크립트
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        public Knight Knight { get; set; }
        public DungeonSceneWorkflow Workflow { get; set; }
        
        
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

            if (Camera.main) Camera.main.transform.SetParent(transform);
        }

        private void OnDestroy()
        {
            _inputActions.Disable();
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
            
            int nextXPos = Knight.Position.X + (int)movement.x;
            int nextYPos = Knight.Position.Y + (int)movement.y;
            Knight.Manager.Dungeon.GetTile(nextXPos, nextYPos, out Tile tile);
            
            if ((tile.Status & StatusFlag.Empty) > 0)
            {
                Knight.Position = tile.Coord;
                transform.DOMove(new Vector2(tile.Coord.X, tile.Coord.Y), 0.2f).SetEase(Ease.InOutCubic);
            }
            
            Workflow.NextTurn();
        }
    }
}
