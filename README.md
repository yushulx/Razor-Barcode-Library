# Razor Barcode Library
A Razor Class Library, equipped with the [Dynamsoft JavaScript Barcode SDK](https://www.npmjs.com/package/dynamsoft-javascript-barcode), enables the creation of web-based barcode reader and scanner applications entirely in C#.

## Online Demo
[https://yushulx.me/Razor-Barcode-Library/](https://yushulx.me/Razor-Barcode-Library/)

## Demo Video
https://github.com/yushulx/Razor-Barcode-Library/assets/2202306/ac3c333f-7895-420a-94ee-6debe805a8b7



## Prerequisites
- [Dynamsoft JavaScript Barcode License](https://www.dynamsoft.com/customer/license/trialLicense?product=dbr&utm_source=github&utm_campaign=razor-barcode-library)


## Quick Usage
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

- Create a barcode scanner instance and set the callback function.
    
    ```csharp
    @implements BarcodeScanner.ICallback

    BarcodeReader reader = await barcodeJsInterop.CreateBarcodeScanner();
    await scanner.RegisterCallback(this);

    public async Task OnCallback(List<BarcodeResult> results) {}
    ```
- Open the camera and start scanning.
    
    ```csharp
    <div id="videoContainer"></div>

    await scanner.SetVideoElement("videoContainer");
    List<Camera> cameras = await scanner.GetCameras();
    await scanner.OpenCamera(cameras[0]);
    ```

## API

### Camera Class
Represents a camera device with its device ID and label.

### BarcodeResult Class
Represents the result of a barcode scan, including the decoded text, format, and positional details.

### BarcodeJsInterop Class

Provides JavaScript interop functionalities for barcode operations. 

- `Task LoadJS()`: Loads and initializes the JavaScript module.
- `Task SetLicense(string license)`: Sets the license key for the barcode functionality.
- `Task LoadWasm()`: Loads the WebAssembly for barcode processing.
- `Task<BarcodeReader> CreateBarcodeReader()`: Creates a new BarcodeReader instance.
- `Task<BarcodeScanner> CreateBarcodeScanner()`: Creates a new BarcodeScanner instance.
- `Task DrawCanvas(string id, int sourceWidth, int sourceHeight, List<BarcodeResult> results)`: Draws the barcode results on a specified canvas.
- `Task ClearCanvas(string id)`: Clears the specified canvas element.

### BarcodeReader Class

Provides functionalities to decode barcodes from various sources such as Base64 strings and canvas objects. 

- `Task<List<BarcodeResult>> DecodeBase64(string base64)`: Asynchronously decodes a barcode from a Base64 encoded string.
- `Task<List<BarcodeResult>> DecodeCanvas(IJSObjectReference canvas)`: Asynchronously decodes a barcode from a canvas object.
- `Task<string> GetParameters()`: Asynchronously retrieves the current parameters of the barcode reader.
- `Task<int> SetParameters(string parameters)`: Asynchronously sets the parameters for the barcode reader.

### BarcodeScanner Class

Provides functionalities for barcode scanning using a camera. 

- `Task SetVideoElement(string videoId)`: Sets a div element as the video container.
- `Task OpenCamera(Camera camera)`: Opens the camera for barcode scanning.
- `Task CloseCamera()`: Closes the current camera.
- `Task<List<Camera>> GetCameras()`: Gets a list of available cameras.
- `Task RegisterCallback(ICallback callback)`: Registers a callback for handling barcode scan results.

## Example
- [Blazor WebAssembly](https://github.com/yushulx/Razor-Barcode-Library/tree/main/example)
    
    ![razor barcode reader](https://user-images.githubusercontent.com/2202306/282373638-b2cf96db-8466-4c3a-a802-4fd39cec42cd.png)