using FP.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace FP.Input
{
    public class InputManager : MonoBehaviourSingleton<InputManager>
    {
        #region Private Variables
        [SerializeField] private InputActionAsset _inputs;
        [SerializeField] private InputInitializer _initializer;
        private Dictionary<string, InputActionMap> _actionMaps = new();
        private Dictionary<string, InputAction> _inputActions = new();
        private HashSet<string> _previousEnableActions = new();
        private HashSet<string> _previousEnableActionsMaps = new();
        [SerializeField] private bool _showLogs = true;
        private InputDevice _lastUsedDevice;
        #endregion

        #region Properties
        public InputActionAsset Inputs => _inputs;
        public InputDevice LastUsedDevice => _lastUsedDevice;
        #endregion

        #region Events
        public UnityEvent<InputDevice> OnDeviceChagned = new();
        #endregion

        #region MonoBehaviour Callbacks
        protected override void Awake()
        {
            base.Awake();

            if (_inputs == null)
            {
                Debug.LogError("InputManager: InputActionAsset is not assigned.");
                this.enabled = false;
                return;
            }

            _inputActions ??= new Dictionary<string, InputAction>();
            _actionMaps ??= new Dictionary<string, InputActionMap>();

            foreach (var map in _inputs.actionMaps)
            {
                _actionMaps.Add(map.name, map);
                foreach (var action in map.actions)
                {
                    _inputActions.Add(action.name, action);
                }
            }

            ActiveInputsWithInitializer();
        }

        private void OnEnable()
        {
            ActiveInputsWithInitializer();
            UnsubscribeEvents();
            SubscribeEvents(); ;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
            DisableAllInputs();
        }
        #endregion

        #region Private Methods
        private void SubscribeEvents()
        {
            InputSystem.onEvent += OnInputEvent;
            InputSystem.onDeviceChange += DeviceChanged;
        }

        private void UnsubscribeEvents()
        {
            InputSystem.onEvent -= OnInputEvent;
            InputSystem.onDeviceChange -= DeviceChanged;
        }

        private void DeviceChanged(InputDevice device, InputDeviceChange deviceChange)
        {
            if (deviceChange is not InputDeviceChange.SoftReset) { return; }

            OnDeviceChagned?.Invoke(device);
        }

        #region Input Event Filtering

        private float _lastInputTime;
        private const float SwitchCooldown = 0.15f; // Tiempo mínimo antes de aceptar cambio de dispositivo

        private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
        {
            // Validaciones básicas
            if (device == null || !eventPtr.valid)
                return;

            // Ignorar eventos de tipo no relevante
            if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
                return;

            // Solo continuar si el evento fue causado por input real del usuario
            if (!HasRealUserInput(device))
                return;

            // Normalizar el tipo de dispositivo
            InputDevice normalizedDevice = device switch
            {
                Keyboard => Keyboard.current,
                Mouse => Keyboard.current,
                Gamepad g => g,
                _ => null
            };

            if (normalizedDevice == null)
                return;

            // Evitar spam de cambios de dispositivo
            if (Time.unscaledTime - _lastInputTime < SwitchCooldown && normalizedDevice != _lastUsedDevice)
                return;

            _lastInputTime = Time.unscaledTime;

            if (_lastUsedDevice == normalizedDevice)
                return;

            _lastUsedDevice = normalizedDevice;
            OnDeviceChagned?.Invoke(_lastUsedDevice);

            if (_showLogs)
                Debug.Log($"[InputManager] Active Device: {_lastUsedDevice.displayName}");
        }

        /// <summary>
        /// Detecta si un dispositivo está realmente siendo usado por el jugador
        /// </summary>
        private bool HasRealUserInput(InputDevice device)
        {
            // Recorrer controles relevantes del dispositivo
            foreach (var control in device.allControls)
            {
                if (control.synthetic || control.noisy)
                    continue;

                object value = control.ReadValueAsObject();
                if (value == null)
                    continue;

                // Filtros de actividad reales
                switch (value)
                {
                    case float f when Mathf.Abs(f) > 0.2f:
                        return true;
                    case Vector2 v when v.magnitude > 0.2f:
                        return true;
                    case bool b when b:
                        return true;
                }
            }

            return false;
        }

        #endregion

        private void ReportActionError(string actionName)
        {
            if (!_showLogs) return;
            Debug.LogError($"InputManager: Action {actionName} not found.");
        }

        /// <summary>
        /// Read the Input Initializer and Enable or Disable based on the Value
        /// </summary>
        private void ActiveInputsWithInitializer()
        {
            if (_initializer == null) { return; }

            foreach (var action in _initializer.InputInitialize)
            {
                if (!_inputActions.TryGetValue(action.ActionName, out var inputAction)) { continue; }

                if (action.Value)
                {
                    inputAction.Enable();
                }
                else
                {
                    inputAction.Disable();
                }
            }
        }
        #endregion

        #region Public Methods
        public void DisableAllInputs() => _inputs?.Disable();
        public void EnableAllInputs() => _inputs?.Enable();

        private Dictionary<string, float> _lastActionTimes = new();

        public void ForceDeviceChange(InputDevice device)
        {
            if (device == null)
                return;

            _lastUsedDevice = device;
            OnDeviceChagned?.Invoke(device);

            if (_showLogs)
                Debug.Log($"[InputManager] Forced Device Change: {device.displayName}");
        }


        public bool CanTrigger(string actionName, float debounce = 0.25f)
        {
            if (!_lastActionTimes.TryGetValue(actionName, out var lastTime))
                lastTime = -999f;

            if (Time.unscaledTime - lastTime < debounce)
                return false;

            _lastActionTimes[actionName] = Time.unscaledTime;
            return true;
        }

        public float GetFloatValue(string actionName)
        {
            if (_inputActions.TryGetValue(actionName, out var action))
                return action.ReadValue<float>();

            ReportActionError(actionName);
            return 0f;
        }

        public bool IsActionPressed(string actionName)
        {
            if (_inputActions.TryGetValue(actionName, out var action))
                return action.IsPressed();

            ReportActionError(actionName);
            return false;
        }

        public bool IsActionTriggered(string actionName)
        {
            if (_inputActions.TryGetValue(actionName, out var action))
                return action.triggered;

            ReportActionError(actionName);
            return false;
        }

        public Vector2 GetVector2Value(string actionName)
        {
            if (_inputActions.TryGetValue(actionName, out var action))
                return action.ReadValue<Vector2>();

            ReportActionError(actionName);
            return Vector2.zero;
        }

        public bool IsActionEnabled(string actionName)
        {
            if (_inputActions.TryGetValue(actionName, out var action))
                return action.enabled;

            ReportActionError(actionName);
            return false;
        }

        public bool IsActionMapEnabled(string actionMapName)
        {
            if (_actionMaps.TryGetValue(actionMapName, out var actionMap))
                return actionMap.enabled;

            ReportActionError(actionMapName);
            return false;
        }

        public void EnableActionMapIfDisabled(string actionName)
        {
            if (_actionMaps.TryGetValue(actionName, out var actionMap) && !actionMap.enabled)
                actionMap.Enable();
            else
                ReportActionError(actionName);
        }

        public void EnableActionIfDisabled(string actionName)
        {
            if (!_inputActions.TryGetValue(actionName, out var action))
            {
                ReportActionError(actionName);
                return;
            }

            if (!action.enabled)
            {
                action.Enable();
            }
        }

        public void DisableActionMapIfEnabled(string actionName)
        {
            if (_actionMaps.TryGetValue(actionName, out var actionMap) && actionMap.enabled)
                actionMap.Disable();
            else
                ReportActionError(actionName);
        }

        public void DisableActionIfEnabled(string actionName)
        {
            if (_inputActions.TryGetValue(actionName, out var action) && action.enabled)
                action.Disable();
            else
                ReportActionError(actionName);
        }

        public void DisableInputAction(string actionName)
        {
            if (_inputActions.TryGetValue(actionName, out var action))
                action.Disable();
            else
                ReportActionError(actionName);
        }

        public void EnableInputAction(string actionName)
        {
            if (_inputActions.TryGetValue(actionName, out var action))
                action.Enable();
            else
                ReportActionError(actionName);
        }

        public void DisableActionMap(string actionMapName)
        {
            if (_actionMaps.TryGetValue(actionMapName, out var actionMap))
                actionMap.Disable();
            else
                ReportActionError(actionMapName);
        }

        public void EnableActionMap(string actionMapName)
        {
            if (_actionMaps.TryGetValue(actionMapName, out var actionMap))
                actionMap.Enable();
            else
                ReportActionError(actionMapName);
        }

        public void SubscribeToAction(string actionName, ActionContexts actionContexts)
        {
            if (!_inputActions.TryGetValue(actionName, out var action))
            {
                ReportActionError(actionName);
                return;
            }

            if (actionContexts.ActionStarted != null) action.started += actionContexts.ActionStarted;
            if (actionContexts.ActionPerformed != null) action.performed += actionContexts.ActionPerformed;
            if (actionContexts.ActionCanceled != null) action.canceled += actionContexts.ActionCanceled;
        }

        public void UnsubscribeFromAction(string actionName, ActionContexts actionContexts)
        {
            if (!_inputActions.TryGetValue(actionName, out var action))
            {
                ReportActionError(actionName);
                return;
            }

            if (actionContexts.ActionStarted != null) action.started -= actionContexts.ActionStarted;
            if (actionContexts.ActionPerformed != null) action.performed -= actionContexts.ActionPerformed;
            if (actionContexts.ActionCanceled != null) action.canceled -= actionContexts.ActionCanceled;
        }

        public void StopInputs(string[] exceptions = null)
        {
            _previousEnableActions.Clear();

            foreach (var action in _inputActions.Where(action => action.Value.enabled))
            {
                _previousEnableActions.Add(action.Key);
                action.Value.Disable();
            }

            if (exceptions == null) return;

            foreach (var actionName in exceptions)
                EnableInputAction(actionName);
        }

        public void StopMapInputs(string[] exceptions = null)
        {
            _previousEnableActions.Clear();
            _previousEnableActionsMaps.Clear();

            foreach (var (actionMapName, actionMap) in _actionMaps)
            {
                if (!actionMap.enabled) continue;

                _previousEnableActionsMaps.Add(actionMapName);

                foreach (var action in actionMap.actions.Where(action => action.enabled))
                    _previousEnableActions.Add(action.name);

                actionMap.Disable();
            }

            if (exceptions == null) return;

            foreach (var actionMapName in exceptions)
                EnableActionMap(actionMapName);
        }

        public void ResumeInputs()
        {
            foreach (var action in _previousEnableActions)
                EnableInputAction(action);
        }

        public void ResumeMapInputs()
        {
            foreach (var actionMap in _previousEnableActionsMaps)
                EnableActionMap(actionMap);
        }

        public void ResumeAllInputs()
        {
            ResumeInputs();
            ResumeMapInputs();
        }
        #endregion
    }

    public struct ActionContexts
    {
        public ActionContexts(Action<InputAction.CallbackContext> actionStarted,
            Action<InputAction.CallbackContext> actionPerformed,
            Action<InputAction.CallbackContext> actionCanceled)
        {
            ActionStarted = actionStarted;
            ActionPerformed = actionPerformed;
            ActionCanceled = actionCanceled;
        }

        public Action<InputAction.CallbackContext> ActionStarted;
        public Action<InputAction.CallbackContext> ActionPerformed;
        public Action<InputAction.CallbackContext> ActionCanceled;
    }
}
