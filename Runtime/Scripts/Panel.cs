using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Services.UI
{
    public class Panel : MonoBehaviour, ITransitionHandle
    {
        public bool autoClearAction = true;
        public bool isDisabledOnClose = true;
        [SerializeField] private GameObject root;
        [Space]
        [SerializeField] private PanelTransition transition;

        public PanelTransition Transition
        {
            get => transition;
        }

        public bool IsInitialized { get; private set; } = false;

        public Action OnOpen { get; set; }
        public Action OnClose { get; set; }

        public bool IsOpened { get; private set; } = false;

        private UniTaskCompletionSource utcsAction;

        private GameObject Root
        {
            get
            {
                if (root)
                    return root;

                root = gameObject;
                return root;
            }
        }

        /// <summary>
        /// Intialize this panel and transition (transition can initable when property 'animation' is not null or empty only).
        /// </summary>
        protected virtual void Start()
        {
            Initialize();
        }

        public virtual void Initialize()
        {
            if (IsInitialized)
                return;

            transition.Initialize(this);

            IsInitialized = true;
        }

        /// <summary>
        /// Enable or show this pnael.
        /// </summary>
        public virtual void Open()
        {
            Initialize();

            if (!transition.IsInitialized || !transition.IsValideToFadeIn)
            {
                OnFadeInBegin();
                OnFadeInComplete();
                return;
            }

            transition.FadeIn();
        }

        /// <summary>
        /// Disable or hide this panel.
        /// </summary>
        public virtual void Close()
        {
            Initialize();

            if (!transition.IsInitialized || !transition.IsValideToFadeOut)
            {
                OnFadeOutBegin();
                OnFadeOutComplete();
                return;
            }

            transition.FadeOut();
        }

        public async virtual UniTask OpenAsync()
        {
            Open();

            if (utcsAction == null)
                return;

            await utcsAction.Task;
        }


        public async virtual UniTask CloseAsync()
        {
            Close();

            if (utcsAction == null)
                return;

            await utcsAction.Task;
        }

        /// <summary>
        /// Nagative switch open or close when invoke.
        /// </summary>
        public void ShowNagative()
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

        #region Transition_Event
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
            if (utcsAction != null)
                utcsAction.TrySetCanceled();

            utcsAction = new UniTaskCompletionSource();
        }

        private void EndAction()
        {
            if (utcsAction == null)
                return;

            utcsAction.TrySetResult();
            utcsAction = null;
        }

        public void ClearAction()
        {
            if (utcsAction == null)
                return;

            utcsAction.TrySetCanceled();
            utcsAction = null;
        }

        private void OnDestroy()
        {
            if (!autoClearAction)
                return;

            if (!Application.isPlaying)
                return;

            ClearAction();
        }
        #endregion
    }
}