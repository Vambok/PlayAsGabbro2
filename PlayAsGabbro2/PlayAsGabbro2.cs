using OWML.Common;
using OWML.ModHelper;
using UnityEngine;

namespace PlayAsGabbro2 {
    public class PlayAsGabbro2 : ModBehaviour {
        public static PlayAsGabbro2 Instance;

        PromptManager promptManager;
        ScreenPrompt prompt;
        float notificationDelay = 0;

        public void Awake() {
            Instance = this;
        }

        public void Start() {
            OnCompleteSceneLoad(OWScene.TitleScreen, OWScene.TitleScreen);
            LoadManager.OnCompleteSceneLoad += OnCompleteSceneLoad;
        }

        public void OnCompleteSceneLoad(OWScene previousScene, OWScene newScene) {
            if(newScene != OWScene.SolarSystem) return;
            ModHelper.Console.WriteLine("Loaded into solar system!", MessageType.Success);
            prompt = new ScreenPrompt("I don't feel like exploring today. Let's play the flute and enjoy the weather.");
            ModHelper.Events.Unity.FireInNUpdates(() => {
                Transform toto = new GameObject("toto").transform;
                toto.SetParent(GameObject.Find("gabbro_OW_V02:gabbro_rig_v01:Head_Top_Jnt").transform);
                toto.localPosition = new Vector3(1, 0, 0);
                toto.localEulerAngles = new Vector3(90, 270, 0);
                Transform playerCam = GameObject.Find("PlayerCamera").transform;
                playerCam.SetParent(toto);
                promptManager = Locator.GetPromptManager();
                promptManager._bottomLeftList._listPrompts.Clear();
                GameObject playerBody = GameObject.Find("Player_Body");
                playerBody.GetComponent<PlayerCharacterController>().LockMovement();//enabled = false;
                Locator.GetDeathManager().ToggleInvincibility();
                playerBody.GetComponent<PlayerBody>().SetPosition(GameObject.Find("Prefab_NOM_SharedStone_TimeLoop").transform.position);
                playerBody.transform.Find("Traveller_HEA_Player_v2").gameObject.SetActive(false);
                playerBody.transform.Find("Audio_Player").gameObject.SetActive(false);
                Transform playerDetector = playerBody.transform.Find("PlayerDetector");
                playerDetector.SetParent(playerCam);
                playerDetector.localPosition = Vector3.zero;
            }, 31);
        }

        void Update() {
            if(Time.timeSinceLevelLoad > notificationDelay + 4 && (OWInput.IsNewlyPressed(InputLibrary.tab) || OWInput.IsNewlyPressed(InputLibrary.jump) || OWInput.IsNewlyPressed(InputLibrary.left) || OWInput.IsNewlyPressed(InputLibrary.right) || OWInput.IsNewlyPressed(InputLibrary.up) || OWInput.IsNewlyPressed(InputLibrary.down))) {
                promptManager.AddScreenPrompt(prompt, PromptPosition.Center, true);
                ModHelper.Events.Unity.FireInNUpdates(() => { promptManager.RemoveScreenPrompt(prompt, PromptPosition.Center); }, 200);
                notificationDelay = Time.timeSinceLevelLoad;
            }
        }
    }

}
