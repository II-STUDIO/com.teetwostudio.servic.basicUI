using UnityEngine;

namespace Services.UI
{

    public abstract class UIDragDropHadnler : UIDragHandler, IDropableArea
    {
        public abstract void OnTakeReleaseIn(UIDragHandler drager);
    }
}