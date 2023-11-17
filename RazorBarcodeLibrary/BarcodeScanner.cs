using Microsoft.JSInterop;
using System.Text.Json;

namespace RazorBarcodeLibrary
{
    /// <summary>
    /// Represents a camera device with its device ID and label.
    /// </summary>
    public class Camera
    {
        /// <summary>
        /// Gets or sets the unique device identifier for the camera.
        /// </summary>
        public string DeviceId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the label of the camera.
        /// </summary>
        public string Label { get; set; } = string.Empty;
    }

    /// <summary>
    /// Provides functionalities for barcode scanning using a camera.
    /// </summary>
    public class BarcodeScanner : IDisposable
    {
        // Private fields for JavaScript object references and internal state
        private IJSObjectReference _module;
        private IJSObjectReference _jsObjectReference;
        private List<Camera> _cameras = new List<Camera>();
        private ICallback? _callback;
        private DotNetObjectReference<BarcodeScanner> objRef;
        private bool _disposed = false;

        // Public properties for source dimensions
        public int SourceWidth, SourceHeight;

        /// <summary>
        /// Initializes a new instance of the BarcodeScanner class.
        /// </summary>
        /// <param name="module">A reference to the JavaScript module.</param>
        /// <param name="scanner">A reference to the JavaScript object for barcode scanning.</param>
        public BarcodeScanner(IJSObjectReference module, IJSObjectReference scanner)
        {
            _module = module;
            _jsObjectReference = scanner;
            objRef = DotNetObjectReference.Create(this);
        }

        /// <summary>
        /// Sets a div element as the video container.
        /// </summary>
        /// <param name="videoId">The ID of the div element.</param>
        public async Task SetVideoElement(string videoId)
        {
            await _module.InvokeVoidAsync("setVideoElement", _jsObjectReference, videoId);
        }

        /// <summary>
        /// Opens the camera for barcode scanning.
        /// </summary>
        /// <param name="camera">The camera to be used for scanning.</param>
        public async Task OpenCamera(Camera camera)
        {
            await _module.InvokeVoidAsync("openCamera", _jsObjectReference, camera, objRef, "OnSizeChanged");
        }

        /// <summary>
        /// Closes the current camera.
        /// </summary>
        public async Task CloseCamera()
        {
            await _module.InvokeVoidAsync("closeCamera", _jsObjectReference);
        }

        /// <summary>
        /// Gets a list of available cameras.
        /// </summary>
        /// <returns>A list of available Camera objects.</returns>
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

        /// <summary>
        /// Interface for barcode scanner callback.
        /// </summary>
        public interface ICallback
        {
            Task OnCallback(List<BarcodeResult> results);
        }

        /// <summary>
        /// Invoked when barcode scan results are ready.
        /// </summary>
        /// <param name="message">The scan result data.</param>
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

        /// <summary>
        /// Invoked when the size of the video source changes.
        /// </summary>
        /// <param name="width">The new width of the video source.</param>
        /// <param name="height">The new height of the video source.</param>
        [JSInvokable]
        public Task OnSizeChanged(int width, int height)
        {
            SourceWidth = width;
            SourceHeight = height;

            return Task.CompletedTask;
        }

        /// <summary>
        /// Registers a callback for handling barcode scan results.
        /// </summary>
        /// <param name="callback">The callback to register.</param>
        public async Task RegisterCallback(ICallback callback)
        {
            _callback = callback;
            await _module.InvokeVoidAsync("registerCallback", _jsObjectReference, objRef, "OnResultReady");
        }

        /// <summary>
        /// Release unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_disposed == false)
            {
                objRef.Dispose();
                _disposed = true;
            }
        }

        /// <summary>
        /// Destructor for the BarcodeScanner class.
        /// </summary>
        ~BarcodeScanner()
        {
            if (_disposed == false)
                Dispose();
        }
    }
}
