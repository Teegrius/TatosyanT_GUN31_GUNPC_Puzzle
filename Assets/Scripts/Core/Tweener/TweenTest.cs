using System;
using UnityEngine;

namespace Core.Tweener
{
    public class TweenTest : MonoBehaviour
    {
        public GameObject Test;
        private ITweener _rotateLeft;

        private void Start()
        {
            // _rotateLeft = TweenFactory.RotateAround(_rotate, 90, Vector3.forward, 1)
            //     .OnStart((() => Debug.LogError("Start Rotating 1")))
            //     .OnFinish((() => Debug.LogError("StopRotation 1")))
            //     .ThenScale(_rotate, Vector2.one * 0.2f, 1)
            //     .ThenScale(_rotate, Vector2.one * 2, 1);
            _rotateLeft = TweenFactory.Scale2D(Test, Vector2.one * 0.1f, 10);
            _rotateLeft.Play();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _rotateLeft.Stop();
            }
            // if (Input.GetKeyDown(KeyCode.T))
            // {
            //     _rotateRight.Play();
            // }
        }
    }
}