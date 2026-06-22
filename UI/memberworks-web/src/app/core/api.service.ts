import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AppConfig } from './config';
import {
  ApplicationRole, HouseholdDetail, HouseholdSummary, Organization,
  RelationshipType, User,
} from './models';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private http = inject(HttpClient);
  private base = AppConfig.apiBaseUrl;

  // Organization
  getMyOrganization(): Observable<Organization> {
    return this.http.get<Organization>(`${this.base}/organizations/me`);
  }

  // Users
  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.base}/users`);
  }
  createUser(body: { email: string; firstName: string; lastName: string; role: ApplicationRole }) {
    return this.http.post<string>(`${this.base}/users`, body);
  }

  // Households
  getHouseholds(): Observable<HouseholdSummary[]> {
    return this.http.get<HouseholdSummary[]>(`${this.base}/households`);
  }
  getHousehold(id: string): Observable<HouseholdDetail> {
    return this.http.get<HouseholdDetail>(`${this.base}/households/${id}`);
  }
  createHousehold(body: { name: string; primaryUserId: string }) {
    return this.http.post<string>(`${this.base}/households`, body);
  }
  addMember(householdId: string, userId: string) {
    return this.http.post<string>(`${this.base}/households/${householdId}/members`, { userId });
  }
  addRelationship(
    householdId: string,
    body: { fromMemberId: string; toMemberId: string; type: RelationshipType },
  ) {
    return this.http.post<string>(`${this.base}/households/${householdId}/relationships`, body);
  }
}
