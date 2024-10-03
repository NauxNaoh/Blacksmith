using Naux.Joystick;
using UnityEngine;

namespace Runtime
{
    public class PlayerCharacter : Character
    {
        [Space]
        [SerializeField] private Joystick joystick;

        private CharacterController characterController;
        private Vector3 destination = Vector3.zero;

        void Start()
        {
            Initialized();
        }

        void Update()
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
        void FixedUpdate()
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
            Debug.Log($"{areaType} doing!");

        }


        void SwitchAction(AreaType areaType)
        {
            //use for now, change to stratery pattern?
            switch (areaType)
            {
                case AreaType.None:
                    break;
                case AreaType.IronBarrelArea:
                    actionHandle.ChangeCharacterAction(CharacterAction.CarryIron);
                    break;
                case AreaType.WoodBarrelArea:
                    actionHandle.ChangeCharacterAction(CharacterAction.CarryWood);
                    break;
                default:
                    break;
            }
        }
    }
}