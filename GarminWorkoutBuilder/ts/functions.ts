namespace InteropFunctions {
    export function saveFile(filename, base64) {
        console.log("triggering download");
        const link = document.createElement('a');
        link.download = filename;
        link.href = "data:text/plain;base64," + encodeURIComponent(base64);
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        console.log("triggered download");
    }
}
Object.assign(window, { InteropFunctions });
