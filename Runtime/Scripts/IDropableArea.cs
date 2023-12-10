using UnityEngine;

namespace Services.UI
{
    public interface IDropableArea
    {
        [Tooltip("Invoke whene dragable object release drage state on this UI object")]
        public void OnTakeReleaseIn(UIDragHandler drager);
    }
}