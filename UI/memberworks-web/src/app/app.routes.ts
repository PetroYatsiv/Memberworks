import { Routes } from '@angular/router';
import { authGuard } from './core/auth.guard';

export const routes: Routes = [
  { path: 'login', loadComponent: () => import('./features/login/login').then((m) => m.LoginComponent) },
  {
    path: '',
    canActivate: [authGuard],
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'households' },
      {
        path: 'organization',
        loadComponent: () => import('./features/organization/organization').then((m) => m.OrganizationComponent),
      },
      {
        path: 'users',
        loadComponent: () => import('./features/users/users').then((m) => m.UsersComponent),
      },
      {
        path: 'households',
        loadComponent: () => import('./features/households/households').then((m) => m.HouseholdsComponent),
      },
      {
        path: 'households/:id',
        loadComponent: () =>
          import('./features/households/household-detail').then((m) => m.HouseholdDetailComponent),
      },
    ],
  },
  { path: '**', redirectTo: '' },
];
