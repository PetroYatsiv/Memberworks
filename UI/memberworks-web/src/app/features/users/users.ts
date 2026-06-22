import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { ApiService } from '../../core/api.service';
import { AuthService } from '../../core/auth.service';
import { ApplicationRole, User } from '../../core/models';

@Component({
  selector: 'app-users',
  imports: [
    FormsModule, MatCardModule, MatFormFieldModule, MatInputModule,
    MatSelectModule, MatButtonModule, MatTableModule, MatChipsModule,
  ],
  templateUrl: './users.html',
})
export class UsersComponent {
  private api = inject(ApiService);
  auth = inject(AuthService);

  users = signal<User[]>([]);
  error = signal<string | null>(null);
  saving = signal(false);

  readonly columns = ['name', 'email', 'role'];
  form = { email: '', firstName: '', lastName: '', role: 'Member' as ApplicationRole };

  constructor() {
    this.load();
  }

  load(): void {
    this.api.getUsers().subscribe((u) => this.users.set(u));
  }

  create(): void {
    this.error.set(null);
    this.saving.set(true);
    this.api.createUser({ ...this.form }).subscribe({
      next: () => {
        this.form = { email: '', firstName: '', lastName: '', role: 'Member' };
        this.saving.set(false);
        this.load();
      },
      error: (err) => {
        this.error.set(err?.error?.title ?? 'Could not create user.');
        this.saving.set(false);
      },
    });
  }
}
