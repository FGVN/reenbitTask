import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-root',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  email: string = '';
  file: File = new File([], "null");
  errorMessage: string = '';

  constructor(private http: HttpClient, private snackBar: MatSnackBar) {
    // Injecting HttpClient and MatSnackBar in the constructor
  }

  onFileSelected() {
    const inputNode: any = document.querySelector('#file');
    this.file = inputNode.files[0] as File;
  }

  uploadFile(): void {
    const formData = new FormData();
    formData.append('email', this.email);
    formData.append('file', this.file);

    this.http.post('/file/upload', formData)
      .subscribe(
        (response: any) => {
          // Display success message
          this.snackBar.open(response.message, 'Success', {
            duration: 3000, // 3 seconds
            panelClass: ['snackbar-success']
          });
        },
        (error: any) => {
          console.error(error);
          // Display detailed error message from IActionResult response
          this.errorMessage = error.error.title || error.error.message;
          // Display error message
          this.snackBar.open(this.errorMessage, 'Error', {
            duration: 5000, // 5 seconds
            panelClass: ['snackbar-error']
          });
        }
      );

  }
}
