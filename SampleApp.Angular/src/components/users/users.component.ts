import { Component } from '@angular/core';
import User from '../../../models/user';
import { UsersService } from '../../services/users.service';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [MatTableModule],
  templateUrl: './users.component.html',
  styleUrl: './users.component.scss'
})
export class UsersComponent {

  users: User[] = []
  displayedColumns: string[] = ['id', 'login'];
 
  constructor(private userService: UsersService){
  }

  ngOnInit() {
    this.userService.getUsers().subscribe(r => this.users = r)
  }

}
