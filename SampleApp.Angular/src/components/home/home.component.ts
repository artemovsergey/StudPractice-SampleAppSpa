import { Component, OnInit } from '@angular/core';
import User from '../../../models/user';
import { CommonModule } from '@angular/common';
import { UsersService } from '../../services/users.service';
import {MatTableModule} from '@angular/material/table'

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, MatTableModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent implements OnInit {

  users: User[] = []
  displayedColumns: string[] = ['id', 'name'];
 
  constructor(private userService: UsersService){
  }

  ngOnInit() {
    this.userService.getUsers().subscribe(r => this.users = r)
  }

}
