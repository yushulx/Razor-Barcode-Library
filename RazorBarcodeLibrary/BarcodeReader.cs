using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RazorBarcodeLibrary
{
    public class BarcodeReader
    {
        private IJSObjectReference? _jsObjectReference;

        public BarcodeReader(IJSObjectReference reader)
        {
            _jsObjectReference = reader;
        }

        public async Task<List<BarcodeResult>> DecodeBase64(string base64)
        {
            List<BarcodeResult> results = new List<BarcodeResult>();
            if (_jsObjectReference == null) { return results; }
            JsonElement? result = await _jsObjectReference.InvokeAsync<JsonElement>("decode", base64);
            if (result != null)
            {
                JsonElement element = result.Value;

                if (element.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement item in element.EnumerateArray())
                    {
                        BarcodeResult barcodeResult = new BarcodeResult();
                        barcodeResult.FullInfo = item.ToString();
                        if (item.TryGetProperty("barcodeFormatString", out JsonElement formatValue))
                        {
                            string? value = formatValue.GetString();
                            if (value != null)
                            {
                                barcodeResult.Format = value;
                            }

                        }

                        if (item.TryGetProperty("barcodeText", out JsonElement textValue))
                        {
                            string? value = textValue.GetString();
                            if (value != null)
                            {
                                barcodeResult.Text = value;
                            }

                        }

                        results.Add(barcodeResult);
                    }
                }
            }

            return results;
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
