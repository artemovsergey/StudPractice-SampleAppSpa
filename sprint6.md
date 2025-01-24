# Валидация в формах Angular

## Настройка стилей для формы регистрации

```css
.sign {
    display:flex;
    justify-content: center;

    height: 100%;

    border-width: 0px;
    border-color: blue;
    border-style: dashed;

    margin: 5px;
    padding-bottom: 30px;
  }

.sign-grid {
    display: grid;
    place-items: center;
    height: 70vh;
    }

button{
    width: 100%
}

```


## Блок кода для отладочной информации

```html
        <div *ngIf="signForm.invalid">
            <p>
                Значение формы: {{signForm.value | json}}
            </p>
            <p>
                Статус формы: {{signForm.status| json}}
            </p>
            <p>
                Проверка логина: {{signForm.hasError('checkConfirmLogin') | json }}
            </p>
        </div>
```

- объявите в компоненте SignComponent переменную ```sign: FormGroup```
- в конструкторе компонента мы на поля с именем login и password привязываем встроенные валидаторы, а также на всю форму привязываем наш собственный валидатор для проверки логина пользователя.

```ts
    this.signForm = new FormGroup({
      login: new FormControl("", [Validators.required]),
      password: new FormControl("", [Validators.maxLength(8)])
    }, {validators: [this.checkLoginValidator]})
```

далее приводится метод валидатора

```ts
private checkLoginValidator : ValidatorFn = (control: AbstractControl): ValidationErrors | null => {
    const login = control.get('login');    
    if(login?.value == "admin"){
      console.log("логин: ", login!.value)
      return { "checkConfirmLogin": "Недопустимый логин пользователя!" }
    }
    return null
}
```

для примера можно посмотреть как работают валидаторы для отдельного контрола в форме, тогда их надо привязывать отдельно

```ts
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
```

- для включения валидации на форме надо в теге form написать атрибту ```[formGroup]="signForm"```, также убрать явную привязку данных в input.
  
Теперь input будет выглядеть так:

```html
<input matInput formControlName="login" type="text" autocomplete="off" />
```

Также сделайте для других полей, которым нужна валидация

Ниже приводится блок кода, который показывает информацию, если срабатывают валидаторы

```html
        <div class="alert" *ngIf="signForm.hasError('checkConfirmLogin') && (signForm.touched || signForm.dirty)">
            <label>
                Недопустимый логин пользователя!
            </label>
        </div>
```

**Замечание**: раньше мы передавали в метод sign(this.model), а теперь у нас нет модели, а есть состояние формы this.signForm.value

# SnackBar

- в компоненте SignComponent объявите переменную ```snackBar: MatSnackBar = inject(MatSnackBar)```. Для этого в секцию imports поместите модуль ```MatSnackBarModule````.

- далее, создайте метод

```ts
  openSnackBar(message: string) {
    this.snackBar.open(message, "", { duration: 3000});
  }
```

- теперь можно этот метод вызвать, например, при успешной регистрации пользователя со значением параметра ```message``` = "Пользователь успешно зарегистрирован!".

# Пакет ngx-toastr

- установите пакет, добавив в ```package.json``` новую строку в секцию ```dependencies``` под названием  "ngx-toastr": "^19.0.0"
- выполните в командной строке в проекте Angular команду: npm install
- в ```app.config.ts``` в секцию providers добавьте новый пункт 

```ts

import { provideToastr } from 'ngx-toastr';

provideToastr({
                timeOut: 10000,
                positionClass: 'toast-bottom-right',
                preventDuplicates: true,
              })
...


```

- в файле ```angular.json``` в секцию ```styles``` добавьте стили "./node_modules/ngx-toastr/toastr.css"
- теперь вы можете внедрять сервиc всплывающих сообщений в компоненты. Так, например, для работы в ```SignComponent``` нам надо запросить объект в конструкторе


```
constructor(private toast: ToastrService)
```

предварительно импортировав данный класс ```import { ToastrService } from 'ngx-toastr';```

- далее, при успешной регистрации можно показать пользователю сообщение путем вызова метода ```this.toast.success("Пользователь зарегистрирован")```

# Spinner loading

- установите пакет "ngx-spinner": "^17.0.0",

- реализуйте сервис для отслеживания загрузки

```ts
import { Injectable } from '@angular/core';
import { NgxSpinnerService} from 'ngx-spinner'

