using TMPro;
using UnityEngine;

namespace DarkLordGame
{
    public class InteractionUI : MonoBehaviour
    {
        public RectTransform rootTransform;//update in the world pos
        public CanvasGroup canvasGroup; //for alpha
        public TextMeshProUGUI text;//for updating key
    }
}
