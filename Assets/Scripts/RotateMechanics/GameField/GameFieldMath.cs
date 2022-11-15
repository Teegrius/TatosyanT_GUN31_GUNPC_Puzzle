using SceneObjects;
using UnityEngine;

namespace RotateMechanics.GameField
{
    public static class GameFieldMath
    {
        private static int CompareAndNormalize(float a, float b) => Mathf.Abs(a) > Mathf.Abs(b) ? Mathf.RoundToInt(a) : 0;
        public static Vector2 CalculateNewPosition(MainObject mainObject,float x, float y, float gridSize) => mainObject.Position + CalculatePositionOnGrid(x, y, gridSize);
        public static Vector2 CalculatePositionOnGrid(float x, float y, float gridSize) => new Vector2(CompareAndNormalize(x,y), CompareAndNormalize(y,x)) * gridSize;
    }
}