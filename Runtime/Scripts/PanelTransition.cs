using UnityEngine;

namespace Services.UI
{
    [System.Serializable]
    public class PanelTransition
    {
        [SerializeField] private BaseTransitionSlot fadeIn;
        [SerializeField] private BaseTransitionSlot fadeOut;

        private ITransitionHandle _handle;

        public float FadeInTime => fadeIn ? fadeIn.Time : 0f;
        public float FadeOutTime => fadeOut ? fadeOut.Time : 0f;

        public bool IsProcessing { get; private set; }
        public bool IsInitialized { get; private set; }

        public bool IsValidToFadeIn => fadeIn != null;
        public bool IsValidToFadeOut => fadeOut != null;

        public void Initialize(ITransitionHandle handle)
        {
            if (IsInitialized) return;

            _handle = handle;

            fadeIn?.SetListener(OnFadeInStart, OnFadeInComplete);
            fadeOut?.SetListener(OnFadeOutStart, OnFadeOutComplete);

            IsInitialized = true;
        }

        public void FadeIn()
        {
            if (!IsInitialized)
            {
                Debug.LogWarning("PanelTransition must be initialized before FadeIn.");
                return;
            }

            if (IsProcessing && fadeOut?.IsProcessing == true)
                fadeOut.ForceCompleted();

            fadeIn?.Begin();
        }

        public void FadeOut()
        {
            if (!IsInitialized)
            {
                Debug.LogWarning("PanelTransition must be initialized before FadeOut.");
                return;
            }

            if (IsProcessing && fadeIn?.IsProcessing == true)
                fadeIn.ForceCompleted();

            fadeOut?.Begin();
        }

        private void OnFadeInStart()
        {
            IsProcessing = true;
            _handle?.OnFadeInBegin();
        }

        private void OnFadeInComplete()
        {
            IsProcessing = false;
            _handle?.OnFadeInComplete();
        }

        private void OnFadeOutStart()
        {
            IsProcessing = true;
            _handle?.OnFadeOutBegin();
        }

        private void OnFadeOutComplete()
        {
            IsProcessing = false;
            _handle?.OnFadeOutComplete();
        }
    }
}
