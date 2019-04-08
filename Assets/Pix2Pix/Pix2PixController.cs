using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pix2Pix
{
    public class Pix2PixController : MonoBehaviour
    {
        [SerializeField] string _weightFileName;

        public RenderTexture _sourceTexture
        {
            get;
            private set;
        }

        public RenderTexture _resultTexture
        {
            get;
            private set;
        }

        Dictionary<string, Pix2Pix.Tensor> _weightTable;
        Pix2Pix.Generator _generator;

        float _budget = 100;
        float _budgetAdjust = 10;

        public float Rate
        {
            get;
            private set;
        }
        // Start is called before the first frame update
        void Start()
        {
            _sourceTexture = new RenderTexture(256, 256, 0);
            _resultTexture = new RenderTexture(256, 256, 0);

            _sourceTexture.filterMode = FilterMode.Point;
            _resultTexture.enableRandomWrite = true;

            _sourceTexture.Create();
            _resultTexture.Create();

            InitializePix2Pix();
        }

        // Update is called once per frame
        void Update()
        {
            UpdatePix2Pix();
        }

        void InitializePix2Pix()
        {
            var filePath = System.IO.Path.Combine(Application.streamingAssetsPath, _weightFileName);
            _weightTable = Pix2Pix.WeightReader.ReadFromFile(filePath);
            print(_weightTable);
            _generator = new Pix2Pix.Generator(_weightTable);
        }

        void UpdatePix2Pix()
        {
            // Advance the Pix2Pix inference until the current budget runs out.
            for (var cost = 0.0f; cost < _budget;)
            {
                if (!_generator.Running) _generator.Start(_sourceTexture);

                cost += _generator.Step();

                if (!_generator.Running) _generator.GetResult(_resultTexture);
            }

            Pix2Pix.GpuBackend.ExecuteAndClearCommandBuffer();

            // Review the budget depending on the current frame time.
            _budget -= (Time.deltaTime * 60 - 1.25f) * _budgetAdjust;
            _budget = Mathf.Clamp(_budget, 150, 1200);

            _budgetAdjust = Mathf.Max(_budgetAdjust - 0.05f, 0.5f);

            // Update the text display.
            Rate = 60 * _budget / 1000;
        }

        void FinalizePix2Pix()
        {
            _generator.Dispose();
            Pix2Pix.WeightReader.DisposeTable(_weightTable);
        }

        void OnDestroy()
        {
            Destroy(_sourceTexture);
            Destroy(_resultTexture);

            FinalizePix2Pix();
        }
    }
}