using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    [Serializable]
    public class MissionDB
    {
        public int idLastMission;
        public List<MissionModel> lstMission;


    }


    [Serializable]
    public class MissionModel
    {
        public int idMission;
        public int idBlueprint;
        public int amountRequest;
    }
}