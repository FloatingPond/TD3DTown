using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

namespace Input
{
    public interface IInputReader
    {
        Vector2 MoveDirection { get; }
        void EnablePlayerActions();
    }
    
    [CreateAssetMenu(fileName = "InputReader", menuName = "Input/InputReader")]
    public class InputReader : ScriptableObject, IInputReader, IPlayerActions
    {
        public UnityAction<Vector2> Move = delegate {  };
        public UnityAction<float> Rotate = delegate {  };
        public UnityAction<Vector2> Look = delegate {  };
        public UnityAction<float> Zoom = delegate {  };
        
        private InputSystem_Actions m_inputActions;
        
        public Vector2 MoveDirection => m_inputActions.Player.Move.ReadValue<Vector2>();
        public Vector2 LookDirection => m_inputActions.Player.Look.ReadValue<Vector2>();
        public float CurrentZoom => m_inputActions.Player.Zoom.ReadValue<float>();
        
        public bool IsMoveInputPressed => m_inputActions.Player.Move.IsPressed();
        public void EnablePlayerActions()
        {
            if (m_inputActions == null)
            {
                m_inputActions = new InputSystem_Actions();
                m_inputActions.Player.SetCallbacks(this);
            }
            m_inputActions.Enable();
        }
        
        public void DisablePlayerActions()
        {
            m_inputActions.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Move?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnRotate(InputAction.CallbackContext context)
        {
            if (context.ReadValue<float>() == 0)
            {
                return;
            }

            Rotate?.Invoke(context.ReadValue<float>());
        }

        public void OnZoom(InputAction.CallbackContext context)
        {
            Zoom.Invoke(context.ReadValue<float>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Look?.Invoke(context.ReadValue<Vector2>());
        }
    }
}