export function init(license) {
    return new Promise((resolve, reject) => {
        let script = document.createElement('script');
        script.type = 'text/javascript';
        script.src = '_content/RazorBarcodeLibrary/dbr.js';
        script.onload = async () => {
            resolve(); 
        };
        script.onerror = () => {
            reject(); 
        };
        document.head.appendChild(script);
    });
}

export function getVersion() {
    if (!Dynamsoft) return "";
    return Dynamsoft.DBR.BarcodeReader.version;
}

export function setLicense(license) {
    if (!Dynamsoft) return;
    Dynamsoft.DBR.BarcodeScanner.license = license;
}

export async function loadWasm() {
    if (!Dynamsoft) return;
    try {
        await Dynamsoft.DBR.BarcodeReader.loadWasm();
    }
    catch (ex) {
        console.error(ex);
    }
}

export async function createBarcodeReader(source) {
    if (!Dynamsoft) return;

    try {
        let barcodeReader = await Dynamsoft.DBR.BarcodeReader.createInstance();
        return barcodeReader;
    }
    catch (ex) {
        console.error(ex);
    }
    return null;
}

export async function getParameters(source) {

}

export async function setParameters(source) {

}
