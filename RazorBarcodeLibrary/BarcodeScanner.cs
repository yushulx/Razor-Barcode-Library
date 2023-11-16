using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RazorBarcodeLibrary
{
    public class Camera
    {
        public string DeviceId { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
    }
    public class BarcodeScanner
    {
        private IJSObjectReference _module;
        private IJSObjectReference _jsObjectReference;
        private List<Camera> _cameras = new List<Camera>();

        public BarcodeScanner(IJSObjectReference module, IJSObjectReference scanner)
        {
            _module = module;
            _jsObjectReference = scanner;
        }

        public async Task SetVideoElement(string videoId)
        {
            await _module.InvokeVoidAsync("setVideoElement", _jsObjectReference, videoId);
        }

        public async Task OpenCamera(Camera camera)
        {
            await _module.InvokeVoidAsync("openCamera", _jsObjectReference, camera);
        }

        public async Task CloseCamera()
        {
            await _module.InvokeVoidAsync("closeCamera", _jsObjectReference);
        }

        public async Task<List<Camera>> GetCameras()
        {
            JsonElement? result = await _module.InvokeAsync<JsonElement>("getCameras", _jsObjectReference);

            if (result != null)
            {
                JsonElement element = result.Value;

                if (element.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement item in element.EnumerateArray())
                    {
                        Camera camera = new Camera();
                        if (item.TryGetProperty("deviceId", out JsonElement devideIdValue))
                        {
                            string? value = devideIdValue.GetString();
                            if (value != null)
                            {
                                camera.DeviceId = value;
                            }
                        }

                        if (item.TryGetProperty("label", out JsonElement labelValue))
                        {
                            string? value = labelValue.GetString();
                            if (value != null)
                            {
                                camera.Label = value;
                            }
                        }
                        _cameras.Add(camera);
                    }
                }
            }

            return _cameras;
        }
    }
}
