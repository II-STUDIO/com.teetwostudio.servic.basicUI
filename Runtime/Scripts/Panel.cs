using UnityEngine;
using UnityEngine.Events;

namespace Services.UI
{
    public class Panel : MonoBehaviour,ITransitionHandle
    {
        [SerializeField] private GameObject root;
        public bool isDisabledOnClose = true;
        [Space]
        [SerializeField] private GenericTransition transition;

        public GenericTransition Transition
        {
            get => transition;
        }

        public bool IsInitialized { get; private set; } = false;

        public UnityEvent OnOpen { get; protected set; }
        public UnityEvent OnClose { get; protected set; }

        public bool IsOpened { get; private set; } = false;

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

            if (!transition.IsInitialized)
            {
                Enable();
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

            if (!transition.IsInitialized)
            {
                Disable();
                return;
            }

            transition.FadeOut();
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
            Enable();
        }

        public virtual void OnFadeInComplete()
        {
            //Do Nothing
        }

        public virtual void OnFadeOutBegin()
        {
            //Do Nothing
        }

        public virtual void OnFadeOutComplete()
        {
            Disable();
        }
        #endregion
    }
}