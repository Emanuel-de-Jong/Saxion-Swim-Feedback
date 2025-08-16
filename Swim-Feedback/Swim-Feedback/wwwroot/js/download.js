window.saveAsFile = function (filename, bytesBase64) {
    // Convert the base64 string to a Uint8Array
    const linkSource = atob(bytesBase64);
    const typedArray = new Uint8Array(linkSource.length);
    for (let i = 0; i < linkSource.length; i++) {
        typedArray[i] = linkSource.charCodeAt(i);
    }

    // Create a Blob from the Uint8Array
    const file = new Blob([typedArray], { type: 'application/octet-stream' });

    // Create a temporary link element and trigger the download
    const link = document.createElement('a');
    link.href = URL.createObjectURL(file);
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};
