using UnityEngine;
using System.Collections;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using System;
using DaggerfallWorkshop;

namespace DaggerfallMod
{
    public class ToggleMount : MonoBehaviour
    {
        private Mod mod;
        private KeyCode toggleKey = KeyCode.G;

        public void Setup(Mod mod)
        {
            this.mod = mod;
            mod.IsReady = true;

            var settings = mod.GetSettings();
            string keyName = settings.GetValue<string>("General", "Hotkey");

            var key = (KeyCode)Enum.Parse(typeof(KeyCode), keyName);

            if (Enum.IsDefined(typeof(KeyCode), key))
            {
                toggleKey = key;
            } else
            {
                DaggerfallUnity.LogMessage("ToggleMount: Hotkey could not be processed! Falling back to 'G'!", true);
            }
        }
        
        void Update()
        {
            if (GameManager.Instance.StateManager.CurrentState == StateManager.StateTypes.Game)
            {
                if (Input.GetKeyDown(toggleKey))
                {
                    if (GameManager.Instance.IsPlayerInside)
                    {
                        DaggerfallUI.AddHUDText(HardStrings.cannotChangeTransportationIndoors);
                    }
                    else
                    {
                        if (GameManager.Instance.PlayerController.isGrounded)
                        {
                            GameManager.Instance.TransportManager.ToggleMount();
                        }
                    }
                }
            }
        }
        
        [Invoke(StateManager.StateTypes.Game)]
        public static void Init(InitParams initParams)
        {
            var instance = GameManager.Instance.PlayerObject.AddComponent<ToggleMount>();
            instance.Setup(initParams.Mod);
        }
    }
}
