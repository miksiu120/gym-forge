import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
  CreateTrainingPlan,
  CompleteTrainingUnit,
  DueTrainingUnit,
  TrainingUnit,
  TrainingPlanSummary,
  TrainingStatistics,
} from '../../interfaces/training.interfaces';

@Injectable({ providedIn: 'root' })
export class TrainingPlanService {
  private readonly apiUrl = `${environment.apiUrl}/training-plans`;

  constructor(private readonly http: HttpClient) {}

  create(plan: CreateTrainingPlan): Observable<TrainingPlanSummary> {
    return this.http.post<TrainingPlanSummary>(this.apiUrl, plan, {
      headers: this.authHeaders(),
    });
  }

  getMine(): Observable<TrainingPlanSummary[]> {
    return this.http.get<TrainingPlanSummary[]>(this.apiUrl, {
      headers: this.authHeaders(),
    });
  }

  getDue(through: Date): Observable<DueTrainingUnit[]> {
    return this.http.get<DueTrainingUnit[]>(`${this.apiUrl}/due`, {
      headers: this.authHeaders(),
      params: { through: through.toISOString() },
    });
  }

  getStatistics(): Observable<TrainingStatistics> {
    return this.http.get<TrainingStatistics>(`${this.apiUrl}/statistics`, {
      headers: this.authHeaders(),
    });
  }

  getUnit(unitId: number): Observable<TrainingUnit> {
    return this.http.get<TrainingUnit>(`${this.apiUrl}/units/${unitId}`, {
      headers: this.authHeaders(),
    });
  }

  completeUnit(unitId: number, result: CompleteTrainingUnit): Observable<TrainingUnit> {
    return this.http.put<TrainingUnit>(`${this.apiUrl}/units/${unitId}/complete`, result, {
      headers: this.authHeaders(),
    });
  }

  private authHeaders(): HttpHeaders {
    const token = localStorage.getItem('session_token');
    return new HttpHeaders().set('Authorization', `Bearer ${token ?? ''}`);
  }
}
