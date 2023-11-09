using Microsoft.JSInterop;

namespace RazorBarcodeLibrary
{
    // This class provides an example of how JavaScript functionality can be wrapped
    // in a .NET class for easy consumption. The associated JavaScript module is
    // loaded on demand when first needed.
    //
    // This class can be registered as scoped DI service and then injected into Blazor
    // components for use.

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
            await module.InvokeVoidAsync("init");
        }

        public async ValueTask<string> GetVersion()
        {
            var module = await moduleTask.Value;
            return await module.InvokeAsync<string>("getVersion");
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