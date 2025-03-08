import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';  // For two-way data binding
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http'; // ✅ Import HttpClientModule

@Component({
  selector: 'app-verification',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule], // ✅ Add HttpClientModule
  templateUrl: './verification.component.html',
  styleUrls: ['./verification.component.css']
})
export class VerificationComponent {
  verification: any = { Id: '', VerificationCode: '' }; // ✅ Match the API's expected properties
  resultMessage: string = '';

  constructor(private http: HttpClient) {} // ✅ HttpClient is now available

  verifyDocument(): void {
    this.http.post('http://localhost:5075/api/verify', this.verification)
      .subscribe({
        next: (response) => this.resultMessage = 'Document verified successfully!',
        error: (error) => this.resultMessage = 'Verification failed: The code or document provided is invalid, or the document has not been verified.'
      });
  }
}