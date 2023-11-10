using Microsoft.JSInterop;
using System.Text.Json;

namespace RazorBarcodeLibrary
{
    public class Reader
    {
        private IJSObjectReference? _jsObjectReference;

        public Reader(IJSObjectReference reader) { 
            _jsObjectReference = reader;
        }

        public async Task<JsonElement?> Decode(string base64)
        {
            if (_jsObjectReference == null) { return null; }
            var result = await _jsObjectReference.InvokeAsync<JsonElement>("decode", base64);
            return result;
        }
    }

    public class BarcodeJsInterop : IAsyncDisposable
    {
        private readonly Lazy<Task<IJSObjectReference>> moduleTask;

        public BarcodeJsInterop(IJSRuntime jsRuntime)
        {
            moduleTask = new(() => jsRuntime.InvokeAsync<IJSObjectReference>(
                "import", "./_content/RazorBarcodeLibrary/barcodeJsInterop.js").AsTask());
        }

        public async Task InitializeAsync()
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

        public async Task<Reader> CreateBarcodeReader()
        {
            var module = await moduleTask.Value;
            IJSObjectReference jsObjectReference = await module.InvokeAsync<IJSObjectReference>("createBarcodeReader");
            Reader reader = new Reader(jsObjectReference);
            return reader;
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