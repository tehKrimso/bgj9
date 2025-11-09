using System.Collections;
using UnityEngine;

namespace Infrastructure
{
    public class LoadingCurtain : MonoBehaviour
    {
        public float FadeSpeed = 0.05f;
        public CanvasGroup Curtain;

        public void Show()
        {
            gameObject.SetActive(true);
            Curtain.alpha = 1;
        }

        public void Hide()
        {
            StartCoroutine(FadeIn());
        }
        
        private IEnumerator FadeIn()
        {
            while (Curtain.alpha > 0)
            {
                Curtain.alpha -= FadeSpeed;
                yield return new WaitForSeconds(FadeSpeed);
            }

            gameObject.SetActive(false);
        }
    }
}
