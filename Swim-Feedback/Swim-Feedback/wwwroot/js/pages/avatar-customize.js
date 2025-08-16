let canvas;
let canvasCtx;

function createCanvas() {
    canvas = document.getElementById("avatar-canvas");
    canvas.width = 200;
    canvas.height = 200;

    canvasCtx = canvas.getContext("2d");
};

function addCanvasImg(imgSrc) {
    let img = new Image();
    img.src = "data:image/png;base64," + imgSrc;

    img.onload = () => {
        canvasCtx.drawImage(img, 0, 0, canvas.width, canvas.height);
    };
}
