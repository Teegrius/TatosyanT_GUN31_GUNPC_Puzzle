using Core;

namespace SceneObjects
{
    public sealed class MovingZone : SceneObjectAbstract, IInitializable<MovePoint[]>
    {
        private MovePoint[] _pointsInsideZone;

        public override bool IsOnSamePosition(SceneObjectAbstract objectAbstract)
        {
            for (int i = 0; i < _pointsInsideZone.Length; i++)
            {
                if (_pointsInsideZone[i].IsOnSamePosition(objectAbstract))
                {
                    return true;
                }
            }
            
            return base.IsOnSamePosition(objectAbstract);
        }

        public void Initialize(MovePoint[] type)
        {
            throw new System.NotImplementedException();
        }
    }
}