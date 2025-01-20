import { Component, inject } from '@angular/core';
import User from '../../models/user';
import { UsersService } from '../../services/users.service';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-users',
  imports: [MatTableModule],
  templateUrl: './users.component.html',
  styleUrl: './users.component.scss'
})
export class UsersComponent {
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
