import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';

@Component({
  selector: 'app-sign',
  standalone: true,
  imports: [CommonModule, FormsModule, MatInputModule, MatIconModule, MatButtonModule],
  templateUrl: './sign.component.html',
  styleUrl: './sign.component.scss'
})
export class SignComponent {

  model:any = {}
  router: Router = new Router()
  constructor(private authService: AuthService){}

  sign(){
    this.authService.register(this.model).subscribe({next: r => {this.router.navigate(["home"]) ; console.log(r)},                                                 error: e => console.log(e.error)})
  }


}
