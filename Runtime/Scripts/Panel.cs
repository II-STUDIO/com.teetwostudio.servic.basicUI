using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Services.UI
{
    public class Panel : MonoBehaviour, ITransitionHandle
    {
        [Header("Panel Settings")]
        public bool autoClearAction = true;
        public bool isDisabledOnClose = true;

        [SerializeField] private GameObject root;
        [SerializeField] private PanelTransition transition;

        public PanelTransition Transition => transition;

        public bool IsInitialized { get; private set; }
        public bool IsOpened { get; private set; }

        public Action OnOpen { get; set; }
        public Action OnClose { get; set; }

        private UniTaskCompletionSource? utcsAction;

        private GameObject Root => root != null ? root : (root = gameObject);

        protected virtual void Start()
        {
            Initialize();
        }

        public virtual void Initialize()
        {
            if (IsInitialized) return;

            transition.Initialize(this);
            IsInitialized = true;
        }

        public virtual void Open()
        {
            if (IsOpened) return;

            Initialize();

            if (!transition.IsInitialized || !transition.IsValidToFadeIn)
            {
                OnFadeInBegin();
                OnFadeInComplete();
                return;
            }

            transition.FadeIn();
        }

        public virtual void Close()
        {
            if (!IsOpened) return;

            Initialize();

            if (!transition.IsInitialized || !transition.IsValidToFadeOut)
            {
                OnFadeOutBegin();
                OnFadeOutComplete();
                return;
            }

            transition.FadeOut();
        }

        public async virtual UniTask OpenAsync()
        {
            if (IsOpened) return;

            utcsAction = new UniTaskCompletionSource();
            Open();

            await utcsAction.Task;
        }

        public async virtual UniTask CloseAsync()
        {
            if (!IsOpened) return;

            utcsAction = new UniTaskCompletionSource();
            Close();

            await utcsAction.Task;
        }

        public void Toggle()
        {
            if (IsOpened)
                Close();
            else
                Open();
        }

        private void Enable()
        {
            IsOpened = true;
            Root.SetActive(true);
            OnOpen?.Invoke();
        }

        private void Disable()
        {
            IsOpened = false;
            if (isDisabledOnClose)
                Root.SetActive(false);

            OnClose?.Invoke();
        }

        #region Transition Event Hooks

        public virtual void OnFadeInBegin()
        {
            StartAction();
            Enable();
        }

        public virtual void OnFadeInComplete()
        {
            EndAction();
        }

        public virtual void OnFadeOutBegin()
        {
            StartAction();
        }

        public virtual void OnFadeOutComplete()
        {
            Disable();
            EndAction();
        }

        private void StartAction()
        {
            utcsAction?.TrySetCanceled(); // Cancel previous action if overlapping
            utcsAction ??= new UniTaskCompletionSource();
        }

        private void EndAction()
        {
            utcsAction?.TrySetResult();
            utcsAction = null;
        }

        public void ClearAction()
        {
            utcsAction?.TrySetCanceled();
            utcsAction = null;
        }

        private void OnDestroy()
        {
            if (Application.isPlaying && autoClearAction)
                ClearAction();
        }

        #endregion
    }
}
