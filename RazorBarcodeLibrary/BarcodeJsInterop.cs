using Microsoft.JSInterop;
using System.Text.Json;

namespace RazorBarcodeLibrary
{
    public class BarcodeJsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public BarcodeJsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/RazorBarcodeLibrary/barcodeJsInterop.js").AsTask());
        }

        public async Task LoadJS()
        {
            var module = await moduleTask.Value;
            await module.InvokeAsync<object>("init");
        }

        public async Task SetLicense(string license)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("setLicense", license);
        }

        public async Task LoadWasm()
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("loadWasm");
        }

        public async ValueTask<string> GetVersion()
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("getVersion");
        }

        public async Task<BarcodeReader> CreateBarcodeReader()
        {
            var module = await moduleTask.Value;
            IJSObjectReference jsObjectReference = await module.InvokeAsync<IJSObjectReference>("createBarcodeReader");
            BarcodeReader reader = new BarcodeReader(module, jsObjectReference);
            return reader;
        }

        public async Task<IJSObjectReference> Base64ToCanvas(string base64)
        {
            var module = await moduleTask.Value;
            var canvas = await module.InvokeAsync<IJSObjectReference>("decodeBase64Image", base64);
            return canvas;
        }

        public async Task DrawCanvas(string id, int sourceWidth, int sourceHeight, List<BarcodeResult> results)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("drawCanvas", id, sourceWidth, sourceHeight, results);
        }

        public async Task ClearCanvas(string id)
        {
            var module = await moduleTask.Value;
            await module.InvokeVoidAsync("clearCanvas", id);
        }

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