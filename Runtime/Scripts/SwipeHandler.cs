using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

namespace Services.UI
{
    [RequireComponent(typeof(Image))]
    public class SwipeHandler : MonoBehaviour, IBeginDragHandler,IDragHandler, IEndDragHandler
    {
        [SerializeField] private float minDetectDistance = 22f;
        [SerializeField] private float maxDetectTime = 0.4f;
        [Header("Event")]
        [SerializeField] private UnityEvent onSwipeLeft;
        [SerializeField] private UnityEvent onSwipeRight;
        [SerializeField] private UnityEvent onSwipeUp;
        [SerializeField] private UnityEvent onSwipeDown;

        public UnityEvent OnSwipeLeft => onSwipeLeft;
        public UnityEvent OnSwipeRight => onSwipeRight;
        public UnityEvent OnSwipeUp => onSwipeUp;
        public UnityEvent OnSwipeDown => onSwipeDown;

        private Vector2 beginPosition;
        private Vector2 endPosition;
        private CountDown detectTimeCountdown;

        private bool isValide = false;

        private void Awake()
        {
            detectTimeCountdown.onComplete = CancelEvent;
        }
        private void Update()
        {
            detectTimeCountdown.Update(SystemTime.DeltaTime);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            isValide = true;

            beginPosition = eventData.position;

            detectTimeCountdown.Start(maxDetectTime);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (!isValide)
                return;

            endPosition = eventData.position;

            float distance = Vector2.Distance(beginPosition, endPosition);

            if (distance < minDetectDistance)
                return;

            CompleteEvent();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!isValide)
                return;

            endPosition = eventData.position;

            float distance = Vector2.Distance(beginPosition, endPosition);

            if (distance < minDetectDistance)
                return;

            CompleteEvent();
        }

        private void CancelEvent()
        {
            isValide = false;

            detectTimeCountdown.Clear();
        }

        private void CompleteEvent()
        {
            isValide = false;

            detectTimeCountdown.Clear();

            CalculteInput();
        }

        private void CalculteInput()
        {
            Vector2 beginY = beginPosition * Vector2.up;
            Vector2 endY = endPosition * Vector2.up;

            Vector2 beginX = beginPosition * Vector2.right;
            Vector2 endX = endPosition * Vector2.right;

            float yDistance = Vector2.Distance(beginY, endY);
            float xDistance = Vector2.Distance(beginX, endX);

            if(xDistance >= yDistance)
            {
                ExecuteEvent(beginX.x, endX.x, SwipeRight, SwipeLeft);
            }
            else
            {
                ExecuteEvent(beginY.y, endY.y, SwipeUp, SwipeDown);
            }
        }

        private void ExecuteEvent(float begin, float end, Action onEndGeater, Action onBeginGeater)
        {
            if (end > begin)
                onEndGeater();
            else
                onBeginGeater();
        }

        private void SwipeLeft()
        {
            onSwipeLeft.Invoke();
        }

        private void SwipeRight()
        {
            onSwipeRight.Invoke();
        }

        private void SwipeUp()
        {
            onSwipeUp.Invoke();
        }

        private void SwipeDown()
        {
            onSwipeDown.Invoke();
        }
    }
}