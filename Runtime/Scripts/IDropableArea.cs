using UnityEngine;

namespace Services.UI
{
    public interface IDropableArea
    {
        public void OnDroped(UIDrageHandler drager);
    }
}