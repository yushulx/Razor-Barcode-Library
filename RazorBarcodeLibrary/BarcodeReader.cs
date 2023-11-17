using Microsoft.JSInterop;
using System.Text.Json;

namespace RazorBarcodeLibrary
{
    /// <summary>
    /// Provides functionalities to decode barcodes from various sources and manage barcode reader settings.
    /// </summary>
    public class BarcodeReader
    {
        // Fields to hold JavaScript object references.
        private IJSObjectReference _module;
        private IJSObjectReference _jsObjectReference;

        // Public properties to store source dimensions.
        public int SourceWidth, SourceHeight;

        /// <summary>
        /// Initializes a new instance of the BarcodeReader class.
        /// </summary>
        /// <param name="module">A reference to the JavaScript module.</param>
        /// <param name="reader">A reference to the JavaScript object for barcode reading.</param>
        public BarcodeReader(IJSObjectReference module, IJSObjectReference reader)
        {
            _module = module;
            _jsObjectReference = reader;
        }

        /// <summary>
        /// Asynchronously decodes a barcode from a Base64 encoded string.
        /// </summary>
        /// <param name="base64">The Base64 encoded string containing the barcode image.</param>
        /// <returns>A task that represents the asynchronous decode operation. The task result contains a list of BarcodeResult objects.</returns>
        public async Task<List<BarcodeResult>> DecodeBase64(string base64)
        {
            JsonElement? result = await _jsObjectReference.InvokeAsync<JsonElement>("decode", base64);
            SourceWidth = await _module.InvokeAsync<int>("getSourceWidth", _jsObjectReference);
            SourceHeight = await _module.InvokeAsync<int>("getSourceHeight", _jsObjectReference);
            return BarcodeResult.WrapResult(result);
        }

        /// <summary>
        /// Asynchronously decodes a barcode from a canvas object.
        /// </summary>
        /// <param name="canvas">A reference to the JavaScript object representing the canvas with the barcode image.</param>
        /// <returns>A task that represents the asynchronous decode operation. The task result contains a list of BarcodeResult objects.</returns>
        public async Task<List<BarcodeResult>> DecodeCanvas(IJSObjectReference canvas)
        {
            JsonElement? result = await _jsObjectReference.InvokeAsync<JsonElement>("decode", canvas);
            SourceWidth = await _module.InvokeAsync<int>("getSourceWidth", _jsObjectReference);
            SourceHeight = await _module.InvokeAsync<int>("getSourceHeight", _jsObjectReference);
            return BarcodeResult.WrapResult(result);
        }

        /// <summary>
        /// Asynchronously retrieves the current parameters of the barcode reader.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result is a string representation of the current barcode reader settings.</returns>
        public async Task<string> GetParameters()
        {
            if (_jsObjectReference == null) { return ""; }

            return await _jsObjectReference.InvokeAsync<string>("outputRuntimeSettingsToString");
        }

        /// <summary>
        /// Asynchronously sets the parameters for the barcode reader.
        /// </summary>
        /// <param name="parameters">A string representation of the settings to apply to the barcode reader.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is an integer indicating the status (0 for success, -1 for failure).</returns>
        public async Task<int> SetParameters(string parameters)
        {
            if (_jsObjectReference == null) { return -1; }

            try
            {
                await _jsObjectReference.InvokeVoidAsync("initRuntimeSettingsWithString", parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return -1;
            }

            return 0;
        }
    }
}
