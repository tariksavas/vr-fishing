namespace Base.Game.BaseObject
{
    using Base.Game.BaseObject.XR;

    public interface IInteractable
    {
        public void OnTriggerEnterHand(BaseHand hand);
        public void OnTriggerExitHand(BaseHand hand);
    }
}