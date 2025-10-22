using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DarkLordGame
{
    public class FadeLayer : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        private void Awake()
        {
            ForceAlpha(1.0f);
        }

        public void ForceAlpha(float alpha)
        {
            canvasGroup.alpha = alpha;
            gameObject.SetActive(alpha > 0);
        }

        public IEnumerator Fade(float start, float target, float period = 2)
        {
            float t = 0;
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            while (t < period)
            {
                ForceAlpha(Mathf.Lerp(start, target, t));
                t += Time.unscaledDeltaTime;
                yield return null;
            }
            ForceAlpha(target);
            if (target <= 0.0f)
            {
                gameObject.SetActive(false);
            }
        }
    }
}