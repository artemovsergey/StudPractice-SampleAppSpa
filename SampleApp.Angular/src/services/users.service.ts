import { inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of, Subject } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../environments/environment.development';
import User from '../models/user';
import { IUserRepository } from '../interfaces/IUserRepository';

@Injectable({
  providedIn: 'root'
})
export class UsersService implements IUserRepository {

  http = inject(HttpClient)

  // сервис отдает поток данных
  Users$ = new BehaviorSubject<User[]>([]);

//   getUsersByRole(roleId: number): Observable<User[]>{
//     return this.http.get<User[]>(`${environment.baseUrl}/role/${roleId}/Users`)
//   }

  getUserWithMicropost(id: number): Observable<User> {
    return this.http.get<User>(`${environment.baseUrl}/Users/${id}/microposts`)
  }

  getAll(): Observable<User[]> {
    return this.http.get<User[]>(`${environment.baseUrl}/Users`)
  }

  get(id: number): Observable<User> {
    return this.http.get<User>(`${environment.baseUrl}/Users/${id}`)
  }

  create(u: User): Observable<User> {
    return this.http.post<User>(`${environment.baseUrl}/Users`, u, this.generateHeaders())
  }

  del(id: number): Observable<boolean> {
    return this.http.delete<boolean>(`${environment.baseUrl}/Users/${id}`)
  }

  update(u: User): Observable<User> {
    return this.http.put<User>(`${environment.baseUrl}/Users`, u, this.generateHeaders())
  }

  private generateHeaders = () => {
    return {
      headers: new HttpHeaders({'Content-Type': 'application/json'})
    }
  }

  
}
