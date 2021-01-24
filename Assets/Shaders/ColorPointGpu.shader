Shader "Graph/ColorPointGpu"
{
    SubShader
    {
        CGPROGRAM

        #pragma target 4.5

        #pragma surface ConfigureSurface Standard fullforwardshadows addshadow
        #pragma instancing_options procedural:ConfigureProcedural
        
        #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
        StructuredBuffer<float3> _Positions;
        #endif

        struct Input
        {
			float3 worldPos;
		};
        
        void ConfigureProcedural ()
        {
            #if defined(UNITY_PROCEDURAL_INSTANCING_ENABLED)
            float3 position = _Positions[unity_InstanceId];
            #endif
        }

        void ConfigureSurface (Input input, inout SurfaceOutputStandard surface)
        {
			surface.Albedo = saturate(input.worldPos * 0.5 + 0.5);
			surface.Smoothness = 1.0;
		}
        ENDCG
    }
    
    Fallback "Diffuse"
}
