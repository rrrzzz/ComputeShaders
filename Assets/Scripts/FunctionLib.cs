using System;
using UnityEngine;

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
        p.y = Mathf.Sin(Mathf.PI * (u + v - t));
        p.z = v;
                                           
        return p;
    } 

    private static Vector3 MultiWave(float u, float v, float t)
    {
        Vector3 p;
            
        p.x = u;
          
        p.y = Mathf.Sin(Mathf.PI * (u + t));
        p.y += Mathf.Sin(2 * Mathf.PI * (v + t)) * 0.5f;
        p.y += Mathf.Sin(Mathf.PI * (u + v + 0.25f * t));
        p.y *= 2f / 3;
            
        p.z = v;
            
        return p;
    }

    private static Vector3 Ripple(float u, float v, float t)
    {
        Vector3 p;
            
        p.x = u;
            
        var d = Mathf.Sqrt(u * u + v * v);
        p.y  = Mathf.Sin(Mathf.PI * (4f * d - t)) / (1f + 10f * d);
            
        p.z = v;
            
        return p;
    }
        
    private static Vector3 Sphere(float u, float v, float t)
    {
        Vector3 p;
           
        var r = Mathf.Cos(v * Mathf.PI / 2);
        p.x = r * Mathf.Sin(Mathf.PI * u);
        p.y = Mathf.Sin(v * Mathf.PI / 2);
        p.z = r * Mathf.Cos(Mathf.PI * u);
            
        return p;
    }
        
    private static Vector3 Torus(float u, float v, float t)
    {
        Vector3 p;
        var rOuter = 0.7f + 0.1f * Mathf.Sin(Mathf.PI * (6f * u + 0.5f * t));
        var rWidth = 0.15f + 0.05f * Mathf.Sin(Mathf.PI * (8f * u + 4f * v + 2f * t));
        var s = rOuter + rWidth * Mathf.Cos(v * Mathf.PI);
        p.x = s * Mathf.Sin(Mathf.PI * u);
        p.y = rWidth * Mathf.Sin(v * Mathf.PI);
        p.z = s * Mathf.Cos(Mathf.PI * u);
            
        return p;
    }
        
    private static Vector3 TorusAwesome(float u, float v, float t)
    {
        Vector3 p;
        var r = Mathf.Cos(Mathf.PI * 2 * v + t);
        var s = 0.05f + 0.6f + r * Mathf.Cos(v * Mathf.PI);
        p.x = s * Mathf.Sin(Mathf.PI * u);
        p.y = r * Mathf.Sin(v * Mathf.PI);
        p.z = s * Mathf.Cos(Mathf.PI * u);
            
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