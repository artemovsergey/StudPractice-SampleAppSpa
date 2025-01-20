import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../environments/environment.development';
import { map, ReplaySubject } from 'rxjs';
import User from '../models/user';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  currentUser$ = new ReplaySubject<User>(1);
  router = inject(Router)
  http = inject(HttpClient)
  token: string = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidXNlciIsImV4cCI6MTc2ODkzNzgyNX0.H5ELw1tzIEX_RdwZrLmEbkaDiLCukEtQWykp55U3-I0"

  login(model: any){
    return this.http.post<User>(`${environment.baseUrl}/Users/Login`, model, this.generateHeaders()).pipe(
      map((response: User) => {
        const user = response;
        if(user){
          localStorage.setItem("user",JSON.stringify(user))
          this.currentUser$.next(user)
          console.log(user)
        }
        else{
          console.log(response)
        }
      })
    )
  }

  register(model:any){
    return this.http.post<User>(`${environment.baseUrl}/` + "Users/", model)
  }

  logout(){
    localStorage.removeItem("user")
    this.currentUser$.next(null!)
    this.router.navigate(["auth"])
  }


  private generateHeaders = () => {
    return {
      headers: new HttpHeaders({'Content-Type': 'application/json','Authorization': 'Bearer ' + this.token})
    }
  }


}
