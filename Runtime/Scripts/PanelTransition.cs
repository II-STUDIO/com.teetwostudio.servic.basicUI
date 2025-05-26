using UnityEngine;

namespace Services.UI
{
    [System.Serializable]
    public class PanelTransition
    {
        [SerializeField] private BaseTransitionSlot fadeIn;
        [SerializeField] private BaseTransitionSlot fadeOut;

        private ITransitionHandle _handle;

        public float FadeInTime
        {
            get
            {
                if (fadeIn == null)
                    return 0f;

                return fadeIn.Time;
            }
        }

        public float FadeOutTime
        {
            get
            {
                if (fadeOut == null)
                    return 0f;

                return fadeOut.Time;
            }
        }

        public bool IsProcessing { get; private set; }

        public bool IsInitialized { get; private set; }

        public bool IsValidToFadeIn
        {
            get => fadeIn;
        }

        public bool IsValidToFadeOut
        {
            get => fadeOut;
        }

        public void Initialize(ITransitionHandle handle)
        {
            if (IsInitialized)
                return;

            _handle = handle;

            if (fadeIn)
                fadeIn.SetListener(OnFadeInStart, OnFadeInCompelte);
            if (fadeOut)
                fadeOut.SetListener(OnFadeOutStart, OnFadeOutComplete);

            IsInitialized = true;
        }


        public void FadeIn()
        {
            if (!IsInitialized)
            {
                Debug.LogWarningFormat("Panel trasition need to initialize first");
                return;
            }

            if (IsProcessing)
            {
                if (fadeIn.IsProcessing)
                    return;

                if (fadeOut.IsProcessing)
                    fadeOut.ForceCompleted();
            }

            if (fadeIn)
                fadeIn.Begin();
        }

        public void FadeOut()
        {
            if (!IsInitialized)
            {
                Debug.LogWarningFormat("Panel trasition need to initialize first");
                return;
            }

            if (IsProcessing)
            {
                if (fadeOut.IsProcessing)
                    return;

                if (fadeIn.IsProcessing)
                    fadeIn.ForceCompleted();
            }

            if (fadeOut)
                fadeOut.Begin();
        }

        #region FadeIn Event
        private void OnFadeInStart()
        {
            IsProcessing = true;

            if (_handle != null)
                _handle.OnFadeInBegin();
        }

        private void OnFadeInCompelte()
        {
            IsProcessing = false;

            if (_handle != null)
                _handle.OnFadeInComplete();
        }

        #endregion

        #region FadeOut Event
        private void OnFadeOutStart()
        {
            IsProcessing = true;

            if (_handle != null)
                _handle.OnFadeOutBegin();
        }

        private void OnFadeOutComplete()
        {
            IsProcessing = false;

            if (_handle != null)
                _handle.OnFadeOutComplete();
        }
        #endregion
    }
}