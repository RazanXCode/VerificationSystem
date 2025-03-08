import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, HttpClientModule],  // Add HttpClientModule here
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  documents: any[] = [];

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadDocuments();
  }
  loadDocuments(): void {
    this.http.get<any>('http://localhost:5075/api/documents/2')  // Fetch single document
      .subscribe({
        next: (data) => {
          console.log('API Response:', data); // Debugging log
          this.documents = [data];  // Convert single object into an array
        },
        error: (error) => {
          console.error('Error fetching document:', error);
        }
      });
  }
  
}
