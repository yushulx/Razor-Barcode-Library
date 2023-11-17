using Microsoft.JSInterop;

namespace RazorBarcodeLibrary
{
    /// <summary>
    /// Provides JavaScript interop functionalities for barcode operations.
    /// </summary>
    public class BarcodeJsInterop : IAsyncDisposable
    {
        // Holds a task that resolves to a JavaScript module reference.
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        /// <summary>
        /// Initializes a new instance of the BarcodeJsInterop class.
        /// </summary>
        /// <param name="jsRuntime">The JS runtime to use for invoking JavaScript functions.</param>
        public BarcodeJsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/RazorBarcodeLibrary/barcodeJsInterop.js").AsTask());
        }

        /// <summary>
        /// Loads and initializes the JavaScript module.
        /// </summary>
        public async Task LoadJS()
        {
            var module = await moduleTask.Value;
            await module.InvokeAsync<object>("init");
        }

        /// <summary>
        /// Sets the license key for the barcode functionality.
        /// </summary>
        /// <param name="license">The license key.</param>
        public async Task SetLicense(string license)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("setLicense", license);
        }

        /// <summary>
        /// Loads the WebAssembly for barcode processing.
        /// </summary>
        public async Task LoadWasm()
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("loadWasm");
        }

        /// <summary>
        /// Gets the version of the barcode library.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the version string.</returns>
        public async ValueTask<string> GetVersion()
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("getVersion");
        }

        /// <summary>
        /// Creates a new BarcodeReader instance.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result is a new BarcodeReader instance.</returns>
        public async Task<BarcodeReader> CreateBarcodeReader()
        {
            var module = await moduleTask.Value;
            IJSObjectReference jsObjectReference = await module.InvokeAsync<IJSObjectReference>("createBarcodeReader");
            BarcodeReader reader = new BarcodeReader(module, jsObjectReference);
            return reader;
        }

        /// <summary>
        /// Creates a new BarcodeScanner instance.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result is a new BarcodeScanner instance.</returns>
        public async Task<BarcodeScanner> CreateBarcodeScanner()
        {
            var module = await moduleTask.Value;
            IJSObjectReference jsObjectReference = await module.InvokeAsync<IJSObjectReference>("createBarcodeScanner");
            BarcodeScanner scanner = new BarcodeScanner(module, jsObjectReference);
            return scanner;
        }

        /// <summary>
        /// Converts a Base64 string to a canvas element.
        /// </summary>
        /// <param name="base64">The Base64 encoded string.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is a reference to the canvas element.</returns>
        public async Task<IJSObjectReference> Base64ToCanvas(string base64)
        {
            var module = await moduleTask.Value;
            var canvas = await module.InvokeAsync<IJSObjectReference>("decodeBase64Image", base64);
            return canvas;
        }

        /// <summary>
        /// Draws the barcode results on a specified canvas.
        /// </summary>
        /// <param name="id">The ID of the canvas element.</param>
        /// <param name="sourceWidth">The width of the source image.</param>
        /// <param name="sourceHeight">The height of the source image.</param>
        /// <param name="results">The list of barcode results to draw.</param>
        public async Task DrawCanvas(string id, int sourceWidth, int sourceHeight, List<BarcodeResult> results)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("drawCanvas", id, sourceWidth, sourceHeight, results);
        }

        /// <summary>
        /// Clears the specified canvas element.
        /// </summary>
        /// <param name="id">The ID of the canvas element to clear.</param>
        public async Task ClearCanvas(string id)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("clearCanvas", id);
        }

        /// <summary>
        /// Releases unmanaged resources asynchronously.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (moduleTask.IsValueCreated)
            {
                var module = await moduleTask.Value;
                await module.DisposeAsync();
            }
        }
    }
}