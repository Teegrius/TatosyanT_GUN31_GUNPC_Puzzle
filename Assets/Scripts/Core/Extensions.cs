using UnityEngine;

namespace Core
{
    public static class Extensions
    {
        public static bool IsBetweenRange(this float thisValue, float value1, float value2) 
            => thisValue >= Mathf.Min(value1, value2) && thisValue <= Mathf.Max(value1, value2);
    }
}