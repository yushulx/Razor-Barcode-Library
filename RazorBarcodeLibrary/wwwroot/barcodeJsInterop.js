export function getVersion() {
    return Dynamsoft.DBR.BarcodeReader.version;
}

export function init() {
    let script = document.createElement('script');
    script.type = 'text/javascript';
    script.src = '_content/RazorBarcodeLibrary/dbr.js';
    script.onload = async () => {
        await Dynamsoft.DBR.BarcodeReader.loadWasm();
    }
    script.onerror = () => {
    }
    document.head.appendChild(script);
}