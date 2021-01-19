using System;
using static UnityEngine.Mathf;

namespace Catlike
{
    public static class FunctionLib
    {
        private static readonly Func<float, float, float, float>[] FuncTypes = 
            { Wave, MultiWave, Ripple };
        
        public static Func<float, float, float, float> GetFunction(FunctionType functionType) => 
            FuncTypes[(int) functionType];
        
        private static float Wave(float x, float z, float t) => Sin(PI * (x - t));

        private static float MultiWave(float x, float z, float t)
        {
            var y = Sin(PI * (x + t));
            y += Sin(2 * PI * (x + t)) * 0.5f;

            return y * (2f / 3);
        }

        private static float Ripple(float x, float z, float t)
        {
            var d = Abs(x);
            var y = Sin(PI * (4f * d - t)) / (1f + 10f * d);
            return y;
        }
    }
    
    public enum FunctionType
    {
        Wave,
        MultiWave,
        Ripple
    }
}
