import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators, AbstractControl } from '@angular/forms';
import { Router } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { WorkshopsService, Workshop } from '../../core/services/workshops';
import { AppointmentsService } from '../../core/services/appointments';
import { CreateAppointment } from '../../core/models/appointment.model';

@Component({
  selector: 'app-appointment-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatCardModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './appointment-form.html',
  styleUrl: './appointment-form.scss'
})
export class AppointmentFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private workshopsService = inject(WorkshopsService);
  private appointmentsService = inject(AppointmentsService);
  private router = inject(Router);

  workshops: Workshop[] = [];
  loading = false;
  error: string | null = null;
  maxYear = new Date().getFullYear() + 1;

  form = this.fb.group({
    placeId: ['', [Validators.required, this.workshopValidator]],
    appointmentAt: ['', [Validators.required, this.futureDateValidator]],
    serviceType: ['', [Validators.required, Validators.maxLength(200)]],
    contact: this.fb.group({
      name: ['', [Validators.required, Validators.maxLength(200)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(200)]],
      phone: ['']
    }),
    vehicle: this.fb.group({
      make: [''],
      model: [''],
      year: [''],
      licensePlate: ['']
    })
  });

  ngOnInit(): void {
    this.loadWorkshops();
  }

  loadWorkshops(): void {
    this.workshopsService.getWorkshops().subscribe({
      next: (data) => {
        this.workshops = data.filter(w => w.active);
      },
      error: (err) => {
        this.error = 'Error al cargar los talleres. Por favor, intente más tarde.';
        console.error('Error loading workshops:', err);
      }
    });
  }

  workshopValidator(control: AbstractControl): { [key: string]: any } | null {
    const value = control.value;
    if (!value || value === '') {
      return { required: true };
    }
    const numValue = Number(value);
    if (isNaN(numValue) || numValue <= 0) {
      return { invalidWorkshop: true };
    }
    return null;
  }

  futureDateValidator(control: AbstractControl): { [key: string]: any } | null {
    if (!control.value) {
      return null;
    }
    const selectedDate = new Date(control.value);
    const now = new Date();
    if (selectedDate <= now) {
      return { pastDate: true };
    }
    return null;
  }

  get appointmentAtControl() {
    return this.form.get('appointmentAt');
  }

  get contactGroup() {
    return this.form.get('contact');
  }

  get vehicleGroup() {
    return this.form.get('vehicle');
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.loading = true;
    this.error = null;

    const formValue = this.form.value;
    const appointmentAt = new Date(formValue.appointmentAt!);
    
    const appointment: CreateAppointment = {
      placeId: Number(formValue.placeId),
      appointmentAt: appointmentAt.toISOString(),
      serviceType: formValue.serviceType!,
      contact: {
        name: formValue.contact!.name!,
        email: formValue.contact!.email!,
        phone: formValue.contact!.phone || undefined
      },
      vehicle: formValue.vehicle!.make || formValue.vehicle!.model || formValue.vehicle!.year || formValue.vehicle!.licensePlate
        ? {
            make: formValue.vehicle!.make || undefined,
            model: formValue.vehicle!.model || undefined,
            year: formValue.vehicle!.year ? Number(formValue.vehicle!.year) : undefined,
            licensePlate: formValue.vehicle!.licensePlate || undefined
          }
        : undefined
    };

    this.appointmentsService.createAppointment(appointment).subscribe({
      next: () => {
        this.router.navigate(['/']);
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.message || 'Error al crear el turno. Por favor, intente nuevamente.';
        console.error('Error creating appointment:', err);
      }
    });
  }

  cancel(): void {
    this.router.navigate(['/']);
  }
}
