using System;
using System.Collections.Generic;

namespace Runtime
{
    [Serializable]
    public class BlueprintDB
    {
        public List<BlueprintModel> lstBlueprintInfo;
    }

    [Serializable]
    public class BlueprintModel
    {
        public int id;
        public bool isLock;
    }
}