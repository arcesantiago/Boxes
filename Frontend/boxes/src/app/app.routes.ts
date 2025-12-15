import { Routes } from '@angular/router';
import { AppointmentListComponent } from './pages/appointment-list/appointment-list';
import { AppointmentFormComponent } from './pages/appointment-form/appointment-form';

export const routes: Routes = [
  { path: '', component: AppointmentListComponent },
  { path: 'new', component: AppointmentFormComponent }
];
