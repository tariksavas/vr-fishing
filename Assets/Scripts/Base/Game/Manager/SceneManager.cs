namespace Base.Game.Manager
{
    using Base.Game.BaseObject.XR;

    public class SceneManager : MyObject
    {
        protected override void Registration()
        {
            base.Registration();

            BaseHand.MenuButtonDown += OnMenuButtonDown;
        }

        protected override void UnRegistration()
        {
            base.UnRegistration();

            BaseHand.MenuButtonDown -= OnMenuButtonDown;
        }

        private void OnMenuButtonDown()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}