@Injectable({
  providedIn: 'root'
})


export class BusyService {

  busyRequestCount = 0

  constructor(private busyService: NgxSpinnerService) { }

  busy(){
    this.busyRequestCount++;
    this.busyService.show(undefined, {
      type: "line-scale-party",
      bdColor: 'rgba(255,255,255,0)',
      color:'rgb(63,81,181)'
    })
  }


  idle(){
    this.busyRequestCount--;
    if(this.busyRequestCount <= 0){
      this.busyRequestCount = 0;
      this.busyService.hide()
    }
  }
}
```

- создайте перехватчик loadingInterceptor

```ts
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { delay, finalize } from 'rxjs';
import { BusyService } from '../services/busy.service';

export const loadingInterceptor: HttpInterceptorFn = (req, next) => {

  const loadingService = inject(BusyService);

  loadingService.busy();

  return next(req).pipe(delay(1000), finalize(() =>  { loadingService.idle()}));
};
```

- зарегистрируйте перехватчик в app.config.ts

```ts
provideHttpClient(
    withInterceptors([loadingInterceptor]),
),
```

- включите спиннер в шаблон главного компонента, импортировав ```NgSpinnerModule``` в компоненте app.


```html
<ngx-spinner type="line-scale-party">
    <h3> Loading ... </h3>
</ngx-spinner>

<app-header/>
<router-outlet/>
```

- настройте стили для спиннера в файле angular.json

```json
"styles": [
              "@angular/material/prebuilt-themes/indigo-pink.css",
              "./node_modules/ngx-toastr/toastr.css",
              "src/styles.scss",
               "./node_modules/ngx-spinner/animations/line-scale-party.css"
            ]
```


# Interceptors. Обработка ошибок

- создайте перехватчик для глобальной обработки ошибок

```ts
import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
// import { ToastrService } from 'ngx-toastr';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  
  const router = inject(Router);
  // const toast =  inject(ToastrService);
   
  return next(req).pipe(
    catchError(e => {
      if(e){
        console.log(e)
        switch(e.status){
          case 400:
            if(e.error.errors){
              const modalStateErrors = [];
              for(const key in e.error.errors){
                if(e.error.errors[key]){
                  modalStateErrors.push(e.error.errors[key])
                }
              }

              // toast.error(modalStateErrors.flat().toString())
              throw modalStateErrors.flat();
            } 
            else{
              // toast.error(e.statusText + ": " + e.error.message, e.status)
            }
            break;

          case 401:
            // toast.error(e.statusText, e.status)
            break;
          case 404:
            // toast.error(e.statusText, e.status)
            router.navigate(["/not-found"]);
            break;
          case 500:
            const navigationExtras: NavigationExtras = {state: {error: e.error}}
            // toast.error(e.statusText, e.status)
            router.navigateByUrl('/error-server', navigationExtras);
            break;    
          default:
            // toast.error("Произошла непредвиденная ошибка");
            console.log(e);
            break;
        }
      }
      return throwError(() => e)
    })
  );
};

```

- зарегистрируйте перехватчик ```errorInterceptor```

app.config.ts
```ts
    provideHttpClient(
      withInterceptors([errorInterceptor, loadingInterceptor]),
    ),
```


# # Interceptors. Обработка jwt токена

- создайте перехватчик ```jwtInterceptor``` и зарегистрируйте его.

```ts
import { HttpInterceptorFn } from '@angular/common/http';
import { UsersService } from '../services/users.service';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { take } from 'rxjs';

export const jwtInterceptor: HttpInterceptorFn = (req, next) => {
  
  const authService = inject(AuthService);
  let currentUser: any;
  authService.currentUser$.pipe(take(1)).subscribe(u => currentUser = u)

  if(currentUser){
    req = req.clone({
      setHeaders:{
        Authorization: `Bearer ${currentUser.token}`
      }
    })
  }

  return next(req);
};

```

# Guards

- создайте защитник для маршрутов

```
ng g guard auth
```

```ts
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { ToastrService } from 'ngx-toastr';
import { map, Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})

export class authGuard implements CanActivate{
  
