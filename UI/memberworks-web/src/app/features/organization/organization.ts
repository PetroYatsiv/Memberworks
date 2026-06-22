import { Component, inject, signal } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { ApiService } from '../../core/api.service';
import { Organization } from '../../core/models';

@Component({
  selector: 'app-organization',
  imports: [MatCardModule, MatChipsModule, MatIconModule],
  templateUrl: './organization.html',
})
export class OrganizationComponent {
  private api = inject(ApiService);
  org = signal<Organization | null>(null);

  constructor() {
    this.api.getMyOrganization().subscribe((o) => this.org.set(o));
  }
}
