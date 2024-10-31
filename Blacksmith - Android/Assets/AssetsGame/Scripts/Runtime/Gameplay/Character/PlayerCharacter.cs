using Naux.Joystick;
using System.Collections;
using UnityEngine;

namespace Runtime
{
    public class PlayerCharacter : Character
    {
        [Space]
        [SerializeField] private Joystick joystick;
        [SerializeField] private WorkingActionHandler workingActionHandler;

        private CharacterController characterController;
        private Vector3 destination = Vector3.zero;


        private void Start()
        {
            Initialized();
            workingActionHandler.Initialized();
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

        public override void WorkingOnWorkArea(AreaType areaType)
        {
            var _workingType = ConvertAreaTypeToWorkingType(areaType);

            workingActionHandler.DoWorkingType(_workingType);
        }

        WorkingType ConvertAreaTypeToWorkingType(AreaType areaType)
        {
            switch (areaType)
            {
                case AreaType.None:
                default:
                    return WorkingType.None;
                case AreaType.BlueprintsArea:
                    return WorkingType.BlueprintBoardWorking;
                case AreaType.MissionBoardArea:
                    return WorkingType.MissionBoardWorking;
                case AreaType.IronBarrelArea:
                    return WorkingType.IronBarrelWorking;
                case AreaType.KilnArea:
                    return WorkingType.KilnWorking;
                case AreaType.WoodBarrelArea:
                    return WorkingType.WoodBarrelWorking;
                case AreaType.CraftTableArea:
                    return WorkingType.CraftTableWorking;
            }
        }



        void SwitchAction(AreaType areaType)
        {
            switch (areaType)
            {
                case AreaType.None:
                    break;
                case AreaType.MissionBoardArea:
                    break;

                case AreaType.IronBarrelArea:
                    actionAnimHandle.ChangeCharacterAction(CharacterAction.CarryIron);
                    break;
                case AreaType.WoodBarrelArea:
                    actionAnimHandle.ChangeCharacterAction(CharacterAction.CarryWood);
                    break;

                case AreaType.KilnArea:
                    //action
                    break;
                case AreaType.CraftTableArea:
                    //action
                    break;
                case AreaType.BlueprintsArea:
                    //action ?
                    break;
                default:
                    break;

            }
        }
        
        

        //    InjectionLocalHelper.Instance.LocalPopupHandle.HideLocalPopup();
       


        //write context menu auto ref for character


        
    }
}