
using UnityEngine;

namespace Kino
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Kino Image Effects/Binary")]
    public class Binary : MonoBehaviour
    {
        #region Editable attributes

       
        // Scale factor of dither pattan
        [SerializeField, Range(1, 8)] int _ditherScale = 1;


        // Color (dark)
        [SerializeField] Color _color0 = Color.black;

        public Color color0
        {
            get { return _color0; }
            set { _color0 = value; }
        }

        // Color (light)
        [SerializeField] Color _color1 = Color.white;

        public Color color1
        {
            get { return _color1; }
            set { _color1 = value; }
        }

        // Opacity
        [SerializeField, Range(0, 1)] float _opacity = 1.0f;

        public float Opacity
        {
            get { return _opacity; }
            set { _opacity = value; }
        }

        #endregion

        #region Private resources

        [SerializeField, HideInInspector] Shader _shader;

        [SerializeField, HideInInspector] Texture2D _bayer2x2Texture;

        Texture2D DitherTexture
        {
            get
            {
                if (_bayer2x2Texture == null)
                {
                    _bayer2x2Texture = Resources.Load<Texture2D>("BN");
                }
                return _bayer2x2Texture;
            }
        }

        Material _material;

        #endregion

        #region MonoBehaviour implementation

        void OnDestroy()
        {
            if (_material != null)
                if (Application.isPlaying)
                    Destroy(_material);
                else
                    DestroyImmediate(_material);
        }

        private void Awake()
        {

            if (_material == null)
            {
                _material = new Material(Shader.Find("Binary"));
                _material.hideFlags = HideFlags.DontSave;
            }
            _material.SetTexture("_DitherTex", DitherTexture);

            _material.SetFloat("_Scale", _ditherScale);
        }

        public void updateColor()
        {
            _material.SetColor("_Color0", _color0);
            _material.SetColor("_Color1", _color1);
            _material.SetFloat("_Opacity", _opacity);
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Graphics.Blit(source, destination, _material);
        }

        #endregion
    }
}
