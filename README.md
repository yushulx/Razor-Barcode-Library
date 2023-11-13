# Razor Barcode Library
A Razor Class Library, equipped with the [Dynamsoft JavaScript Barcode SDK](https://www.npmjs.com/package/dynamsoft-javascript-barcode), enables the creation of web-based barcode reader applications entirely in C#.

## Prerequisites
- [Dynamsoft JavaScript Barcode License](https://www.dynamsoft.com/customer/license/trialLicense?product=dbr&utm_source=github&utm_campaign=razor-barcode-library)


## Usage
- Import and initialize the Razor Barcode Library.
    
    ```csharp
    @using RazorBarcodeLibrary
    
    protected override async Task OnInitializedAsync()
    {
        barcodeJsInterop = new BarcodeJsInterop(JSRuntime);
        await barcodeJsInterop.LoadJS();
    }
    ```
- Set the license key and load the wasm module.
    
    ```csharp
    await barcodeJsInterop.SetLicense('LICENSE-KEY');
    await barcodeJsInterop.LoadWasm();
    ```

- Create a barcode reader instance.
    
    ```csharp
    BarcodeReader reader = await barcodeJsInterop.CreateBarcodeReader();
    ```

- Read barcodes from a base64 image source.
    
    ```csharp
    List<BarcodeResult> results = await reader.DecodeBase64(imageSrc);
    string text = "";
    foreach (BarcodeResult result in results)
    {
        text += "format: " + result.Format + ", ";
        text += "text: " + result.Text + "<br>";
    }
    ```

## Example
- [Blazor WebAssembly](https://github.com/yushulx/Razor-Barcode-Library/tree/main/example)
    
    <img src="https://github.com/yushulx/Razor-Barcode-Library/assets/2202306/b2cf96db-8466-4c3a-a802-4fd39cec42cd" width="360" alt="blazor multiple qr code detection">