import { Contact } from './contact.model';
import { Vehicle } from './vehicle.model';

export interface Appointment {
  id: number;
  placeId: number;
  appointmentAt: string;
  serviceType: string;
  contact: Contact;
  vehicle?: Vehicle;
  createdAt: string;
}

export interface CreateAppointment {
  placeId: number;
  appointmentAt: string;
  serviceType: string;
  contact: Contact;
  vehicle?: Vehicle;
}
