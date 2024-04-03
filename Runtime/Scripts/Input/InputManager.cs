using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Events;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;

namespace SLIDDES.UI
{
    /// <summary>
    /// Main manager for all input related stuff.
    /// </summary>
    [AddComponentMenu("SLIDDES/UI/Input/InputManager")]
    [DefaultExecutionOrder(-1)]
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }
        public static bool ApplicationIsQuitting { get; private set; }
        public static bool CurrentInputDeviceIsMouseAndKeyboard
        {
            get
            {
                try
                {
                    return LastPlayerInputPress.PlayerInput.devices[0].ToString() == InputDeviceNameKeyboard
                        && LastPlayerInputPress.PlayerInput.devices[1].ToString() == InputDeviceNameMouse;
                }
                catch
                {
                    return false;
                }
            }
        }
        public static bool HideCursorOnControllerInput
        {
            get
            {
                if(Instance == null) return false;
                return Instance.hideCursorOnControllerInput;
            }
            set
            {
                if(Instance == null) return;
                Instance.hideCursorOnControllerInput = value;
            }
        }
        public static string CurrentInputDeviceName
        {
            get
            {
                if(LastPlayerInputPress == null || LastPlayerInputPress.CurrentInputDevice == null) return InputDeviceNameMouse;
                return LastPlayerInputPress.CurrentInputDevice.ToString();
            }
        }
        /// <summary>
        /// The last player that got their input device changed
        /// </summary>
        public static Player LastPlayerInputDeviceChanged { get; private set; }
        /// <summary>
        /// The last player that pressed input
        /// </summary>
        public static Player LastPlayerInputPress
        {
            get
            {
                return lastPlayerInputPress;
            }
            set
            {
                if(value != lastPlayerInputPress)
                {
                    lastPlayerInputPress = value;

                    if(Instance.showDebug) Debug.Log($"{debugPrefix} Last player that pressed input: {(lastPlayerInputPress != null ? lastPlayerInputPress.Index : "null")}");

                    if(lastPlayerInputPress != null && lastPlayerInputPress.CurrentInputDevice != null) Instance.SetCurrentInputDevice(lastPlayerInputPress.CurrentInputDevice.ToString());

                    if(lastPlayerInputPress != null && Instance.hideCursorOnControllerInput)
                    {                      
                        Cursor.visible = CurrentInputDeviceName == InputDeviceNameKeyboard || CurrentInputDeviceName == InputDeviceNameMouse;
                    }

                    OnLastPlayerInputPressChanged?.Invoke(value);
                }
            }
        }
        /// <summary>
        /// Get all active players
        /// </summary>
        public static Player[] Players => Instance.players.ToArray();
        /// <summary>
        /// The current active sprite asset for UI
        /// </summary>
        public static TMP_SpriteAsset CurrentSpriteAsset
        {
            get
            {
                if (CurrentInputDeviceProfile == null)
                {
                    Debug.LogWarning($"{debugPrefix} CurrentSpriteAsset from CurrentInputDeviceProfile not found, returning default sprite asset");
                    return Instance.defaultSpriteAsset;
                }
                return CurrentInputDeviceProfile.CurrentSpriteAsset;
            }
        }
        public static InputDeviceProfile CurrentInputDeviceProfile
        {
            get
            {
                return Instance.inputDeviceProfiles.FirstOrDefault(x => x.inputDeviceNames.Contains(CurrentInputDeviceName));
            }
        }
        public static UnityAction OnForceUpdateInputPrompts { get; set; }
        /// <summary>
        /// Invokes when any of the players current input device changes
        /// </summary>
        /// <value>latest changed playerInput.CurrentInputDevice name</value>
        public static UnityAction<string> OnInputDeviceNameChanged { get; set; }
        public static UnityAction<Player> OnLastPlayerInputPressChanged { get; set; }

        public static readonly string InputDeviceNameMouse = "Mouse:/Mouse";
        public static readonly string InputDeviceNameKeyboard = "Keyboard:/Keyboard";
        public static readonly string InputDeviceNameXboxController = "XInputControllerWindows:/XInputControllerWindows";
        public static readonly string InputDeviceNameXboxOneController = "XInputControllerWindows:/XInputControllerWindows1";
        public static readonly string InputDeviceNamePS4Controller = "DualShock4GamepadHID:/DualShock4GamepadHID";
        public static readonly string InputDeviceNamePS5Controller = "DualSenseGamepadHID:/DualSenseGamepadHID";

        private static string currentInputDeviceName;
        private static Player lastPlayerInputPress;

        [SerializeField] private bool loadAndSaveInput = true;
        [Tooltip("When a player gains control over the UI, set its corresponding player UI input module to the correct input")]
        [SerializeField] private bool autoSwitchPlayerUIInputModule = true;
        [Tooltip("When a controller takes over input, hide the mouse cursor")]
        [SerializeField] private bool hideCursorOnControllerInput = true;
        [Tooltip("When updating players, assign the player inputActionAsset. Set to false if using PlayerInputManager")]
        [SerializeField] private bool assignPlayerInputActionAsset;
        [SerializeField] private InputActionAsset inputActionAsset;
        [Tooltip("Time to wait before switching to another input device of a player")]
        [SerializeField] private float inputDeviceChangeMinTime = 0.5f;
        [Tooltip("Time to wait before allowing switching to another player")]
        [SerializeField] private float lastPlayerInputPressChangeMinTime = 0.5f;
        [SerializeField] private InputDeviceProfile[] inputDeviceProfiles;
        [SerializeField] private TMP_SpriteAsset defaultSpriteAsset;

        [SerializeField] private bool showDebug;

        private static readonly string debugPrefix = "[InputManager]";
        private static readonly string savePrefix = "InputRebindManager";

        private float lastPlayerInputPressChangeTimer;
        private List<Player> players = new List<Player>();
        private List<InputActionReferenceMultiplayer> inputActionReferencesMultiplayer = new List<InputActionReferenceMultiplayer>();

        private void Awake()
        {
            if(Instance != null)
            {
                Debug.LogWarning($"{debugPrefix} Instance of this already active, destroying this one...");
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            ApplicationIsQuitting = false;
            UpdatePlayers();
        }

        private void OnEnable()
        {
            Application.quitting += () => { ApplicationIsQuitting = true; };
            UpdatePlayers();
        }

        private void OnDisable()
        {
            Application.quitting -= () => { ApplicationIsQuitting = true; };            
        }

        private void Update()
        {
            CheckPlayers();
        }

        private void LateUpdate()
        {
            CheckPlayersLate();
        }

        /// <summary>
        /// Check if an input action reference can have its values displayed
        /// </summary>
        /// <param name="inputActionReference"></param>
        /// <returns></returns>
        public static bool AbleToDisplayInputActionReference(InputActionReference inputActionReference)
        {
            string bindingID = GetBindingID(inputActionReference);
            if(string.IsNullOrEmpty(bindingID)) return false;

            string stringInputBind = GetInputPrompt(inputActionReference);
            if(string.IsNullOrEmpty(stringInputBind)) return false;

            return true;
        }

        /// <summary>
        /// Get the input prompt from an inputActionReference
        /// </summary>
        /// <param name="inputActionReference">the reference to get the prompt for</param>
        /// <returns>prompt string</returns>
        /// <example>
        /// inputActionReference bound to "D-Pad/Down" returns <sprite name="D-Pad/Down">
        /// </example>
        public static string GetInputPrompt(InputActionReference inputActionReference)
        {
            if(inputActionReference == null || inputActionReference.action == null) return "";

            InputActionReference playerClonedInputActionReference;
            if(Application.isPlaying)
            {
                // Since each player has their own cloned InputActionAsset we need to find the same inputActionReference in the asset
                // Get the cloned version of the inputActionReference
                if(LastPlayerInputPress == null || LastPlayerInputPress.InputActionAsset == null)
                {
                    return "";
                }
                InputAction foundAction = LastPlayerInputPress.InputActionAsset.FindAction(inputActionReference.action.id);
                if(foundAction == null)
                {
                    return "";
                }
                playerClonedInputActionReference = ScriptableObject.CreateInstance<InputActionReference>();
                playerClonedInputActionReference.Set(foundAction);
            }
            else
            {
                // Work in editor
                playerClonedInputActionReference = inputActionReference;
            }


            string bindingID = GetBindingID(playerClonedInputActionReference);
            int bindingIndex = playerClonedInputActionReference.action.bindings.IndexOf(x => x.id.ToString() == bindingID);

            if(bindingIndex != -1)
            {
                string input;
                try
                {
                    input = playerClonedInputActionReference?.action.GetBindingDisplayString(bindingIndex);
                }
                catch
                {
                    return "";
                }

                //Debug.Log(input);

                // "D-Pad/Down"
                // "W|Num 8/A|Num 4/S|Num 2/D|Num 6"

                string result = "";
                if(input.Length == 1)
                {
                    // If only 1 character
                    result = $"<sprite name=\"{input}\">";
                }
                else
                {
                    // Use regex magic
                    string replacement = "<sprite name=\"$1\">";
                    string pattern = @"((\w+-\w+|\w+)(\s(\w+|\d+))?(\s(\w+))?)"; // seems to work :)
                    result = Regex.Replace(input, pattern, replacement);
                }

                return result;
            }

            return "";
        }

        /// <summary>
        /// Get a binding ID string
        /// </summary>
        /// <param name="inputActionReference">The input action reference to get the binding id from</param>
        /// <returns>string with binding ID or empty</returns>
        public static string GetBindingID(InputActionReference inputActionReference, Player player = null)
        {
            if(ApplicationIsQuitting) return "";

            string currentControlScheme;
            if(Application.isPlaying)
            {
                if(player == null) player = LastPlayerInputPress;
                currentControlScheme = player.PlayerInput.currentControlScheme;
                if(string.IsNullOrEmpty(currentControlScheme)) currentControlScheme = InputDeviceNameMouse;
            }
            else
            {
                // Work in editor
                currentControlScheme = InputDeviceNameMouse;
            }

            for(int i = 0; i < inputActionReference.action.bindings.Count; i++)
            {
                InputBinding inputBinding = inputActionReference.action.bindings[i];

                // Check if inputBinding controlScheme contains name of currentControlScheme
                // Check if input binding group is part of a composite
                if(string.IsNullOrEmpty(inputBinding.groups))
                {
                    // Part of a composite, check bounds
                    // Check next one if it contains currentControlScheme
                    if(i + 1 <= inputActionReference.action.bindings.Count - 1)
                    {
                        if(inputActionReference.action.bindings[i + 1].groups.Contains(currentControlScheme))
                        {
                            // Return the current index
                            return inputActionReference.action.bindings[i].id.ToString();
                        }
                    }
                }

                if(inputBinding.groups.Contains(currentControlScheme))
                {
                    return inputBinding.id.ToString();
                }
            }

            return "";
        }

        public static void ForceUpdateInputPrompts()
        {
            if(Instance.showDebug) Debug.Log($"{debugPrefix} Force update input prompts");
            OnForceUpdateInputPrompts?.Invoke();
        }

        /// <summary>
        /// Set a callback for when input phase changes from a inputActionAsset
        /// </summary>
        /// <param name="inputActionAsset"></param>
        /// <param name="add"></param>
        /// <param name="action"></param>
        public static void SetCallbackOnActionTriggerd(InputActionAsset inputActionAsset, bool add, System.Action<InputAction.CallbackContext> action)
        {
            foreach(InputActionMap inputActionMap in inputActionAsset.actionMaps)
            {
                if(add)
                {
                    inputActionMap.actionTriggered += action;
                }
                else
                {
                    inputActionMap.actionTriggered -= action;
                }
            }
        }

        public static void AddInputActionReferenceMultiplayer(InputActionReferenceMultiplayer iarm)
        {
            // Check instance
            if(Instance == null)
            {
                Debug.LogWarning($"{debugPrefix} Cannot add iarm cause instance is null");
                return;
            }

            // If already added dont
            if(Instance.inputActionReferencesMultiplayer.Contains(iarm))
            {
                Debug.LogWarning($"{debugPrefix} IARM was already added");
                return;
            }

            Instance.inputActionReferencesMultiplayer.Add(iarm);
            Instance.UpdatePlayersInputActionReferenceMultiplayer(iarm, true);
        }

        public static void RemoveInputActionReferenceMultiplayer(InputActionReferenceMultiplayer iarm)
        {
            // Check instance
            if(Instance == null)
            {
                Debug.LogWarning($"{debugPrefix} Cannot remove iarm because inputManager instance is null (applicationIsQuitting: {ApplicationIsQuitting})");
                return;
            }

            // If already added dont
            if(!Instance.inputActionReferencesMultiplayer.Contains(iarm))
            {
                Debug.LogWarning($"{debugPrefix} IARM was already removed");
                return;
            }

            Instance.inputActionReferencesMultiplayer.Remove(iarm);
            Instance.UpdatePlayersInputActionReferenceMultiplayer(iarm, false);
        }

        /// <summary>
        /// Sets the current active player as only active input
        /// </summary>
        public static void SetPlayerInputSolo()
        {
            foreach(Player player in Players)
            {
                if(player == LastPlayerInputPress) continue;
                player.DisableInput();
            }
        }

        /// <summary>
        /// Enable all player input
        /// </summary>
        public static void SetPlayerInputAll()
        {
            foreach(Player player in Players)
            {
                player.EnableInput();
            }
        }

        /// <summary>
        /// Disable all event system input but the current one
        /// </summary>
        public static void SetMultiplayerEventSystemInputSolo()
        {
            foreach(var player in Players)
            {
                if(player.MultiplayerEventSystem == EventSystem.current) continue;
                player.MultiplayerEventSystem.enabled = false;
            }
        }

        /// <summary>
        /// Enable all event system inputs
        /// </summary>
        public static void SetMultiplayerEventSystemInputAll()
        {
            foreach(var player in Players)
            {
                player.MultiplayerEventSystem.enabled = true;
            }
        }

        public static void PlayersSwitchActionMap(string mapNameOrID)
        {
            foreach(Player player in Players)
            {
                if(player == null || player.PlayerInput == null) continue;
                if(!player.PlayerInput.inputIsActive || !player.PlayerInput.isActiveAndEnabled) continue;
                player.PlayerInput.SwitchCurrentActionMap(mapNameOrID);
            }
        }

        private void UpdatePlayersInputActionReferenceMultiplayer(InputActionReferenceMultiplayer iarm, bool add)
        {
            if(ApplicationIsQuitting) return;

            for (int i = 0; i < players.Count; i++)
            {
                if(players[i] == null) continue;
                if(players[i].InputActionAsset == null) continue;

                if(add)
                {
                    iarm.AddCallbackToAction(players[i].InputActionAsset.FindAction(iarm.ActionName));
                }
                else
                {
                    iarm.RemoveCallbackFromAction(players[i].InputActionAsset.FindAction(iarm.ActionName));
                }
            }
        }

        private void RemoveAllPlayersInputActionReferenceMultiplayer()
        {
            for(int i = 0; i < players.Count; i++)
            {
                for(int j = 0; j < inputActionReferencesMultiplayer.Count; j++)
                {
                    if(players[i] == null) continue;
                    if(players[i].InputActionAsset == null) continue;
                    InputActionReferenceMultiplayer iarm = inputActionReferencesMultiplayer[j];
                    iarm.RemoveCallbackFromAction(players[i].InputActionAsset.FindAction(iarm.ActionName));
                }
            }
        }

        public void UpdatePlayers()
        {
            if(ApplicationIsQuitting)
            {
                Debug.LogWarning($"{debugPrefix} Will not update players cause application is quitting");
                return;
            }

            // Remove all iarm (gets added back later)
            RemoveAllPlayersInputActionReferenceMultiplayer();

            // Clear old
            players.Clear();
            LastPlayerInputPress = null;
            LastPlayerInputDeviceChanged = null;

            // Get all players
            PlayerInput[] playersInScene = FindObjectsOfType<PlayerInput>(true);
            Debug.Log($"{debugPrefix} Found {playersInScene.Length} players");
            for(int i = 0; i < playersInScene.Length; i++)
            {
                players.Add(new Player(playersInScene[i]));
            }

            Debug.Log($"{debugPrefix} Updated {players.Count} players");

            if(players.Count > 0)
            {
                LastPlayerInputPress = players[players.Count - 1];
                LastPlayerInputDeviceChanged = players[players.Count - 1];
            }

            LoadPlayersInputActionAssets();
        }

        public void LoadPlayersInputActionAssets()
        {
            for(int i = 0; i < players.Count; i++)
            {
                // Assign the inputActionAsset to the player. The player will create its own unique clone of the asset
                // Recommended that this is set to false
                if(assignPlayerInputActionAsset) players[i].InputActionAsset = inputActionAsset; 
                
                // Assign any rebinds
                if(showDebug) Debug.Log($"{debugPrefix} Loading rebinds inputactionasset for player {players[i].Index}...");
                string rebinds = PlayerPrefs.GetString(GetPlayerPrefsKeyRebinds(players[i].Index));
                if(!string.IsNullOrEmpty(rebinds) && loadAndSaveInput)
                {
                    try
                    {
                        players[i].InputActionAsset.LoadBindingOverridesFromJson(rebinds);
                        if(showDebug) Debug.Log($"{debugPrefix} Loaded rebinds from InputActionAsset: {rebinds}");
                    }
                    catch
                    {
                        if(showDebug) Debug.Log($"{debugPrefix} Failed to load rebinds from InputActionAsset (Can happen if input action asset was changed)");
                    }
                }
                else
                {
                    if(showDebug) Debug.Log($"{debugPrefix} No rebinds for inputactionasset");
                }

                players[i].InputActionAsset.Enable();
                // Set players uiInputModule
                if(players[i].PlayerInput.uiInputModule != null)
                {
                    players[i].PlayerInput.uiInputModule.actionsAsset = players[i].InputActionAsset;
                }
                else
                {
                    Debug.LogWarning($"{debugPrefix} Player {players[i].Index} uiInputModule is not set");
                }

                // Add inputActionReferencesMultiplayer
                for(int j = 0; j < inputActionReferencesMultiplayer.Count; j++)
                {
                    InputActionReferenceMultiplayer iarm = inputActionReferencesMultiplayer[j];
                    iarm.AddCallbackToAction(players[i].InputActionAsset.FindAction(iarm.ActionName));
                }
            }
        }

        public void SavePlayersInputActionAssetRebinds()
        {
            if(!loadAndSaveInput) return;

            for(int i = 0; i < players.Count; i++)
            {
                if(players[i].InputActionAsset == null)
                {
                    if(showDebug) Debug.LogWarning($"{debugPrefix} Tried saving input action asset that was null");
                    continue;
                }

                string rebinds = players[i].InputActionAsset.SaveBindingOverridesAsJson();
                if(showDebug) Debug.Log($"Rebinds: {rebinds}");
                PlayerPrefs.SetString(GetPlayerPrefsKeyRebinds(players[i].Index), rebinds);
                if(showDebug) Debug.Log($"{debugPrefix} Saved InputActionAsset for player {players[i].Index}");

                // Update player inputSystemUIInputModule if present
                if(players[i].PlayerInput.uiInputModule != null)
                {
                    players[i].PlayerInput.uiInputModule.actionsAsset = players[i].InputActionAsset;
                }
            }
        }

        public void SetCurrentInputDevice(string inputDeviceName)
        {
            if(ApplicationIsQuitting) return;
            if(inputDeviceName == CurrentInputDeviceName) return;

            //CurrentInputDeviceName = inputDeviceName;

            if(showDebug) Debug.Log($"{debugPrefix} Input device name changed to: {inputDeviceName}");

            OnInputDeviceNameChanged.Invoke(inputDeviceName);
        }

        public void ResetAllInputActionAssetRebinds()
        {
            inputActionAsset.RemoveAllBindingOverrides();
            for(int i = 0; i < players.Count; i++)
            {
                players[i].InputActionAsset.RemoveAllBindingOverrides();
            }

            SavePlayersInputActionAssetRebinds();
            ForceUpdateInputPrompts();
        }

        public void ResetCurrentPlayerInputActionAssetRebinds()
        {
            if(LastPlayerInputPress == null) return;
            
            LastPlayerInputPress.InputActionAsset.RemoveAllBindingOverrides();

            SavePlayersInputActionAssetRebinds();
            ForceUpdateInputPrompts();
        }

        private string GetPlayerPrefsKeyRebinds(int playerIndex)
        {
            return $"{Application.productName}_{savePrefix}_Rebinds_{playerIndex}";
        }

        private void CheckPlayers()
        {
            Player lastPlayerInputPress = null;

            for(int i = 0; i < players.Count; i++)
            {
                Player player = players[i];
                if(player.Index == -1) continue;

                CheckPlayerInputHasChanged(player);
                
                if(player.PressedInputThisFrame)
                {   

                    lastPlayerInputPress = player;
                }
            }

            if(lastPlayerInputPressChangeTimer < lastPlayerInputPressChangeMinTime)
            {
                lastPlayerInputPressChangeTimer += Time.unscaledDeltaTime;
            }
            else
            {
                if(lastPlayerInputPress != null)
                {
                    // Set new
                    LastPlayerInputPress = lastPlayerInputPress;
                    lastPlayerInputPressChangeTimer = 0;
                }
            }

        }

        private void CheckPlayersLate()
        {
            for(int i = 0; i < players.Count; i++)
            {
                Player player = players[i];
                if(player.Index == -1) continue;

                // Reset
                player.PressedInputThisFrame = false;
            }
        }

        private bool CheckPlayerInputHasChanged(Player player)
        {
            // Check if input changed
            if(player.TimeSinceLastSwitch < inputDeviceChangeMinTime)
            {
                player.TimeSinceLastSwitch += Time.unscaledDeltaTime;
                return false;
            }
            player.TimeSinceLastSwitch = 0;

            if(player.PlayerInput.devices.Count <= 0) return false;

            InputDevice inputDevice = player.PlayerInput.devices[0];
            if(inputDevice != null && player.CurrentInputDevice != inputDevice)
            {
                // New input device
                if(showDebug) Debug.Log($"{debugPrefix} {player.PlayerInput.name} switched input device {player.CurrentInputDevice} to {inputDevice}");
                player.CurrentInputDevice = inputDevice;

                LastPlayerInputDeviceChanged = player;
                SetCurrentInputDevice(player.CurrentInputDevice.ToString());
                return true;
            }
            return false;
        }

        [System.Serializable]
        public class InputDeviceProfile
        {
            public TMP_SpriteAsset CurrentSpriteAsset => spriteAssets[spriteAssetIndex];

            public string label;
            public string[] inputDeviceNames;

            public int spriteAssetIndex;

            public TMP_SpriteAsset[] spriteAssets = new TMP_SpriteAsset[1];

            public InputDeviceProfile(string label, string inputDeviceName)
            {
                this.label = label;
                this.inputDeviceNames = new string[] { inputDeviceName };
            }

            public InputDeviceProfile(string label, string[] inputDeviceNames)
            {
                this.label = label;
                this.inputDeviceNames = inputDeviceNames;
            }
        }
                
        public class Player
        {
            public bool PressedInputThisFrame { get; set; }
            public int Index => PlayerInput.playerIndex;
            public float TimeSinceLastSwitch
            {
                get { return timeSinceLastSwitch; }
                set
                {
                    timeSinceLastSwitch = Mathf.Clamp(value, 0, 10);                    
                }
            }
            public InputDevice CurrentInputDevice { get; set; }
            public InputActionAsset InputActionAsset 
            {
                get 
                {
                    if(PlayerInput == null) return null;
                    return PlayerInput.actions; 
                }
                set { PlayerInput.actions = value; }
            }
            public PlayerInput PlayerInput
            {
                get
                {
                    return playerInput;
                }
                set
                {
                    if(playerInput != null)
                    {
                        InputManager.SetCallbackOnActionTriggerd(playerInput.actions, false, OnActionTriggerd);
                    }
                    playerInput = value;
                    if(playerInput != null)
                    {
                        InputManager.SetCallbackOnActionTriggerd(playerInput.actions, true, OnActionTriggerd);
                    }
                }
            }
            public Canvas Canvas { get; set; }
            public InputDevice[] InputDevices { get; set; }
            public MultiplayerEventSystem MultiplayerEventSystem => playerInput.uiInputModule.GetComponent<MultiplayerEventSystem>();

            private float timeSinceLastSwitch;
            private PlayerInput playerInput;

            public Player(PlayerInput playerInput)
            {
                PlayerInput = playerInput;
                if(playerInput.devices.Count > 0) CurrentInputDevice = playerInput.devices[0];
            }

            public void EnableInput()
            {
                playerInput.ActivateInput();
            }

            public void DisableInput()
            {
                playerInput.DeactivateInput();
            }

            private void OnActionTriggerd(InputAction.CallbackContext context)
            {
                if(context.phase == InputActionPhase.Performed || context.phase == InputActionPhase.Canceled)
                {
                    // When a value like 0 is detected, dont set input pressed this frame
                    try
                    {
                        if(context.ReadValue<float>() > 0)
                        {
                            PressedInputThisFrame = true;
                        }
                    }
                    catch
                    {
                        try
                        {
                            if(context.ReadValue<Vector2>() != Vector2.zero)
                            {
                                PressedInputThisFrame = true;
                            }
                        }
                        catch
                        {

                        }
                    }

                    //PressedInputThisFrame = true;
                }
            }
        }
    }
}
