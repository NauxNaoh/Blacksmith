using Naux.Joystick;
using System.Collections;
using UnityEngine;

namespace Runtime
{
    public class PlayerCharacter : Character
    {
        [Space]
        [SerializeField] private Joystick joystick;

        private CharacterController characterController;
        private Vector3 destination = Vector3.zero;

        private KilnMiniGameHandle kilnMiniGameHandle;
        private CraftMiniGameHandle craftMiniGameHandle;

        private void Start()
        {
            Initialized();
            StartCoroutine(RegisterInstanceMiniGame());
        }
        IEnumerator RegisterInstanceMiniGame()
        {
            yield return new WaitUntil(() => InjectionHelper.Instance != null);
            kilnMiniGameHandle = InjectionHelper.Instance.KilnMiniGameHandle;
            craftMiniGameHandle = InjectionHelper.Instance.CraftMiniGameHandle;
        }

        private void Update()
        {
            var _joystickDir = joystick.Direction;
            if (_joystickDir.magnitude > 0)
            {
                destination.x = _joystickDir.x;
                destination.z = _joystickDir.y;
            }
            else
                destination = Vector3.zero;
        }
        private void FixedUpdate()
        {
            Move(destination);
        }

        void OnTriggerStay(Collider other)
        {
            other.TryGetComponent<WorkingRange>(out var _workingRange);
            if (_workingRange == null) return;

            _workingRange.StandOnWorkingRange(this);
        }
        void OnTriggerExit(Collider other)
        {
            other.TryGetComponent<WorkingRange>(out var _workingRange);
            if (_workingRange == null) return;

            _workingRange.ExitOnWorkingRange();
        }

        public override void Initialized()
        {
            characterType = CharacterType.Player;
            characterController = GetComponent<CharacterController>();
        }
        public override void Move(Vector3 direction)
        {
            characterController.Move(direction * Time.fixedDeltaTime * speed);
        }

        public override void WorkingForNowHAHA(AreaType areaType)
        {
            //Play action carry resource
            SwitchAction(areaType);
            Debug.Log($"character doing {areaType}!");

        }


        void SwitchAction(AreaType areaType)
        {
            //use for now, change to stratery pattern?
            switch (areaType)
            {
                case AreaType.None:
                    break;
                case AreaType.MissionBoardArea:
                    break;

                case AreaType.IronBarrelArea:
                    actionHandle.ChangeCharacterAction(CharacterAction.CarryIron);
                    break;
                case AreaType.WoodBarrelArea:
                    actionHandle.ChangeCharacterAction(CharacterAction.CarryWood);
                    break;

                case AreaType.KilnArea:
                    StartCoroutine(DoingKilnArea());
                    break;
                case AreaType.CraftTableArea:
                    StartCoroutine(DoingCraftTableArea());
                    break;
                default:
                    break;
            }
        }
        IEnumerator DoingKilnArea()
        {
            kilnMiniGameHandle.BoardGameInitialized();
            InjectionHelper.Instance.LocalPopupHandle.ShowLocalPopup(PopupType.KilnMiniGame);
            yield return StartCoroutine(kilnMiniGameHandle.StartMiniGameRoutine(CallBackTest));

            InjectionHelper.Instance.LocalPopupHandle.HideLocalPopup();
        }
        IEnumerator DoingCraftTableArea()
        {
            craftMiniGameHandle.BoardGameInitialized();
            InjectionHelper.Instance.LocalPopupHandle.ShowLocalPopup(PopupType.CraftMiniGame);
            yield return StartCoroutine(craftMiniGameHandle.StartMiniGameRoutine(CallBackTest));

            InjectionHelper.Instance.LocalPopupHandle.HideLocalPopup();
        }

        void CallBackTest(bool status)
        {
            Debug.LogError($"status mini game = {status}");
        }
    }
}