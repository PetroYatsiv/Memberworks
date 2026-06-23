import { AfterViewInit, Component, ElementRef, NgZone, inject, signal, viewChild } from '@angular/core';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../core/auth.service';
import { AppConfig } from '../../core/config';

@Component({
  selector: 'app-login',
  imports: [MatCardModule, MatIconModule],
  templateUrl: './login.html',
})
export class LoginComponent implements AfterViewInit {
  private auth = inject(AuthService);
  private router = inject(Router);
  private zone = inject(NgZone);

  buttonHost = viewChild.required<ElementRef<HTMLDivElement>>('googleBtn');
  error = signal<string | null>(null);
  configured = AppConfig.googleClientId.startsWith('REPLACE_') ? false : true;

  ngAfterViewInit(): void {
    if (!this.configured) return;
    this.renderButton();
  }

  private renderButton(attempt = 0): void {
    const google = window.google;
    if (!google) {
      // GSI script may still be loading; retry briefly.
      if (attempt < 20) setTimeout(() => this.renderButton(attempt + 1), 150);
      return;
    }

    google.accounts.id.initialize({
      client_id: AppConfig.googleClientId,
      callback: (res) => this.zone.run(() => this.handleCredential(res.credential)),
    });
    google.accounts.id.renderButton(this.buttonHost().nativeElement, {
      type: 'standard',
      theme: 'outline',
      size: 'large',
      text: 'signin_with',
    });
  }

  private async handleCredential(idToken: string): Promise<void> {
    this.error.set(null);
    try {
      await this.auth.loginWithGoogle(idToken);
      this.router.navigate(['/households']);
    } catch (err) {
      console.error('Google sign-in failed', err);
      const detail =
        err instanceof HttpErrorResponse
          ? `HTTP ${err.status}: ${err.error?.title ?? err.message}`
          : String(err);
      this.error.set(`Sign-in failed. ${detail}`);
    }
  }
}
