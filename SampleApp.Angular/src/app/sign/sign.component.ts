import { Component, inject } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { AbstractControl, FormControl, FormGroup, FormsModule, ReactiveFormsModule, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';
import {MatSnackBar,MatSnackBarModule} from '@angular/material/snack-bar';


@Component({
  selector: 'app-sign',
  standalone: true,
  imports: [MatSnackBarModule, ReactiveFormsModule, CommonModule, FormsModule, MatInputModule, MatIconModule, MatButtonModule],
  templateUrl: './sign.component.html',
  styleUrl: './sign.component.scss'
})

export class SignComponent {

  snackBar: MatSnackBar = inject(MatSnackBar)
  signForm: FormGroup
  authService = inject(AuthService)
  router = inject(Router)

  constructor()
  {
    this.signForm = new FormGroup({
      login: new FormControl("", [Validators.required]),
      password: new FormControl("", [Validators.maxLength(8)])
    }, {validators: [this.checkLoginValidator]})

  }

  // snackBar
  openSnackBar(message: string) {
    this.snackBar.open(message, "", { duration: 3000});
  }

  // валидатор для формы
  private checkLoginValidator : ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    const login = control.get('login');
    
    if(login?.value == "admin"){
      console.log("логин: ", login!.value)
      return { "checkConfirmLogin": "Недопустимый логин пользователя!" }
    }

    return null

  }

  // валидатор для login
  private nameValidator(nameRe: RegExp): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
      const forbidden = nameRe.test(control.value);
      return forbidden ? {forbiddenName: {value: control.value}} : null;
    };
  }
  
  //валидатор для password
  private passwordLengthValidator = () : ValidatorFn => {
      return (control: AbstractControl): ValidationErrors | null => {
    
      if (control.value === "123" ) {
        return { "login = 123": true };
      }

      return control.value
      }
  }

  sign(){
    this.authService.register(this.signForm.value).subscribe({next: r => {this.router.navigate(["auth"]) ;  this.openSnackBar("Пользователь успешно зарегистрирован");    }})
  }


}
