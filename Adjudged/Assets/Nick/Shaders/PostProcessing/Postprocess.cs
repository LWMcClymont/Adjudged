using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Postprocess : MonoBehaviour 
{
	private Material PostprocessMaterial;
	private Material SimpleRender;

    [HideInInspector]
	public RenderTexture CameraRenderTexture;
    [HideInInspector]
    public RenderTexture Buffer;

    [Range(0, 1)]
    public float bwIntensity;

    public void Start()
	{

        PostprocessMaterial = new Material(Shader.Find("Nick/Grayscale"));
        SimpleRender = new Material(Shader.Find("Custom/simple_render"));

        CameraRenderTexture = new RenderTexture (Screen.width, Screen.height, 24);
		Buffer = new RenderTexture (Screen.width, Screen.height, 24);

		Camera.main.targetTexture = CameraRenderTexture;
	}

	void OnPostRender()
	{
        Graphics.SetRenderTarget(Buffer);
		GL.Clear (true, true, Color.black);

        PostprocessMaterial.SetFloat("_bwBlend", bwIntensity);

        Graphics.SetRenderTarget (Buffer.colorBuffer, CameraRenderTexture.depthBuffer);
		Graphics.Blit(CameraRenderTexture, SimpleRender);
		Graphics.Blit(CameraRenderTexture, PostprocessMaterial);
		
		RenderTexture.active = null;
		Graphics.Blit(Buffer, SimpleRender);
	}
}
