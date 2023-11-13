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
            return WrapResult(result);
        }

        private List<BarcodeResult> WrapResult(JsonElement? result)
        {
            List<BarcodeResult> results = new List<BarcodeResult>();
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

                        if (item.TryGetProperty("localizationResult", out JsonElement localizationResult))
                        {
                            if (localizationResult.TryGetProperty("x1", out JsonElement x1Value))
                            {
                                int intValue = x1Value.GetInt32();
                                barcodeResult.x1 = intValue;
                            }

                            if (localizationResult.TryGetProperty("y1", out JsonElement y1Value))
                            {
                                int intValue = y1Value.GetInt32();
                                barcodeResult.y1 = intValue;
                            }

                            if (localizationResult.TryGetProperty("x2", out JsonElement x2Value))
                            {
                                int intValue = x2Value.GetInt32();
                                barcodeResult.x2 = intValue;
                            }

                            if (localizationResult.TryGetProperty("y2", out JsonElement y2Value))
                            {
                                int intValue = y2Value.GetInt32();
                                barcodeResult.y2 = intValue;
                            }

                            if (localizationResult.TryGetProperty("x3", out JsonElement x3Value))
                            {
                                int intValue = x3Value.GetInt32();
                                barcodeResult.x3 = intValue;
                            }

                            if (localizationResult.TryGetProperty("y3", out JsonElement y3Value))
                            {
                                int intValue = y3Value.GetInt32();
                                barcodeResult.y3 = intValue;
                            }

                            if (localizationResult.TryGetProperty("x4", out JsonElement x4Value))
                            {
                                int intValue = x4Value.GetInt32();
                                barcodeResult.x4 = intValue;
                            }

                            if (localizationResult.TryGetProperty("y4", out JsonElement y4Value))
                            {
                                int intValue = y4Value.GetInt32();
                                barcodeResult.y4 = intValue;
                            }

                            Console.WriteLine(barcodeResult.ToString());

                        }

                        results.Add(barcodeResult);
                    }
                }
            }
            return results;
        }

        public async Task<List<BarcodeResult>> DecodeCanvas(IJSObjectReference canvas)
        {
            List<BarcodeResult> results = new List<BarcodeResult>();
            if (_jsObjectReference == null) { return results; }
            JsonElement? result = await _jsObjectReference.InvokeAsync<JsonElement>("decode", canvas);
            return WrapResult(result);
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
