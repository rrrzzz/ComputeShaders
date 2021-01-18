using static UnityEngine.Mathf;

namespace Catlike
{
    public static class FunctionLib
    {
        public static float Wave(float x, float t) => Sin(PI * (x + t));

        public static float MultiWave(float x, float t)
        {
            var y = Sin(PI * (x + t));
            y += Sin(2 * PI * (x + t)) * 0.5f;

            return y * (2f / 3);
        }

        public static float Ripple(float x, float t)
        {
            var d = Abs(x);
            var y = Sin(PI * (4f * d - t) / (1f + 10f * d));
            return y;
        }
    }
}
