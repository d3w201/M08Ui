using Controller.Camera;
using Controller.Dialog;
using Controller.Game;
using Controller.Input;
using Controller.Player;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controller
{
    public class RootController : MonoBehaviour
    {
        //Game-Objects
        protected GameObject Chiusky;
        protected GameObject PauseUI;
        protected GameObject Dialog;
        protected GameObject MainCamera;
        protected GameObject GameManager;
        protected GameObject InputManager;
        protected GameObject EventSystemGameObject;

        //Components
        protected InputController InputController;
        protected DialogController DialogController;
        protected GameController GameController;
        protected Animator Animator;
        protected CharacterController CharacterController;
        protected ChiuskyController ChiuskyController;
        protected CameraController CameraController;
        protected EventSystem EventSystem;

        //AnimationID
        protected int AnimIDSpeed;
        protected int AnimIDAim;
        protected int AnimIDAttack;

        //Awake
        protected void Awake()
        {
            if (!PauseUI)
            {
                PauseUI = GameObject.FindGameObjectWithTag("PauseUI");
            }

            if (!Chiusky)
            {
                Chiusky = GameObject.FindGameObjectWithTag("Chiusky");
            }

            if (!Dialog)
            {
                Dialog = GameObject.FindWithTag("DialogAsset");
            }

            if (!MainCamera)
            {
                MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }

            if (!GameManager)
            {
                GameManager = GameObject.FindGameObjectWithTag("GameManager");
            }

            if (!InputManager)
            {
                InputManager = GameObject.FindGameObjectWithTag("InputManager");
            }
            if (!EventSystemGameObject)
            {
                EventSystemGameObject = GameObject.FindGameObjectWithTag("EventSystem");
            }
        }

        //Start
        protected void Start()
        {
            InputController = InputManager.GetComponent<InputController>();
            ChiuskyController = Chiusky.GetComponent<ChiuskyController>();
            Animator = Chiusky.GetComponent<Animator>();
            CharacterController = Chiusky.GetComponent<CharacterController>();
            DialogController = Dialog.GetComponent<DialogController>();
            GameController = GameManager.GetComponent<GameController>();
            CameraController = MainCamera.GetComponent<CameraController>();
            EventSystem = EventSystemGameObject.GetComponent<EventSystem>();

            AssignAnimationIDs();
        }

        //Private-methods
        private void AssignAnimationIDs()
        {
            AnimIDSpeed = Animator.StringToHash("speed");
            AnimIDAim = Animator.StringToHash("aim");
            AnimIDAttack = Animator.StringToHash("attack");
        }
    }
}