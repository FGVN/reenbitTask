// app.component.ts
import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './home.component.html'
})
export class HomeComponent {
  title = 'reenbit test app';
  public counter: number;
  imageUrl: string | ArrayBuffer;
  imageMatrix: number[][] = [];
  imageSrc: any;

  constructor() {
    this.counter = 0;
    this.imageUrl = "";
  }

  public increment() {
    this.counter++;
  }

  public reset() {
    this.counter = 0;
  }
  onFileSelected() {
    const inputNode: any = document.querySelector('#file');

    if (typeof (FileReader) !== 'undefined') {
      const reader = new FileReader();

      reader.onload = (e: any) => {
        this.imageSrc = e.target.result;
        this.readImage(inputNode.files[0]); // Uncomment this line to call readImage with the selected file
      };

      reader.readAsArrayBuffer(inputNode.files[0]); // Uncomment this line if you want to read as ArrayBuffer
    }
  }


  readImage(file: File): void {
    const reader = new FileReader();

    reader.onload = (e: any) => {
      this.imageUrl = e.target.result;
      this.convertImageToMatrix(e.target.result);
    };

    reader.readAsDataURL(file);
  }

  convertImageToMatrix(dataUrl: string | ArrayBuffer): void {
    const img = new Image();
    img.src = dataUrl.toString();

    img.onload = () => {
      const canvas = document.createElement('canvas');
      const ctx = canvas.getContext('2d');

      canvas.width = img.width;
      canvas.height = img.height;

      if (ctx) {
        ctx.drawImage(img, 0, 0, img.width, img.height);

        const imageData = ctx.getImageData(0, 0, img.width, img.height).data;

        this.imageMatrix = this.createGrayscaleMatrix(imageData, img.width, img.height);
      }
    };
  }

  createGrayscaleMatrix(imageData: Uint8ClampedArray, width: number, height: number): number[][] {
    const matrix: number[][] = [];

    for (let i = 0; i < height; i++) {
      matrix[i] = [];
      for (let j = 0; j < width; j++) {
        const index = (i * width + j) * 4; // Each pixel has 4 values (RGBA)
        const grayscaleValue = (imageData[index] + imageData[index + 1] + imageData[index + 2]) / 3;
        matrix[i][j] = grayscaleValue;
      }
    }

    return matrix;
  }
  //read image - convert to bits - approx bit matrix - show result
}
