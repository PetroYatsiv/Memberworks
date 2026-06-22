import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import { ApiService } from '../../core/api.service';
import { HouseholdSummary, User } from '../../core/models';

@Component({
  selector: 'app-households',
  imports: [
    FormsModule, RouterLink, MatCardModule, MatFormFieldModule, MatInputModule,
    MatSelectModule, MatButtonModule, MatTableModule, MatIconModule,
  ],
  templateUrl: './households.html',
})
export class HouseholdsComponent {
  private api = inject(ApiService);

  households = signal<HouseholdSummary[]>([]);
  users = signal<User[]>([]);
  error = signal<string | null>(null);
  saving = signal(false);

  readonly columns = ['name', 'primary', 'members', 'actions'];
  form = { name: '', primaryUserId: '' };

  constructor() {
    this.load();
    this.api.getUsers().subscribe((u) => this.users.set(u));
  }

  load(): void {
    this.api.getHouseholds().subscribe((h) => this.households.set(h));
  }

  create(): void {
    this.error.set(null);
    this.saving.set(true);
    this.api.createHousehold({ ...this.form }).subscribe({
      next: () => {
        this.form = { name: '', primaryUserId: '' };
        this.saving.set(false);
        this.load();
      },
      error: (err) => {
        this.error.set(err?.error?.title ?? 'Could not create household.');
        this.saving.set(false);
      },
    });
  }
}
