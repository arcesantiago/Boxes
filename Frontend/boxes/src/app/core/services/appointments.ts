import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Appointment, CreateAppointment } from '../models/appointment.model';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AppointmentsService {
  private apiUrl = 'http://localhost:5111/api/Appointments';

  constructor(private http: HttpClient) {}

  getAppointments(): Observable<Appointment[]> {
    return this.http.get<Appointment[]>(this.apiUrl);
  }

  createAppointment(appointment: CreateAppointment): Observable<number> {
    return this.http.post<number>(this.apiUrl, appointment);
  }
}
