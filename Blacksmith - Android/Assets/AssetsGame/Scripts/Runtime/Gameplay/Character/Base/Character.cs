using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] protected ActionHandle actionHandle;
        [SerializeField] protected CharacterType characterType;
        
        public float speed = 5; //change?
        public CharacterType CharacterType => characterType;


        public virtual void Initialized() { }

        public virtual void Move(Vector3 direction) { }

        public virtual void WorkingForNowHAHA(AreaType areaType) { }
    }

    public enum CharacterType
    {
        None = 0,
        Player = 1,
        Employee = 2,
    }
}
