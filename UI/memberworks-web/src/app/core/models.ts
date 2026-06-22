export type ApplicationRole = 'Member' | 'OrgAdmin';

export interface CurrentUser {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  organizationId: string;
  organizationName: string;
  role: ApplicationRole;
}

export interface AuthResponse {
  token: string;
  user: CurrentUser;
}

export interface Organization {
  id: string;
  name: string;
  userCount: number;
  householdCount: number;
}

export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  role: ApplicationRole;
}

export interface HouseholdSummary {
  id: string;
  name: string;
  primaryUserId: string;
  primaryUserName: string;
  memberCount: number;
}

export interface HouseholdMember {
  memberId: string;
  userId: string;
  fullName: string;
  email: string;
  isPrimary: boolean;
}

export interface Relationship {
  id: string;
  fromMemberId: string;
  fromName: string;
  toMemberId: string;
  toName: string;
  type: RelationshipType;
  typeLabel: string;
}

export interface HouseholdDetail {
  id: string;
  name: string;
  primaryUserId: string;
  members: HouseholdMember[];
  relationships: Relationship[];
}

export type RelationshipType =
  | 'Spouse' | 'Parent' | 'Child' | 'Sibling' | 'Grandparent'
  | 'Grandchild' | 'Guardian' | 'Dependent' | 'Partner' | 'Other';

export const RELATIONSHIP_TYPES: RelationshipType[] = [
  'Spouse', 'Parent', 'Child', 'Sibling', 'Grandparent',
  'Grandchild', 'Guardian', 'Dependent', 'Partner', 'Other',
];
