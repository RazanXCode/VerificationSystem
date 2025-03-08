
import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';  
import {store} './redux/actions';  // Import Redux store
import { uploadDocument, uploadSuccess, uploadFailure, setErrors } from './redux/actions';  // Import Redux actions

@Component({
  selector: 'app-document-upload',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],  
  templateUrl: './document-upload.component.html',
  styleUrls: ['./document-upload.component.css']
})
export class DocumentUploadComponent {
  document: any = { name: '', file: null };
  message: string = '';
  errors: string[] = []; 

  constructor(private http: HttpClient) {}

  onFileChange(event: any): void {
    this.document.file = event.target.files[0];
  }

  uploadDocument(): void {
    // Clear previous errors
    this.errors = [];

    // Validate document name length (must exceed 5 characters)
    if (!this.document.name || this.document.name.length <= 5) {
      this.errors.push('Document name must be more than 5 characters.');
    }

    // Validate file type (must be PDF or Word)
    if (!this.document.file) {
      this.errors.push('Document file is required.');
    } else {
      const fileExtension = this.document.file.name.split('.').pop()?.toLowerCase();
      if (fileExtension !== 'pdf' && fileExtension !== 'doc' && fileExtension !== 'docx') {
        this.errors.push('File must be a PDF or Word document (DOC/DOCX).');
      }
    }
        // Dispatch errors to Redux
    store.dispatch(setErrors(this.errors));


    // If there are validation errors, don't proceed with upload and show error message
    if (this.errors.length > 0) {
      this.message = 'Please fix the errors and try again.';
      return;
    }

    // Proceed with the upload if all validations pass
    const formData = new FormData();
    formData.append('file', this.document.file, this.document.file.name);
    formData.append('name', this.document.name);
    formData.append('userId', '2'); // Should Replace with the actual user ID ( from authentication)
  
   
    // Dispatch the upload action to Redux
    store.dispatch(uploadDocument(formData));

    this.http.post('http://localhost:5075/api/documents', formData).subscribe({
      next: (response) => this.message = 'Document uploaded successfully!',
      error: (error) => {
        console.error('Upload error:', error);
        this.message = 'Error uploading document.';
      }
    });
  }
}
