using UnityEngine;
using UnityEngine.UI;

namespace Services.UI
{
    public class ImageProcesser : MonoBehaviour
    {
        [System.Serializable]
        struct ProcessSlot
        {
            public Gradient gradient;
            public Image image;
            public float delay;

            private float _currentTargetFill;
            private float _reftFill;

            public void SetProcessFill(float fillAmountValue)
            {
                _currentTargetFill = fillAmountValue;

                if (delay > 0)
                    return;

                image.fillAmount = _currentTargetFill;
                image.color = gradient.Evaluate(image.fillAmount);
            }

            public void UpdateFill()
            {
                if (delay == 0)
                    return;

                image.fillAmount = Mathf.SmoothDamp(image.fillAmount, _currentTargetFill, ref _reftFill, delay);
                image.color = gradient.Evaluate(image.fillAmount);
            }
        }

        [SerializeField] private ProcessSlot _main;
        [SerializeField] private ProcessSlot _secondary;

        public void UpdateColorImage(float fillAmountValue)
        {
            _main.SetProcessFill(fillAmountValue);
            _secondary.SetProcessFill(fillAmountValue);
        }

        private void Update()
        {
            if (_main.image)
                _main.UpdateFill();

            if (_secondary.image)
                _secondary.UpdateFill();
        }
    }
}

