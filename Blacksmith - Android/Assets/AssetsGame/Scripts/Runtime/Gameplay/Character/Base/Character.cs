using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public abstract class Character : MonoBehaviour, IMoveable
    {
        [SerializeField] protected ActionHandle actionHandle;

        public float speed = 5;

        public virtual void Initialized()
        {
        }

        public virtual void Move(Vector3 direction)
        {
        }
    }
}
