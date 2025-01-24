import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatCard, MatCardActions, MatCardContent, MatCardHeader, MatCardSubtitle, MatCardTitle } from '@angular/material/card';
import { Router, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { UsersService } from '../../services/users.service';
import User from '../../models/user';

@Component({
  selector: 'app-usercard',
  standalone: true,
  imports: [MatButtonModule, MatCard, MatCardHeader, MatCardTitle, MatCardSubtitle, MatCardContent, MatCardActions, RouterModule],
  templateUrl: './usercard.component.html',
  styleUrl: './usercard.component.scss'
})
export class UsercardComponent {

  @Input() user!: User
  @Output() currentUserState = new EventEmitter<User>()
  
  constructor(public userService: UsersService, private router: Router){

  }

  giveUserUp(user: User) {
    this.currentUserState.emit(user)
  }

  delete(id: number){
    this.userService.del(id).subscribe(r => {console.log(r); this.router.navigate(["users"])})
  }

}
