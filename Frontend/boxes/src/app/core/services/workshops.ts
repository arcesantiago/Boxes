import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Workshop {
  id: number;
  name: string | null;
  email: string | null;
  phone: string | null;
  active: boolean;
  formattedAddress: string | null;
}

@Injectable({ providedIn: 'root' })
export class WorkshopsService {
  private apiUrl = 'http://localhost:5111/api/Workshops';

  constructor(private http: HttpClient) {}

  getWorkshops(): Observable<Workshop[]> {
    return this.http.get<Workshop[]>(this.apiUrl);
  }
}
