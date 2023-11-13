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
        barcodeReader.ifSaveOriginalImageInACanvas = true;
        return barcodeReader;
    }
    catch (ex) {
        console.error(ex);
    }
    return null;
}

export function drawCanvas(canvasId, sourceWidth, sourceHeight, results) {
    var canvas = document.getElementById(canvasId);
    if (!canvas) return;
    canvas.width = sourceWidth;
    canvas.height = sourceHeight;
    var context = canvas.getContext('2d');
    context.clearRect(0, 0, canvas.width, canvas.height);
    
    
    for (var i = 0; i < results.length; ++i) {
        let result = results[i];
        context.beginPath();
        context.strokeStyle = 'red';
        context.lineWidth = 5;
        context.moveTo(result.x1, result.y1);
        context.lineTo(result.x2, result.y2);
        context.lineTo(result.x3, result.y3);
        context.lineTo(result.x4, result.y4);
        context.lineTo(result.x1, result.y1);
        context.stroke();

        let x = [result.x1, result.x2, result.x3, result.x4];
        let y = [result.y1, result.y2, result.y3, result.y4];
        x.sort(function (a, b) {
            return a - b;
        });
        y.sort(function (a, b) {
            return a - b;
        });
        let left = x[0];
        let top = y[0];

        context.font = '18px Verdana';
        context.fillStyle = '#ff0000';
        context.fillText(result.text, left, top);
    }
}

export function clearCanvas(canvasId) {
    var canvas = document.getElementById(canvasId);
    if (!canvas) return;
    var context = canvas.getContext('2d');
    context.clearRect(0, 0, canvas.width, canvas.height);
}

export function decodeBase64Image(base64) {
    return new Promise((resolve, reject) => {
        var canvas = document.createElement("canvas");
        var image = new Image();
        image.src = base64;
        image.onload = () => {
            canvas.width = image.width;
            canvas.height = image.height;
            let context = canvas.getContext('2d');
            context.drawImage(image, 0, 0, canvas.width, canvas.height);
            resolve(canvas);
        };
        image.onerror = (error) => {
            reject(error);
        };
    });
}

export function getSourceWidth(reader) {
    let canvas = reader.getOriginalImageInACanvas();
    return canvas.width;
}

export function getSourceHeight(reader) {
    let canvas = reader.getOriginalImageInACanvas();
    return canvas.height;
}   