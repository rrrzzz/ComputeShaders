using System;
using UnityEngine;
using static UnityEngine.Mathf;

namespace Catlike
{
    public static class FunctionLib
    {
        private static readonly Func<float, float, float, Vector3>[] FuncTypes = 
            { Wave, MultiWave, Ripple };
        
        public static Func<float, float, float, Vector3> GetFunction(FunctionType functionType) => 
            FuncTypes[(int) functionType];
        
        private static Vector3 Wave(float u, float v, float t)
        {
            Vector3 p;
            
            p.x = u;
            p.y = Sin(PI * (u + v - t));
            p.z = v;

            return p;
        } 

        private static Vector3 MultiWave(float u, float v, float t)
        {
            Vector3 p;
            
            p.x = u;
          
            p.y = Sin(PI * (u + t));
            p.y += Sin(2 * PI * (v + t)) * 0.5f;
            p.y += Sin(PI * (u + v + 0.25f * t));
            p.y *= 2f / 3;
            
            p.z = v;
            
            return p;
        }

        private static Vector3 Ripple(float u, float v, float t)
        {
            Vector3 p;
            
            p.x = u;
            
            var d = Sqrt(u * u + v * v);
            p.y  = Sin(PI * (4f * d - t)) / (1f + 10f * d);
            
            p.z = v;
            
            return p;
        }
    }
    
    public enum FunctionType
    {
        Wave,
        MultiWave,
        Ripple
    }
}
