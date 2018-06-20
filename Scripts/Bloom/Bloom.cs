//
// Kino/Bloom v2 - Bloom filter for Unity
//
// Copyright (C) 2015, 2016 Keijiro Takahashi
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
using UnityEngine;

namespace Kino
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Kino Image Effects/Bloom")]
    public class Bloom : MonoBehaviour
    {
        #region Public Properties

        /// Prefilter threshold (gamma-encoded)
        /// Filters out pixels under this level of brightness.
        public float thresholdGamma
        {
            get { return Mathf.Max(_threshold, 0); }
            set { _threshold = value; }
        }

        /// Prefilter threshold (linearly-encoded)
        /// Filters out pixels under this level of brightness.
        public float thresholdLinear
        {
            get { return GammaToLinear(thresholdGamma); }
            set { _threshold = LinearToGamma(value); }
        }

        [SerializeField]
        [Tooltip("Filters out pixels under this level of brightness.")]
        float _threshold = 0.8f;

        /// Soft-knee coefficient
        /// Makes transition between under/over-threshold gradual.

        [SerializeField, Range(0, 1)]
        [Tooltip("Makes transition between under/over-threshold gradual.")]
        float _softKnee = 0.338f;


        [SerializeField, Range(1, 7)]
        [Tooltip("Changes extent of veiling effects\n" +
                 "in a screen resolution-independent fashion.")]
        public float radius = 2.5f;

        /// Bloom intensity
        /// Blend factor of the result image.
        public float intensity
        {
            get { return Mathf.Max(_intensity, 0); }
            set { _intensity = value; }
        }

        [SerializeField]
        [Tooltip("Blend factor of the result image.")]
        float _intensity = 0.8f;

        int iterations;


        #endregion

        #region Private Members

        [SerializeField, HideInInspector]
        Shader _shader;

        Material _material;

        const int kMaxIterations = 16;
        RenderTexture[] _blurBuffer1 = new RenderTexture[kMaxIterations];
        RenderTexture[] _blurBuffer2 = new RenderTexture[kMaxIterations];

        float LinearToGamma(float x)
        {
#if UNITY_5_3_OR_NEWER
            return Mathf.LinearToGammaSpace(x);
#else
            if (x <= 0.0031308f)
                return 12.92f * x;
            else
                return 1.055f * Mathf.Pow(x, 1 / 2.4f) - 0.055f;
#endif
        }

        float GammaToLinear(float x)
        {
#if UNITY_5_3_OR_NEWER
            return Mathf.GammaToLinearSpace(x);
#else
            if (x <= 0.04045f)
                return x / 12.92f;
            else
                return Mathf.Pow((x + 0.055f) / 1.055f, 2.4f);
#endif
        }

        #endregion

        #region MonoBehaviour Functions

        void OnEnable()
        {
            _material = new Material(Shader.Find("Bloom"));
            _material.hideFlags = HideFlags.DontSave;

            var lthresh = thresholdLinear;
            _material.SetFloat("_Threshold", lthresh);
            var knee = lthresh * _softKnee + 1e-5f;
            var curve = new Vector3(lthresh - knee, knee * 2, 0.25f / knee);
            _material.SetVector("_Curve", curve);
            _material.SetFloat("_PrefilterOffs", 0.0f);
            UpdateParam();

        }

        void OnDisable()
        {
            DestroyImmediate(_material);
        }

        public void UpdateParam()
        {
            _material.SetFloat("_Intensity", intensity);
            RenderTexture rt= RenderTexture.GetTemporary(Screen.width, Screen.height, 24, RenderTextureFormat.R8);
            var logh = Mathf.Log(rt.height, 2) + radius - 8;
            var logh_i = (int)logh;
            _material.SetFloat("_SampleScale", 0.5f + logh - logh_i);

            iterations = Mathf.Clamp(logh_i, 1, kMaxIterations);
            RenderTexture.ReleaseTemporary(rt);

        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            var useRGBM = Application.isMobilePlatform;

            // source texture size
            var tw = source.width;
            var th = source.height;

            // blur buffer format
            var rtFormat = useRGBM ?
                RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR;


            var prefiltered = RenderTexture.GetTemporary(tw, th, 0, rtFormat);
            var pass = 0;
            Graphics.Blit(source, prefiltered, _material, pass);

            // construct a mip pyramid
            var last = prefiltered;
            for (var level = 0; level < iterations; level++)
            {
                _blurBuffer1[level] = RenderTexture.GetTemporary(
                    last.width / 2, last.height / 2, 0, rtFormat
                );

                pass = (level == 0) ? 2 : 4;
                Graphics.Blit(last, _blurBuffer1[level], _material, pass);

                last = _blurBuffer1[level];
            }

            // upsample and combine loop
            for (var level = iterations - 2; level >= 0; level--)
            {
                var basetex = _blurBuffer1[level];
                _material.SetTexture("_BaseTex", basetex);

                _blurBuffer2[level] = RenderTexture.GetTemporary(
                    basetex.width, basetex.height, 0, rtFormat
                );

                pass = 5;
                Graphics.Blit(last, _blurBuffer2[level], _material, pass);
                last = _blurBuffer2[level];
            }

            // finish process
            _material.SetTexture("_BaseTex", source);
            pass = 8;
            Graphics.Blit(last, destination, _material, pass);

            // release the temporary buffers
            for (var i = 0; i < kMaxIterations; i++)
            {
                if (_blurBuffer1[i] != null)
                    RenderTexture.ReleaseTemporary(_blurBuffer1[i]);

                if (_blurBuffer2[i] != null)
                    RenderTexture.ReleaseTemporary(_blurBuffer2[i]);

                _blurBuffer1[i] = null;
                _blurBuffer2[i] = null;
            }

            RenderTexture.ReleaseTemporary(prefiltered);
        }

        #endregion
    }
}
