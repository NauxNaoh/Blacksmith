using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class ActionHandle : MonoBehaviour
    {
        private CharacterAction characterAction;

        internal CharacterAction CharacterAction => characterAction;

        internal void ChangeCharacterAction(CharacterAction action)
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

    }
}