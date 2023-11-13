namespace RazorBarcodeLibrary
{
    public class BarcodeResult
    {
        public string? Text { get; set; }
        public string? Format { get; set; }

        public string? FullInfo { get; set; }

        public int x1, y1, x2, y2, x3, y3, x4, y4;

        public override string ToString()
        {
            return $"Text: {Text}, Format: {Format}, x1: {x1}, y1: {y1}, x2: {x2}, y2: {y2}, x3: {x3}, y3: {y3}, x4: {x4}, y4: {y4}";
        }
    }
}
