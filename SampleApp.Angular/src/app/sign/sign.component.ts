import { Component, inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-sign',
  imports: [MatFormFieldModule, MatInputModule, FormsModule, MatButtonModule],
  templateUrl: './sign.component.html',
  styleUrl: './sign.component.scss'
})
export class SignComponent {

  model: any = {}
  authService = inject(AuthService)
  router = inject(Router)

  sign(){
    this.authService.register(this.model).subscribe({next: r => this.router.navigate(["auth"]),                                                 error: e => console.log(e.error)})
  }

}
