using UnityEngine;
using System.Collections;
using DaggerfallWorkshop.Game;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using DaggerfallWorkshop.Game.UserInterfaceWindows;
using System;
using DaggerfallWorkshop;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Utility;

namespace DaggerfallMod
{
    public class DropACoin : MonoBehaviour
    {
        private Mod mod;
        private KeyCode toggleKey = KeyCode.O;

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
            }
            else
            {
                DaggerfallUnity.LogMessage("DropACoin: Hotkey could not be processed! Falling back to 'O'!", true);
            }
        }

        void Update()
        {
            if (GameManager.Instance.StateManager.CurrentState == StateManager.StateTypes.Game && Input.GetKeyDown(toggleKey))
            {
                ItemCollection inventory = GameManager.Instance.PlayerEntity.Items;

                if (GameManager.Instance.PlayerEntity.GoldPieces > 0)
                {
                    var loot = GameObjectHelper.CreateDroppedLootContainer(GameManager.Instance.PlayerObject, DaggerfallUnity.NextUID);
                    loot.ContainerImage = InventoryContainerImages.Ground;
                    loot.ContainerType = LootContainerTypes.RandomTreasure;
                    loot.Items.AddItem(ItemBuilder.CreateGoldPieces(1));
                    GameManager.Instance.PlayerEntity.GoldPieces -= 1;

                    DaggerfallUI.AddHUDText("You dropped 1 gold piece!");
                } else
                {
                    DaggerfallUI.AddHUDText("You don't have any gold!");
                }
            }
        }

        [Invoke(StateManager.StateTypes.Game)]
        public static void Init(InitParams initParams)
        {
            var instance = GameManager.Instance.PlayerObject.AddComponent<DropACoin>();
            instance.Setup(initParams.Mod);
        }
    }
}
