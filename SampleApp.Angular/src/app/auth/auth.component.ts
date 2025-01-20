import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input'
import { AuthService } from '../../services/auth.service';
import User from '../../models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-auth',
  imports: [CommonModule, MatInputModule, FormsModule, MatFormFieldModule, MatIconModule, MatButtonModule],
  templateUrl: './auth.component.html',
  styleUrl: './auth.component.scss'
})
export class AuthComponent implements OnInit {

  model: any = {}
  authService = inject(AuthService)
  router = inject(Router)
  currentUser!: User

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(r => {this.currentUser = r; console.log(r)})
  } 
  
  login(){
      this.authService.login(this.model).subscribe({
        next: (v) => this.router.navigate(["home"]),
        error: (e) => console.error(e),
        complete: () => console.info('complete') 
    })
  }
}
