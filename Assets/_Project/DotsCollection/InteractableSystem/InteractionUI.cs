using TMPro;
using UnityEngine;

namespace DarkLordGame
{
    public class InteractionUI : MonoBehaviour
    {
        public RectTransform rootTransform;//update in the world pos
        public CanvasGroup canvasGroup; //for alpha
        public TextMeshProUGUI text;//for updating key
        public Vector3 worldOffset = new Vector3(0, 0, -0.5f);
        public Animator animator;
    }
}
