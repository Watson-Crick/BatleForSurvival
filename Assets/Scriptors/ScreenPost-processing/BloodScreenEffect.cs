using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodScreenEffect : PostEffectBase{

    public Shader BloodScrShader;
    private Material BloodScrMaterial;
    public Material material
    {
        get
        {
            BloodScrMaterial = CheckShaderAndCreateMaterial(BloodScrShader, BloodScrMaterial);
            return BloodScrMaterial;
        }
    }

    [Range(0, 1)]
    public float transparency;

    private Texture2D bloodTexture;

    protected override void Start()
    {
        base.Start();
        bloodTexture = Resources.Load<Texture2D>("Textures/Screen/bs");
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null)
        {
            material.SetTexture("_BloodTex", bloodTexture);
            material.SetFloat("_Transparency", transparency);
            Graphics.Blit(source, destination, material);
        }
        else
        {
            Graphics.Blit(source, destination);
        }
    }



}
