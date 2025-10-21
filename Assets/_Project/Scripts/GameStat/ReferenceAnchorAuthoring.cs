using UnityEngine;

namespace DarkLordGame
{
    public class ReferenceAnchorAuthoring : ClassAuthorizer<ReferenceAnchor>
    {
        
    }
    
    public class ReferenceAnchorBaker : ClassBaker<ReferenceAnchorAuthoring, ReferenceAnchor>
    {
    }

    [System.Serializable]
    public class ReferenceAnchor : ClassComponentData
    {
        // public 
    }
}
