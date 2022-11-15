namespace SceneObjects
{
    public sealed class MainObject : SceneObjectAbstract
    {
        public bool Selected { get; private set; }
        
        private void OnMouseEnter() => Selected = true;

        private void OnMouseExit() => Selected = false;
    }
}