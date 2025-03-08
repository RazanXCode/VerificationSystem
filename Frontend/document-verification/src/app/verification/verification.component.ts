import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-verification',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './verification.component.html',
  styleUrls: ['./verification.component.css']
})
export class VerificationComponent {
  verification: any = { Id: '', VerificationCode: '' };
  resultMessage: string = '';
  errorMessage: string = '';
  isFormValid: boolean = true;

  constructor(private http: HttpClient) {}

  // Method to validate the form fields
  validateForm(): boolean {
    this.isFormValid = true;
    this.errorMessage = ''; // Reset error message

    // Check if ID and VerificationCode are provided
    if (!this.verification.Id || !this.verification.VerificationCode) {
      this.isFormValid = false;
      this.errorMessage = 'Both fields are required.';
      return false;
    }

    // Check if the ID is numeric (assuming document ID should be a number)
    if (isNaN(this.verification.Id)) {
      this.isFormValid = false;
      this.errorMessage = 'Document ID must be a valid number.';
      return false;
    }

    // Check if the verification code is at least 6 characters long
    if (this.verification.VerificationCode.length < 6) {
      this.isFormValid = false;
      this.errorMessage = 'Verification code must be at least 6 characters long.';
      return false;
    }

    return true;
  }

  // Method to verify document
  verifyDocument(): void {
    if (!this.validateForm()) {
      return; // Don't proceed if the form is invalid
    }

    this.http.post('http://localhost:5075/api/verify', this.verification)
      .subscribe({
        next: (response) => {
          this.resultMessage = 'Document verified successfully!';
          this.errorMessage = ''; // Reset error message
        },
        error: (error) => {
          this.resultMessage = '';
          this.errorMessage = 'Verification failed: The code or document provided is invalid.';
        }
      });
  }
}
