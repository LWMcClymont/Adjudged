using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
    [ExecuteInEditMode]
    [AddComponentMenu("Image Effects/Color Adjustments/Grayscale")]
    public class GrayscaleNick : ImageEffectBase {
        public Texture  textureRamp;

        [Range(0, 1)]
        public float bwIntensity = 1;

        // Called by camera to apply image effect
        void OnRenderImage (RenderTexture source, RenderTexture destination) {
            material.SetFloat("_bwBlend", bwIntensity);
            Graphics.Blit (source, destination, material);
        }
    }
}
