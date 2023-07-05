namespace Services.UI
{

    public abstract class UIDrageAndDropHadnler : UIDrageHandler, IDropableArea
    {
        public virtual void OnDroped(UIDrageHandler drager)
        {

        }
    }
}