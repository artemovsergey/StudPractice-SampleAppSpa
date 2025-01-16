import { Component, inject, OnInit } from '@angular/core';
import User from '../../models/user';
import { UsersService } from '../../services/users.service';
import { CommonModule } from '@angular/common';
import {MatTableModule} from '@angular/material/table';

@Component({
  selector: 'app-home',
  imports: [CommonModule, MatTableModule],
  standalone: true,
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {

  users: User[] = []
  displayedColumns: string[] = ['id', 'name'];
  public usersService = inject(UsersService)

  ngOnInit() {
    this.usersService.getAll().subscribe({
      next: (v) => this.users = v,
      error: (e) => console.error(e),
      complete: () => console.info('complete') 
  })
  }
}
