import { Component, computed, inject, input, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { ApiService } from '../../core/api.service';
import { HouseholdDetail, RELATIONSHIP_TYPES, RelationshipType, User } from '../../core/models';

@Component({
  selector: 'app-household-detail',
  imports: [
    FormsModule, RouterLink, MatCardModule, MatFormFieldModule, MatSelectModule,
    MatButtonModule, MatTableModule, MatChipsModule, MatIconModule,
  ],
  templateUrl: './household-detail.html',
})
export class HouseholdDetailComponent {
  private api = inject(ApiService);

  // Bound from the :id route param (withComponentInputBinding is enabled in provideRouter).
  id = input.required<string>();

  household = signal<HouseholdDetail | null>(null);
  allUsers = signal<User[]>([]);
  error = signal<string | null>(null);

  relationshipTypes = RELATIONSHIP_TYPES;
  readonly memberColumns = ['name', 'email', 'role'];
  readonly relColumns = ['from', 'type', 'to'];

  memberForm = { userId: '' };
  relForm = { fromMemberId: '', toMemberId: '', type: 'Parent' as RelationshipType };

  // Users in the org who aren't already members of this household.
  addableUsers = computed(() => {
    const memberUserIds = new Set(this.household()?.members.map((m) => m.userId) ?? []);
    return this.allUsers().filter((u) => !memberUserIds.has(u.id));
  });

  constructor() {
    this.api.getUsers().subscribe((u) => this.allUsers.set(u));
    queueMicrotask(() => this.load());
  }

  load(): void {
    this.api.getHousehold(this.id()).subscribe((h) => this.household.set(h));
  }

  addMember(): void {
    this.error.set(null);
    this.api.addMember(this.id(), this.memberForm.userId).subscribe({
      next: () => {
        this.memberForm.userId = '';
        this.load();
      },
      error: (err) => this.error.set(err?.error?.title ?? 'Could not add member.'),
    });
  }

  addRelationship(): void {
    this.error.set(null);
    this.api.addRelationship(this.id(), { ...this.relForm }).subscribe({
      next: () => {
        this.relForm = { fromMemberId: '', toMemberId: '', type: 'Parent' };
        this.load();
      },
      error: (err) => this.error.set(err?.error?.title ?? 'Could not add relationship.'),
    });
  }
}
