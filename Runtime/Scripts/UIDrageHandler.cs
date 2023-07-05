using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Services.UI
{
    public abstract class UIDrageHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private bool useMousePosition = false;
        [Tag]
        [SerializeField] private string targetTag = "Untagged";
        [Required("Request canvas to use manual set inspector or code for set this property")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform target;
        [SerializeField] private RectTransform visiblePerent;
        [SerializeField] private DropPointAction dropPointAction = DropPointAction.ReturnToOriginAtStart;

        private RectTransform originalParent;

        private Vector3 originPoint;

        private float CanvasScaleFacor
        {
            get
            {
                if (canvas)
                    return canvas.scaleFactor;

                return 1f;
            }
        }

        public Canvas OwnerCanvas
        {
            get => canvas;
            set => canvas = value;
        }

        public string TargetTag
        {
            get => targetTag;
            set => targetTag = value;
        }

        public RectTransform DrageTarget
        {
            get => target;
            set => target = value;
        }

        public RectTransform VisiblePerent
        {
            get => visiblePerent;
            set => visiblePerent = value;
        }

        public DropPointAction DropAction
        {
            get => dropPointAction;
            set => dropPointAction = value;
        }

        public bool UseMosuePosition
        {
            get => useMousePosition;
            set => useMousePosition = value;
        }


        protected virtual void Start()
        {
            if (!target)
                target = (RectTransform)transform;

            originalParent = (RectTransform)target.parent;

            originPoint = target.localPosition;
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            SetBeginPoint();
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (useMousePosition)
                target.position = Input.mousePosition;
            else
                target.anchoredPosition += eventData.delta / CanvasScaleFacor;
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            GameObject dropOnObject = eventData.pointerCurrentRaycast.gameObject;

            CheckDropable(dropOnObject);

            SetEndPoint();
        }

        private void CheckDropable(GameObject dropOnObject)
        {
            if (!dropOnObject)
                return;

            if (!targetTag.IsNullOrEmpty() && dropOnObject.tag != targetTag)
                return;

            IDropableArea dropArea = dropOnObject.GetComponent<IDropableArea>();

            if (dropArea == null)
                return;

            dropArea.OnDroped(this);

            OnDropedOnDropableArea(dropArea);
        }

        protected virtual void OnDropedOnDropableArea(IDropableArea dropArea)
        {

        }

        private void SetBeginPoint()
        {
            if (dropPointAction == DropPointAction.ReturnToOriginAtBegin)
                originPoint = target.localPosition;

            if (visiblePerent)
                target.SetParent(visiblePerent);
        }

        private void SetEndPoint()
        {
            if (dropPointAction == DropPointAction.None)
                return;

            if (visiblePerent)
                target.SetParent(originalParent);

            target.localPosition = originPoint;
        }
    }

    public enum DropPointAction
    {
        None,
        ReturnToOriginAtStart,
        ReturnToOriginAtBegin
    }
}