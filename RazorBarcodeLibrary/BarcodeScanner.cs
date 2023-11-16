using Microsoft.JSInterop;
using System.Text.Json;

namespace RazorBarcodeLibrary
{
    public class Camera
    {
        public string DeviceId { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
    }
    public class BarcodeScanner : IDisposable
    {
        private IJSObjectReference _module;
        private IJSObjectReference _jsObjectReference;
        private List<Camera> _cameras = new List<Camera>();
        private ICallback? _callback;
        DotNetObjectReference<BarcodeScanner> objRef;
        private bool _disposed = false;

        public BarcodeScanner(IJSObjectReference module, IJSObjectReference scanner)
        {
            _module = module;
            _jsObjectReference = scanner;
            objRef = DotNetObjectReference.Create(this);
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

        public interface ICallback
        {
            void OnCallback(List<BarcodeResult> results);
        }

        [JSInvokable]
        public Task OnResultReady(object message)
        {
            List<BarcodeResult> results = BarcodeResult.WrapResult((JsonElement)message);
            if (_callback != null)
            {
                _callback.OnCallback(results);
            }

            return Task.CompletedTask;
        }

        public async Task RegisterCallback(ICallback callback)
        {
            _callback = callback;
            await _module.InvokeVoidAsync("registerCallback", _jsObjectReference, objRef, "OnResultReady");
        }

        public void Dispose()
        {
            if (_disposed == false)
            {
                objRef.Dispose();
                _disposed = true;
            }
        }

        ~BarcodeScanner()
        {
            if (_disposed == false)
                Dispose();
        }
    }
}
