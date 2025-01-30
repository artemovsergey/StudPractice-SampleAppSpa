import { Component, inject, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { take } from 'rxjs';
import { UsersService } from '../../services/users.service';
import Micropost from '../../models/micropsots';
import User from '../../models/user';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.scss'
})
export class ProfileComponent implements OnInit {

  // @ts-ignore
  user: User
  microposts: Micropost[] = []

  authService = inject(AuthService)
  userService = inject(UsersService)
  activateRoute = inject(ActivatedRoute)

  ngOnInit() {
    this.user.id = this.activateRoute.snapshot.params["id"];
    this.authService.currentUser$.pipe(take(1)).subscribe(r => {this.user = r;this.getMicroposts(r.id)})
  }

  getMicroposts(id: number){
    this.userService.getUserWithMicropost(this.user.id).subscribe(r => this.microposts = r.microposts)
  }


}
