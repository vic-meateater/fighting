using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.OnScreen;

/// <summary>
/// Debug class
/// </summary>
namespace Fighting
{
    public class CheckOnScreenControl : MonoBehaviour
    {
        public InputAction moveAction;

        private void Start()
        {
            // Инициализируем и включаем действие
            if (moveAction != null)
            {
                moveAction.Enable();
            }
            else
            {
                Debug.LogError("Move action is not assigned!");
            }

            // Проверка OnScreenControl
            var onScreenControls = FindObjectsOfType<OnScreenControl>();
            foreach (var osc in onScreenControls)
            {
                if (osc.control != null)
                {
                    Debug.Log($"OnScreenControl {osc.name} bound to {osc.control.path}");
                }
                else
                {
                    Debug.LogWarning($"OnScreenControl {osc.name} has no active control path!");
                }
            }
        }

        private void OnEnable()
        {
            // Подписка на событие изменения управления
            InputSystem.onActionChange += OnActionChange;
        }

        private void OnDisable()
        {
            // Отписка от события изменения управления
            InputSystem.onActionChange -= OnActionChange;
        }

        private void OnActionChange(object obj, InputActionChange change)
        {
            if (change == InputActionChange.BoundControlsChanged)
            {
                var action = obj as InputAction;
                if (action != null)
                {
                    Debug.Log($"Action {action.name} controls updated!");
                }
            }
        }
    }
}