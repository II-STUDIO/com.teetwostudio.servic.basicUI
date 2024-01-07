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
            get => fadeIn.Time;
        }

        public float FadeOutTime
        {
            get => fadeOut.Time;
        }

        public bool IsProcessing { get; private set; }

        public bool IsInitialized { get; private set; }

        public void Initialize(ITransitionHandle handle)
        {
            if (IsInitialized)
                return;

            _handle = handle;

            fadeIn.SetListener(OnFadeInStart, OnFadeInCompelte);
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
                return;

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
                return;

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