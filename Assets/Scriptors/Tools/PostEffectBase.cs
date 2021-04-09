using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostEffectBase : MonoBehaviour {

	protected virtual void Start () {
        CheckResources();
	}

    protected void CheckResources()
    {
        bool isSupport = CheckSupport();

        if (!isSupport)
        {
            NotSupport();
        }
    }

    protected bool CheckSupport()
    {
        if (SystemInfo.supportsImageEffects == false)
        {
            Debug.LogWarning("This platform does not support image effects");
            return false;
        }
        return true;
    }

    protected void NotSupport()
    {
        enabled = false;
    }
	
	protected Material CheckShaderAndCreateMaterial(Shader shader, Material material)
    {
        if (shader == null)
            return null;
        if (shader.isSupported && material != null && material.shader == shader)
            return material;
        if (shader.isSupported)
        {
            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;
            if (material)
            {
                return material;
            }else
            {
                return null;
            }
        }else
        {
            return null;
        }
    }
}
