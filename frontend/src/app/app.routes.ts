import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./layouts/main-layout/main-layout.component').then(m => m.MainLayoutComponent),
    children: [
      {
        path: '',
        loadComponent: () =>
          import('./pages/contacts-page/contacts-page.component').then(m => m.ContactsPageComponent)
      },
      {
        path: 'favorites',
        loadComponent: () =>
          import('./pages/contacts-page/contacts-page.component').then(m => m.ContactsPageComponent),
        data: { favoritesOnly: true }
      }
    ]
  },
  { path: '**', redirectTo: '' }
];
