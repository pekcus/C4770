using UnityEngine;

namespace A1
{
    /// <summary>
    /// Hold data for individual floor components.
    /// </summary>
    [DisallowMultipleComponent]
    public class MyFloor : MonoBehaviour
    {
        /// <summary>
        /// The current light level of this floor.
        /// </summary>
        public enum LightLevel : byte
        {
            Dark,
            Light1,
            Light2,
            Light3,
            Base
        }

        /// <summary>
        /// The material to display when this floor is a base tile.
        /// </summary>
        private Material _baseMaterial;

        /// <summary>
        /// The material to display when this floor is dark.
        /// </summary>
        private Material _darkMaterial;

        /// <summary>
        /// The material to display when this floor lights up to color 1.
        /// </summary>
        private Material _l1Material;

        /// <summary>
        /// The material to display when this floor lights up to color 2.
        /// </summary>
        private Material _l2Material;

        /// <summary>
        /// The material to display when this floor lights up to color 3.
        /// </summary>
        private Material _l3Material;

        /// <summary>
        /// The mesh renderer to apply the materials to.
        /// </summary>
        private MeshRenderer _meshRenderer;

        /// <summary>
        /// How this floor tile is lit up.
        /// </summary>
        public LightLevel State { get; private set; }

        /// <summary>
        /// If this floor is likely to get dirty. Floors where this is true are twice as likely to get more dirty than other floor tiles.
        /// </summary>
        public bool _isBase { get; private set; }

        /// <summary>
        /// If the floor tile is lit or not.
        /// </summary>
        public bool IsLit => State >= LightLevel.Light1;

        private void Start()
        {
            UpdateMaterial();
        }

        /// <summary>
        /// When hit, the tile turns off.
        /// </summary>
        public bool Hit()
        {
            if (State == LightLevel.Dark || _isBase)
                return false;
            State = LightLevel.Dark;
            UpdateMaterial();
            return true;
        }

        /// <summary>
        /// Light this floor tile increasing its light level by one.
        /// </summary>
        public void Light()
        {
            if (_isBase)
                return;
            State++;
            UpdateMaterial();
        }

        /// <summary>
        /// Configure this floor tile.
        /// </summary>
        /// <param name="likelyToGetDirty">{Unused} If this floor is likely to get dirty.</param>
        /// <param name="cleanMaterial">The material to display when this floor is clean.</param>
        /// <param name="dirtyMaterial">The material to display when this floor is dirty.</param>
        /// <param name="veryDirtyMaterial">The material to display when this floor is very dirty.</param>
        /// <param name="extremelyDirtyMaterial">The material to display when this floor is extremely dirty.</param>
        public void Setup(bool isBase, Material baseMaterial, Material darkMaterial, Material L1Material,
            Material L2Material, Material L3Material)
        {
            _isBase = isBase;
            _baseMaterial = baseMaterial;
            _darkMaterial = darkMaterial;
            _l1Material = L1Material;
            _l2Material = L2Material;
            _l3Material = L3Material;
            if (!_isBase)
                State = LightLevel.Dark;
        }

        /// <summary>
        /// Update the material based on the current state of the floor.
        /// </summary>
        private void UpdateMaterial()
        {
            if (_meshRenderer == null)
            {
                _meshRenderer = GetComponent<MeshRenderer>();
            }

            _meshRenderer.material = State switch
            {
                LightLevel.Base => _baseMaterial,
                LightLevel.Dark => _darkMaterial,
                LightLevel.Light1 => _l1Material,
                LightLevel.Light2 => _l2Material,
                LightLevel.Light3 => _l3Material
            };
        }
    }
}