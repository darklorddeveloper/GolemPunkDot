using Unity.Entities;
using UnityEngine;

namespace DarkLordGame
{
    public class PhaseesAuthoring : MonoBehaviour
    {
        public CurrentPhase currentPhase;
        public BootStrapPhase bootStrapPhase;
        public IntroPhase introPhase;
        public GamePhase gamePhase;
        public GamePhaseIntro gamePhaseIntro;
        public SettingPhase settingPhase;
        public MinePhase minePhase;

        public class Baker : Baker<PhaseesAuthoring>
        {
            public override void Bake(PhaseesAuthoring authoring)
            {
                var e = GetEntity(TransformUsageFlags.None);
                AddComponent(e, authoring.currentPhase);
                AddComponent(e, authoring.bootStrapPhase);
                AddComponent(e, authoring.introPhase);
                AddComponent(e, authoring.gamePhase);
                AddComponent(e, authoring.gamePhaseIntro);
                AddComponent(e, authoring.minePhase);
                AddComponent(e, authoring.settingPhase);

            }
        }
    }


    
}
