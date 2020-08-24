import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { Divida } from '../models/Divida';

@Injectable({
  providedIn: 'root'
})
export class HomeService {

  url = `${environment.baseUrl}divida/get`;

  constructor(private http: HttpClient) { }

  getDividas(): Observable<Divida[]> {
    return this.http.get<Divida[]>(`${this.url}`)
  }
}
