using System.Collections.Generic;
using UnityEngine;
namespace DarkLordGame
{
    public class SettingUI : MonoBehaviour
    {
        public UI3DToggleGroup group;
        public List<UI3DFlagToggle> languageFlag;

        public UI3DSlider bgm;
        public UI3DSlider soundFX;
        public UI3D finishButton;
    }
}