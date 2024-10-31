using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime
{
    public class MissionBoardHandle : MonoBehaviour
    {
        [SerializeField] private Mission prefabMission;
        [SerializeField] private RectTransform rectContent;


        public void Initialized()
        {

        }

        void SpawnMission()
        {




            var _mission = Instantiate<Mission>(prefabMission, rectContent);

        }

    }
}