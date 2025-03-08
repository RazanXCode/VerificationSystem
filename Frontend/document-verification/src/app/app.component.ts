
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, RouterOutlet } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { DocumentUploadComponent } from './document-upload/document-upload.component';
import { VerificationComponent } from './verification/verification.component';// Path to your Verification component


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterModule, RouterOutlet, DashboardComponent, DocumentUploadComponent, VerificationComponent],
  template: `
    <nav>
      <a routerLink="/dashboard">Dashboard</a>
      <a routerLink="/upload">Upload</a>
      <a routerLink="/verify">Verify</a>
    </nav>
    <router-outlet></router-outlet>
  `,
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'DocumentVerificationApp';
}