  // guard автоматически подписывается на Observable
  constructor(private authService: AuthService, 
              private toast: ToastrService,
              private router: Router){ 
  }

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {

    // const storedUser = JSON.parse(localStorage.getItem("user")!);
    // if (storedUser) {
    //   this.authService.currentUserSource.next(storedUser);
    // }
    // else{
    //   this.authService.currentUserSource.next(null!);
    // }

    return this.authService.currentUser$.pipe(
      map(user => {
        
        if (!user) {
          this.toast.error("Пользователь не авторизован");
          this.router.navigate(["/sign"], { queryParams: { returnUrl: state.url } });
          return false;
        }
        
        return true;
      })

    );
  }
}
```

- подключите защитника на главный маршрут

app.routes.ts
```ts
export const routes: Routes = [
    {
        path:'',
        runGuardsAndResolvers: "always",
        canActivate:[authGuard],
        children:[
            { path: 'profile', component: ProfileComponent},
            { path: 'user/edit', component: EditUserComponent},
            { path: 'users', component: UsersComponent },
            // { path: 'users/:id', component: UserComponent },
        ]
    },

    { path: 'home', component: HomeComponent },
    { path: 'auth', component: AuthComponent },
    { path: 'not-found', component: NotFountComponent },
    { path: 'error-server', component: ErrorServerComponent },
    { path: 'sign', component: SignComponent },
    { path: "**", component: NotFountComponent, pathMatch: 'full'}

];
```

# Редактирование пользователя

- создайте компоннет editUser

```ts
@Component({
  selector: 'app-edit-user',
  standalone: true,
  imports: [CommonModule, FormsModule, MatInputModule, MatFormField, MatLabel, MatIcon, MatButton],
  templateUrl: './edit-user.component.html',
  styleUrl: './edit-user.component.scss'
})
export class EditUserComponent {

  @ViewChild('editForm') editForm!: NgForm

  user!: User
  constructor(private authService: AuthService,
    private toast: ToastrService,
    private userService: UsersService) {
    this.authService.currentUser$.pipe(take(1)).subscribe(u => this.user = u)
  }

  @HostListener('window:beforeunload',['event']) unloadNotification($event: any){
    if(this.editForm.dirty){
      $event.returnValue = true;
    }
  }

  updateUser(){
    console.log(this.user)
    this.userService.update(this.user).subscribe(r => console.log(r))
    // this.toast.success("Пользователь обновлен!")
    // this.editForm.reset()
  }

}
```

- в шаблоне компонента редактирования пользователя добавьте следующий код:


```html
<div class="auth">
  <h1> Редактирование </h1>
  <form #editForm="ngForm" id="editForm" (submit)="updateUser()" autocomplete="off" >
    <!-- <input hidden name="id" type="number" [(ngModel)]="user.id"/> -->
    <p>
      <mat-form-field>
        <mat-label>Login</mat-label>
        <input matInput name="login" type="text" [(ngModel)] = "user.login" name="login" autocomplete="off"  />
      </mat-form-field>
    </p>
    <p>
      <mat-form-field>
        <mat-label>Password</mat-label>
        <input matInput name="password" type="password" [(ngModel)] ="user.password" name="password" autocomplete="off" />
      </mat-form-field>
    </p>

    <button mat-flat-button form="editForm" color="primary" type="submit" [disabled]="!editForm.dirty" >
      <mat-icon>edit</mat-icon>
       Обновить
    </button>

  </form>
</div>
```


# Предупреждения о несохранении данных при выходе

- создайте защитника

```ts
import { CanDeactivateFn } from '@angular/router';
import { EditUserComponent } from '../app/edit-user/edit-user.component';

export const preventUnsavedChangesGuard: CanDeactivateFn<EditUserComponent> = (component:EditUserComponent) => {

  if(component.editForm.dirty){
    return confirm("Вы хотите продолжить?")
  }

  return true;
};
```

# CanDeactivate

```canActivate``` проверяет возможность перехода на определенный компонент, а ```сanDeactivate``` проверяет возможность ухода с определенного компонента

- подключите проверку в маршутизатор

```ts
{ path: 'user/edit', component: EditUserComponent, canDeactivate:[preventUnsavedChangesGuard]}
```

# HttpParams

```html
  <a routerLink="/item/5" [queryParams]="{>product':'phone', 'price': 200}">Item 5</a>
```


# Рефакторинг. Shared Module
```ng g m shared --flat```

- создайте общий модуль, в который вынесите все общие модули для всех компонентов.


# Задание
- обработать исключение ```Npgsql.PostgresException``` в глобальном перехватчике ошибок, которое вылетает когда база данных не создана.