using static UnityEngine.Mathf;

namespace Catlike
{
    public static class FunctionLib
    {
        public static float Wave(float x, float t) => Sin(PI * (x + t));

        public static float MultiWave(float x, float t)
        {
            var y = Sin(PI * (x + t));
            y += Sin(2 * PI * (x + t)) * .5f;
            return y * (2f / 3);
        }
            

    }
}
