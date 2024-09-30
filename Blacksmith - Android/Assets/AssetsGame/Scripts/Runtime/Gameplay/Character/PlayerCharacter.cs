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
                actionHandle.ChangeCharacterAction(CharacterAction.Moving);
            }
            else
                actionHandle.ChangeCharacterAction(CharacterAction.Idle);
        }
        void FixedUpdate()
        {
            Move(destination);
        }

        public override void Initialized()
        {
            characterController = GetComponent<CharacterController>();
        }
        public override void Move(Vector3 direction)
        {
            if (actionHandle.CharacterAction != CharacterAction.Moving) return;
            characterController.Move(direction * Time.deltaTime * speed);
        }
    }
}