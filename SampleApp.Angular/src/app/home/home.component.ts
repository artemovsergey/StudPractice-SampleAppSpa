import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {MatTableModule} from '@angular/material/table';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-home',
  imports: [CommonModule, MatTableModule],
  standalone: true,
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {

  currentUser: any = {}
  authService = inject(AuthService)

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(r => this.currentUser = r)
  }

}
