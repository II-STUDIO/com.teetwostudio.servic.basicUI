using UnityEngine;

namespace Services.UI
{
    public class Panel : MonoBehaviour,ITransitionHandle
    {
        [SerializeField] private GameObject root;
        [Space]
        [SerializeField] private GenericTransition transition;

        public GenericTransition Transition
        {
            get => transition;
        }

        public bool IsInitialized { get; private set; } = false;

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

            if (root)
                return;

            root = gameObject;
        }

        /// <summary>
        /// Enable or show this pnael.
        /// </summary>
        public virtual void Open()
        {
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
            if (root.activeSelf)
                Close();
            else
                Open();
        }

        private void Enable()
        {
            root.SetActive(true);
        }

        private void Disable()
        {
            root.SetActive(false);
        }

        #region Transition_Event
        public void OnFadeInBegin()
        {
            Enable();
        }

        public void OnFadeInComplete()
        {
            //Do Nothing
        }

        public void OnFadeOutBegin()
        {
            //Do Nothing
        }

        public void OnFadeOutComplete()
        {
            Disable();
        }
        #endregion
    }
}