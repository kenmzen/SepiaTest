using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class PostEffectTest : ScriptableRendererFeature
{
    private class Pass : ScriptableRenderPass
    {
        private readonly string tag = "Test Pass";
        private readonly Material _material;
        public Pass(Shader shader) : base()
        {
            _material = new Material(shader);
            renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
        }
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var target = renderingData.cameraData.renderer.cameraColorTarget;
            var buffer = CommandBufferPool.Get(tag);
            buffer.Blit(target, target, _material);
            context.ExecuteCommandBuffer(buffer);
        }
    }
    [SerializeField]
    private Shader _shader;
    private Pass _pass;
    public override void Create()
    {
        _pass = new Pass(_shader);
    }
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(_pass);
    }
}