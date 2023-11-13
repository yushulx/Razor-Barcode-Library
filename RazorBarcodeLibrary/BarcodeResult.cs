using System.Text.Json;

namespace RazorBarcodeLibrary
{
    public class BarcodeResult
    {
        public string Text { get; set; } = string.Empty;
        public string Format { get; set; } = string.Empty;

        public string FullInfo { get; set; } = string.Empty;

        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2 { get; set; }
        public int Y2 { get; set; }
        public int X3 { get; set; }
        public int Y3 { get; set; }
        public int X4 { get; set; }
        public int Y4 { get; set; }


        public override string ToString()
        {
            return $"Text: {Text}, Format: {Format}, X1: {X1}, Y1: {Y1}, X2: {X2}, Y2: {Y2}, X3: {X3}, Y3: {Y3}, X4: {X4}, Y4: {Y4}";
        }

        public Dictionary<string, object> ToJson()
        {
            var jsonDict = new Dictionary<string, object>();
            jsonDict.Add("Text", Text ?? "");
            jsonDict.Add("Format", Format ?? "");
            jsonDict.Add("X1", X1);
            jsonDict.Add("Y1", Y1);
            jsonDict.Add("X2", X2);
            jsonDict.Add("Y2", Y2);
            jsonDict.Add("X3", X3);
            jsonDict.Add("Y3", Y3);
            jsonDict.Add("X4", X4);
            jsonDict.Add("Y4", Y4);
            return jsonDict;
        }

        public static List<BarcodeResult> WrapResult(JsonElement? result)
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
                                barcodeResult.X1 = intValue;
                            }

                            if (localizationResult.TryGetProperty("y1", out JsonElement y1Value))
                            {
                                int intValue = y1Value.GetInt32();
                                barcodeResult.Y1 = intValue;
                            }

                            if (localizationResult.TryGetProperty("x2", out JsonElement x2Value))
                            {
                                int intValue = x2Value.GetInt32();
                                barcodeResult.X2 = intValue;
                            }

                            if (localizationResult.TryGetProperty("y2", out JsonElement y2Value))
                            {
                                int intValue = y2Value.GetInt32();
                                barcodeResult.Y2 = intValue;
                            }

                            if (localizationResult.TryGetProperty("x3", out JsonElement x3Value))
                            {
                                int intValue = x3Value.GetInt32();
                                barcodeResult.X3 = intValue;
                            }

                            if (localizationResult.TryGetProperty("y3", out JsonElement y3Value))
                            {
                                int intValue = y3Value.GetInt32();
                                barcodeResult.Y3 = intValue;
                            }

                            if (localizationResult.TryGetProperty("x4", out JsonElement x4Value))
                            {
                                int intValue = x4Value.GetInt32();
                                barcodeResult.X4 = intValue;
                            }

                            if (localizationResult.TryGetProperty("y4", out JsonElement y4Value))
                            {
                                int intValue = y4Value.GetInt32();
                                barcodeResult.Y4 = intValue;
                            }

                            Console.WriteLine(barcodeResult.ToString());

                        }

                        results.Add(barcodeResult);
                    }
                }
            }
            return results;
        }
    }
}
