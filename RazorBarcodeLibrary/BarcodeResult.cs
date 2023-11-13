namespace RazorBarcodeLibrary
{
    public class BarcodeResult
    {
        public string? Text { get; set; }
        public string? Format { get; set; }

        public string? FullInfo { get; set; }

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
    }
}
