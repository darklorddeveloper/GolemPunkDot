
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DarkLordGame
{

    public class UI3DToggle : UI3D
    {
        public Action<UI3DToggle> onValueChanged;
        protected bool m_Toggle;
        public bool isOn
        {
            get { return m_Toggle; }
            set
            {
                SetToggle(value);
            }
        }
        public void Toggle()
        {
            SetToggle(!m_Toggle);
        }


        public void SetToggle(bool value)
        {
            SetToggleWithoutNotify(value);
            onValueChanged?.Invoke(this);
        }

        public virtual void SetToggleWithoutNotify(bool on)
        {
            m_Toggle = on;
        }
    }

    public class UI3DToggleGroup : MonoBehaviour
    {
        public List<UI3DToggle> toggles = new List<UI3DToggle>();
        public Action<int> onToggleSelected;

        void Start()
        {
            for (int i = 0, length = toggles.Count; i < length; i++)
            {
                toggles[i].onValueChanged += OnValueChanged;
            }
        }

        public void SetSelectedWithoutNotify(int index)
        {
            for (int i = 0, length = toggles.Count; i < length; i++)
            {
                toggles[i].SetToggleWithoutNotify(i == index);
            }

        }

        private void OnValueChanged(UI3DToggle toggle)
        {
            int index = -1;
            for (int i = 0, length = toggles.Count; i < length; i++)
            {
                if (toggles[i] == toggle)
                {
                    index = i;
                    continue;
                }
                toggles[i].SetToggleWithoutNotify(false);
            }

            if (index < 0)
            {
                return;
            }

            onToggleSelected?.Invoke(index);
        }
    }
}
