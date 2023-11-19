using UnityEngine;

namespace Services.UI
{
    public interface IDropableArea
    {
        public void OnTakeReleaseIn(UIDragHandler drager);
    }
}