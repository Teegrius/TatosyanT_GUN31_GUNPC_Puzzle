using UnityEngine;

namespace SceneObjects
{
    public sealed class MainObject : SceneObjectAbstract
    {
        private TrailRenderer _trailRenderer;
        public bool Selected { get; private set; }
        
        private void OnMouseEnter() => Selected = true;

        private void OnMouseExit() => Selected = false;

        public override void Initialize()
        {
            base.Initialize();
            _trailRenderer = GetComponent<TrailRenderer>();
        }

        public void EnableTrail() => _trailRenderer.enabled = true;

        public void DisableTrail() => _trailRenderer.enabled = false;
    }
}