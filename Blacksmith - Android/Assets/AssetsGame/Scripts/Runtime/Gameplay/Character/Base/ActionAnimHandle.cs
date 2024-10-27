using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class ActionAnimHandle : MonoBehaviour
    {
        private CharacterAction characterAction;

        public CharacterAction CharacterAction => characterAction;

        public void ChangeCharacterAction(CharacterAction action)
        {
            if (characterAction == action) return;

            characterAction = action;
        }
    }
    public enum CharacterAction
    {
        Idle = 0,
        Moving = 1,
        Working = 2,

        CarryIron = 50,

        CarryWood = 100,

    }
}