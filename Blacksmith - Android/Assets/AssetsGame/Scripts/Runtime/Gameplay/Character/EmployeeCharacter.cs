using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class EmployeeCharacter : Character
    {
        [SerializeField] private EmployeeInfo employeeInfo;


         void Start()
        {
            Initialized();
        }

        public override void Initialized()
        {
        }
        public override void Move(Vector3 direction)
        {
            if (actionHandle.CharacterAction != CharacterAction.Moving) return;
            //characterController.Move(direction * Time.deltaTime * speed);
        }

    }

    
}