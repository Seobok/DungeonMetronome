using System;
using DG.Tweening;
using Map;
using Unit.Enemy;
using Unit.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using Workflows;

namespace Controller
{
    /// <summary>
    /// Player의 Input을 처리하는 스크립트
    /// </summary>
    public class PlayerController
    {
        public PlayerController(Knight knight)
        {
            _knight = knight;
            
            _inputActions = new InputActions();
            _inputActions.Enable();
            _inputActions.Player.Move.performed += MoveOnPerformed;
            
            if (Camera.main) Camera.main.transform.SetParent(_knight.Transform);
        }

        ~PlayerController()
        {
            _inputActions.Disable();
        }


        public Action NextTurn;
        private readonly Knight _knight;
        private readonly InputActions _inputActions;

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
                _knight.FlipX = false;
            }
            else if (movement.x < 0)
            {
                _knight.FlipX = true;
            }
            
            int nextXPos = _knight.Position.X + (int)movement.x;
            int nextYPos = _knight.Position.Y + (int)movement.y;
            
            Tile tile = _knight.Dungeon.GetTile(nextXPos, nextYPos);
            
            if ((tile.Status & StatusFlag.Empty) > 0)
            {
                _knight.Position = tile.Coord;
                _knight.Transform.DOMove(new Vector3(tile.Coord.X, tile.Coord.Y, _knight.Transform.position.z), 0.2f)
                    .SetEase(Ease.InOutCubic);
            }
            else if ((tile.Status & StatusFlag.Unit) > 0)
            {
                //해당 유닛이 아이템이면
                
                //해당 유닛이 Enemy이면
                if (tile.Unit is Enemy enemy)
                {
                    enemy.TakeDamage(1);
                }
            }
            
            NextTurn?.Invoke();
        }
    }
}
