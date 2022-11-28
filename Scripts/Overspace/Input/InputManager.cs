using Overspace.Pattern.Singleton;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.InputSystem.Utilities;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Overspace.Input
{
    public class InputManager : MonoBehaviourSingleton<InputManager>
    {
        private void OnEnable()
        {
            EnhancedTouchSupport.Enable();
        }

        private void OnDisable()
        {
            EnhancedTouchSupport.Disable();
        }

        public Vector2 ReadInputAsJoystick(float joystickThrow)
        {
            ReadOnlyArray<Touch> activeTouches = Touch.activeTouches;
            if (activeTouches.Count > 0 && EventSystem.current.currentSelectedGameObject == null) return Vector2.ClampMagnitude(activeTouches[0].screenPosition - activeTouches[0].startScreenPosition, joystickThrow) / joystickThrow;
            return Vector2.zero;
        }

        public Vector2 ReadInputAsScreenPosition()
        {
            ReadOnlyArray<Touch> activeTouches = Touch.activeTouches;
            if (EventSystem.current.currentSelectedGameObject != null) return Vector2.zero;
            return activeTouches.Count > 0 ? activeTouches[0].screenPosition : Vector2.zero;
        }
    }
}