import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  email: string = '';
  file: File = new File([], "null");
  errorMessage: string = '';

  constructor(private http: HttpClient) {
    // Injecting HttpClient in the constructor
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
          // Reset error message
          alert(response.message);
          // Alert with success message
        },
        (error: any) => {
          console.error(error);
          // Display detailed error message from IActionResult response
          this.errorMessage = error.error.title || error.error.message;
          alert(this.errorMessage);
        }
      );

  }
}
