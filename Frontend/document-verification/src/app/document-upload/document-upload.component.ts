import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';  // ✅ Import HttpClientModule

@Component({
  selector: 'app-document-upload',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],  // ✅ Add HttpClientModule
  templateUrl: './document-upload.component.html',
  styleUrls: ['./document-upload.component.css']
})
export class DocumentUploadComponent {
  document: any = { name: '', file: null };
  message: string = '';

  constructor(private http: HttpClient) {}

  onFileChange(event: any): void {
    this.document.file = event.target.files[0];
  }

  uploadDocument(): void {
    const formData = new FormData();
    formData.append('file', this.document.file, this.document.file.name);
    formData.append('name', this.document.name);
    formData.append('userId', '1'); // Replace '1' with the actual user ID (e.g., from authentication)
  
    this.http.post('http://localhost:5075/api/documents', formData).subscribe({
      next: (response) => this.message = 'Document uploaded successfully!',
      error: (error) => {
        console.error('Upload error:', error);
        this.message = 'Error uploading document.';
      }
    });
}
  
}

