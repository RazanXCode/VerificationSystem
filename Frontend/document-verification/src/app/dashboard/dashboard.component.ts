
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, HttpClientModule],  
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  documents: any[] = [];
  userId: number = 2; // Temporary default user ID for demonstration purposes. 
  // This should be dynamically assigned after user login to fetch their specific documents.

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadDocuments(this.userId);
  }

  loadDocuments(userId: number): void {
    this.http.get<any[]>(`http://localhost:5075/api/documents/${userId}`)
      .subscribe({
        next: (data) => {
          console.log('API Response:', data);
          this.documents = data;  // Assign the array of documents
        },
        error: (error) => {
          console.error('Error fetching documents:', error);
        }
      });
  }
}
