import { Component, Inject, OnInit, PLATFORM_ID, signal } from '@angular/core';
import {
  Router,
  RouterLink,
  RouterLinkActive,
  RouterOutlet,
} from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { AccountService } from './Services/account-service.js';
import { isPlatformBrowser, NgIf } from '@angular/common';

@Component({
  selector: 'app-root',
  imports: [
    RouterOutlet,
    RouterLink,
    RouterLinkActive,
    FontAwesomeModule,
    NgIf,
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App implements OnInit {
  protected readonly title = signal('OrderManagement');
  constructor(
    public Acc: AccountService,
    private router: Router,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {}
  ngOnInit() {
    this.loadUser();
  }
  private loadUser(): void {
    // Only run this in the browser
    if (isPlatformBrowser(this.platformId)) {
      const token =
        localStorage.getItem('token') || sessionStorage.getItem('token');

      if (token) {
        const payload = JSON.parse(atob(token.split('.')[1]));
        const usernanme =
          payload[
            'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'
          ][0];
        this.Acc.currentUserName = usernanme;
      }
    }
  }

  Logout() {
    this.Acc.LogOut().subscribe({
      next: () => {
        // Handle successful logout (e.g., redirect)
        this.Acc.currentUserName = null;
        localStorage.removeItem('token');
        sessionStorage.removeItem('token');
        localStorage.removeItem('refreshToken');
        sessionStorage.removeItem('refreshToken');
        this.router.navigate(['/account/login']);
      },
      error: (err) => {
        console.error('Logout failed:', err);
      },
    });
  }
}
