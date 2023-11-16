﻿using Microsoft.JSInterop;
using System.Text.Json;

namespace RazorBarcodeLibrary
{
    public class BarcodeReader
    {
        private IJSObjectReference _module;
        private IJSObjectReference _jsObjectReference;

        public int SourceWidth, SourceHeight;

        public BarcodeReader(IJSObjectReference module, IJSObjectReference reader)
        {
            _module = module;
            _jsObjectReference = reader;
        }

        public async Task<List<BarcodeResult>> DecodeBase64(string base64)
        {
            JsonElement? result = await _jsObjectReference.InvokeAsync<JsonElement>("decode", base64);
            SourceWidth = await _module.InvokeAsync<int>("getSourceWidth", _jsObjectReference);
            SourceHeight = await _module.InvokeAsync<int>("getSourceHeight", _jsObjectReference);
            return BarcodeResult.WrapResult(result);
        }

        public async Task<List<BarcodeResult>> DecodeCanvas(IJSObjectReference canvas)
        {
            JsonElement? result = await _jsObjectReference.InvokeAsync<JsonElement>("decode", canvas);
            SourceWidth = await _module.InvokeAsync<int>("getSourceWidth", _jsObjectReference);
            SourceHeight = await _module.InvokeAsync<int>("getSourceHeight", _jsObjectReference);
            return BarcodeResult.WrapResult(result);
        }

        public async Task<string> GetParameters()
        {
            if (_jsObjectReference == null) { return ""; }

            return await _jsObjectReference.InvokeAsync<string>("outputRuntimeSettingsToString");
        }

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
