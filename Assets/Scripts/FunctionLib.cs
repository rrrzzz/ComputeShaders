using System;
using UnityEngine;
using static UnityEngine.Mathf;

namespace Catlike
{
    public static class FunctionLib
    {
        private static readonly Func<float, float, float, Vector3>[] FuncTypes = 
            { Wave, MultiWave, Ripple, Sphere, Torus, TorusAwesome };
        
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
        
        private static Vector3 Sphere(float u, float v, float t)
        {
            Vector3 p;
           
            var r = Cos(v * PI / 2);
            p.x = r * Sin(PI * u);
            p.y = Sin(v * PI / 2);
            p.z = r * Cos(PI * u);
            
            return p;
        }
        
        private static Vector3 Torus(float u, float v, float t)
        {
            Vector3 p;
            var rOuter = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t));
            var rWidth = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t));
            var s = rOuter + rWidth * Cos(v * PI);
            p.x = s * Sin(PI * u);
            p.y = rWidth * Sin(v * PI);
            p.z = s * Cos(PI * u);
            
            return p;
        }
        
        private static Vector3 TorusAwesome(float u, float v, float t)
        {
            Vector3 p;
            var r = Cos(PI * 2 * v + t);
            var s = 0.05f + 0.6f + r * Cos(v * PI);
            p.x = s * Sin(PI * u);
            p.y = r * Sin(v * PI);
            p.z = s * Cos(PI * u);
            
            return p;
        }
    }
    
    public enum FunctionType
    {
        Wave,
        MultiWave,
        Ripple, 
        Sphere,
        Torus, 
        TorusAwesome
    }
}
