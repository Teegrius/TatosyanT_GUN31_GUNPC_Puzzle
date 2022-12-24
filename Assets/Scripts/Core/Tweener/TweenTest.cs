using UnityEngine;

namespace Core.Tweener
{
    public class TweenTest : MonoBehaviour
    {
        public GameObject Test1;
        public GameObject Test2;
        private ITweener _testTweener;

        private void Start()
        {
            _testTweener = TweenFactory
                .CreateParallel(TweenFactory.Scale2D(Test1, Vector3.one * 0.3f, 2), TweenFactory.Move2D(Test1, Test1.transform.position + Vector3.right * 5,10))
                .OnStart(() => Debug.LogError("1"))
                .OnFinish(() => Debug.LogError("2"));
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                _testTweener.Stop();
            }
            
            if (Input.GetKeyDown(KeyCode.T))
            {
                _testTweener.Play();
            }
        }
    }
}