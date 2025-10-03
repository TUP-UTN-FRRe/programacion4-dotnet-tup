const canvas = document.getElementById('myCanvas');
const ctx = canvas.getContext('2d');

let isDrawing = false;
let lastX = 0;
let lastY = 0;

// Set up drawing styles
ctx.strokeStyle = '#000000'; // Black color
ctx.lineWidth = 10; // 2 pixels wide line
ctx.lineJoin = 'round';
ctx.lineCap = 'round';

canvas.addEventListener('mousedown', (e) => {
    isDrawing = true;
    [lastX, lastY] = [e.offsetX, e.offsetY]; // Get current mouse position
});

canvas.addEventListener('mousemove', (e) => {
    if (!isDrawing) return; // Stop the function from running when not drawing

    ctx.beginPath(); // Start a new path
    ctx.moveTo(lastX, lastY); // Move to the last recorded position
    ctx.lineTo(e.offsetX, e.offsetY); // Draw a line to the current mouse position
    ctx.stroke(); // Render the line

    console.log(e.offsetX, e.offsetY);

    [lastX, lastY] = [e.offsetX, e.offsetY]; // Update last position
});

canvas.addEventListener('mouseup', () => {
    isDrawing = false;
});

canvas.addEventListener('mouseout', () => {
    isDrawing = false; // Stop drawing if the mouse leaves the canvas
});