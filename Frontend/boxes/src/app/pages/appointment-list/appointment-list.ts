import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { AppointmentsService } from '../../core/services/appointments';
import { Appointment } from '../../core/models/appointment.model';
import { Observable, catchError, of } from 'rxjs';

@Component({
  selector: 'app-appointment-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule
  ],
  templateUrl: './appointment-list.html',
    styleUrl: './appointment-list.scss'  
})
export class AppointmentListComponent {

  appointments$: Observable<Appointment[]>;

  error: string | null = null;

  constructor(
    private appointmentsService: AppointmentsService,
    private router: Router
  ) {
    this.appointments$ = this.appointmentsService.getAppointments().pipe(
      catchError(err => {
        this.error = 'No se pudieron cargar los turnos. Por favor, intente nuevamente.';
        return of([]);
      })
    );
  }

  navigateToCreate(): void {
    this.router.navigate(['/new']);
  }

  trackById = (_: number, appointment: Appointment) => appointment.id;
}
