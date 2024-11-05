using System;
using System.Collections.Generic;

namespace Runtime
{
    [Serializable]
    public class BlueprintDB
    {
        public List<BlueprintModel> lstBlueprint;
    }
      
    [Serializable]
    public class BlueprintModel
    {
        public int idBlueprint;
        public bool isLock;
    }
}