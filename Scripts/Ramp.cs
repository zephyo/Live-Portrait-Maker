
using UnityEngine;

namespace Kino
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    [AddComponentMenu("Kino Image Effects/Ramp")]
    public class Ramp : MonoBehaviour
    {
        #region Public Properties

        // first color

        [SerializeField]
        Color _color1 = Color.blue;

        public Color FirstColor
        {
            get { return _color1; }
            set { _color1 = value; }
        }

        // second color

        [SerializeField]
        Color _color2 = Color.red;

        public Color SecondColor
        {
            get { return _color2; }
            set { _color2 = value; }
        }

        // blend opacity

        [Range(0, 1)]
        public float _opacity = 0.5f;
        // blend mode



        #endregion

        #region Private Properties

        [SerializeField] Shader _shader;
        Material _material;

        #endregion

        #region MonoBehaviour Functions

        private void Awake()
        {
            if (_material == null)
            {
                _material = new Material(Shader.Find("Ramp"));
                _material.hideFlags = HideFlags.DontSave;
            }

        }

        public void updateColors()
        {
            _material.SetColor("_Color1", Color.Lerp(Color.gray, _color1, _opacity));
            _material.SetColor("_Color2", Color.Lerp(Color.gray, _color2, _opacity));
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {

            if (QualitySettings.activeColorSpace == ColorSpace.Linear)
                _material.EnableKeyword("_LINEAR");
            else
                _material.DisableKeyword("_LINEAR");



            Graphics.Blit(source, destination, _material, 0);
        }

        #endregion
    }
}
