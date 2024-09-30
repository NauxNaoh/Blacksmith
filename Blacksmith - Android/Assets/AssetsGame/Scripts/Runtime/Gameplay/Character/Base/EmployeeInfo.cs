using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    [Serializable]
    public class EmployeeInfo
    {
        public EmployeeType employeeType;
        public int forgeSpeed;
        public int woodworkingSpeed;
        public int transportSpeed;
        public int carryWeight;

        public EmployeeInfo(int forgeSpeed = 0, int woodworkingSpeed = 0, int transportSpeed = 0, int carryWeight = 0)
        {
            this.forgeSpeed = forgeSpeed;
            this.woodworkingSpeed = woodworkingSpeed;
            this.transportSpeed = transportSpeed;
            this.carryWeight = carryWeight;
        }


        
    }
    
    public enum EmployeeType
    {
        Smith = 0,
        Transporter = 1,
    }
}