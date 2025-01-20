# Sprint 5 Register and Authentication in Angular

# Регистрация пользователя

- создайте компоненты ```header``` и ```users```. Для этого выполните команды: 

```
ng g c header
ng g c users
```

# Провайдер роутера

- проверьте конфигурацию для роутера

app.config.ts
```ts
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
```

- в коллекцию ```providers``` внесите новый элемент ```provideRouter(routes)```

# Роутинг

app.routes.ts
```ts
export const routes: Routes = [
    { path: 'header', component: HeaderComponent },
    { path: 'users', component: UsersComponent },
    { path: 'home', component: HomeComponent },
    { path: '', component: HomeComponent },
];
```

# Компонент router-outlet

- в шаблоне ``app`` внесите изменения. Также импортируйте компонент ```header``` в шаблон ```app.component.html```.

```html
<app-header/>
<router-outlet/>
```

# Тема для Angular Material

в ```angular.json``` в разделе ```styles``` подключите предустановленную тему:

```json
"styles": [
        "@angular/material/prebuilt-themes/indigo-pink.css",
        "src/styles.scss"
    ],
```

## Material Icons

- установка пакета для локальной разработки

- ```npm install material-design-icons-iconfont```

- подключение пакета в ```styles.scss```.

```@import '../node_modules/material-design-icons-iconfont/dist/material-design-icons.css';```

# Создание компонента header

- подключите в компоненте ```header``` модули:

```ts
imports: [CommonModule, MatIconModule, MatToolbarModule, MatButtonModule],
```

- в шаблоне компонента

```html
<mat-toolbar color="primary">

  <button mat-button >
    <mat-icon>
      home
    </mat-icon>
    Home
  </button>

</mat-toolbar>
```

# Список всех пользователей

- добавьте новую кнопку в ```mat-toolbar``` в ```header``` для отображения пользователей, а также примените маршрутизацию к этой кнопке к компоненту ```users``` по нажатию на кнопку. Для этого импортируйте модуль ```RouterLink``` в компонент ```header```.

```html
  <button mat-button [routerLink] = "['/users']">
      <mat-icon>
          people
      </mat-icon>
      Пользователи
  </button>
```

- сделайте ссылку на корневую страницу для кнопки с домом.

**Задание**: перенесите функционал вывода списка пользователей из компонента ```home``` в компонент ```users```. В компоненете ```home``` в шаблоне оставьте только приветствие пользователя.

# Создание компонента auth

- создайте компонент auth
- в шаблоне компонента ```auth``` создайте форму:

```html
<div class="auth">
    <h1> Авторизация </h1>
    <form (ngSubmit)= "login()">
        <p>
            <input placeholder="Login" name="login" [(ngModel)]="model.login"/>
        </p>
        <p>
            <input placeholder="Password" name="password" [(ngModel)]="model.password"/>
        </p>
       <button type="submit">Отправить</button>
    </form>
</div>
```

в компоненте ```auth``` создайте свойство ```model:any = {}``` и метод:

```ts
  login(){
    console.log(this.model)
  }
```

Также надо импортировать модуль ```FormModule``` для работы директивы ```[(ngModel)]```.

Проверьте работу формы в консоли браузера


# Подключение компонентов Angular Material

- замените тег ```input``` для логина и пароля:

```html
  <mat-form-field>
      <mat-label>Login</mat-label>
      <input matInput name="login" type="text" [(ngModel)] = "model.login" />
  </mat-form-field>
```

```html
    <mat-form-field>
        <mat-label>Password</mat-label>
        <input matInput name="password" type="password" [(ngModel)] ="model.password" />
    </mat-form-field>
```

- замените отображение кнопки

```html
    <button mat-flat-button color="primary" type="submit">
        <mat-icon>
          login
        </mat-icon>
        Войти
    </button>
```

- проверьте импорт в компоненте ```auth```: должны быть ```FormsModule, MatInputModule, MatFormFieldModule, MatIconModule```

- включить в app.config.ts провайдер анимации браузера ```provideAnimationsAsync()```

> **Замечание**: в консоли браузера может быть предупреждение про autocompele для полей ввода. Поставьте атрибут ```autocomplete="off"``` для полей ввода.


# Создание метода авторизации в API

- создайте метод в ```UsersController```. Чтобы его создать надо добавить сингнатуру в интерфейс и метод в ```UserRepository```.

```Csharp
    [HttpPost("Login")]
    public ActionResult Login(UserDto userDto){
        var user = _repo.FindUser(userDto.Login);
        return CheckPasswordHash(userDto, user);
    }
```

- проверку логики пароля вынесите в отдельный приватный метод

```Csharp
private ActionResult CheckPasswordHash(UserDto userDto, User user)
    {

        using var hmac = new HMACSHA256(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i])
            {
                return Unauthorized($"Неправильный пароль");
            }
        }

        return Ok(user);
    }
```

- проверьте работу метода в API

# AuthService

- создайте сервис ```auth.service.ts```:

```
ng g s ../services/auth
```

```ts
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  http = inject(HttpClient)
  currentUser$ = new ReplaySubject<User>(1);

  token: string = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoidXNlciIsImV4cCI6MTc2ODkzNzgyNX0.H5ELw1tzIEX_RdwZrLmEbkaDiLCukEtQWykp55U3-I0"

  login(model: any){
    return this.http.post<User>(`${environment.baseUrl}/Users/Login`, model, this.generateHeaders()).pipe(
      map((response: User) => {
        const user = response;
        if(user){
          localStorage.setItem("user",JSON.stringify(user))
          this.currentUser$.next(user)
          console.log(user)
        }
        else{
          console.log(response)
        }
      })
    )
  }

  private generateHeaders = () => {
    return {
      headers: new HttpHeaders({'Content-Type': 'application/json','Authorization': 'Bearer ' + this.token})
    }
  }

}
```

- в методе компонента ```auth``` вызовите метод сервиса ```authService```, предварительно запросив его в конструкторе компонента.

```ts
  login(){
      this.authService.login(this.model).subscribe({
        next: (v) => console.log(v),
        error: (e) => console.error(e),
        complete: () => console.info('complete') 
    })
  }
```

**Замечание**: Observable-объекты - это ленивые объекты, на которые надо подписаться, чтобы получить результат. После успешного ответа от API, мы сохраняем пользователя в ```localStorage``` браузера, сериализуя его в json.

- после успешной авторизации надо выполнить переход на компонент ```home```. Для этого создайте объект роутера:

```ts
router = inject(Router)
```
- добавьте логику перехода в методе ```login``` компонента ```auth```:

```ts
  login(){
      this.authService.login(this.model).subscribe({
        next: (v) => this.router.navigate(["home"]),
        error: (e) => console.error(e),
        complete: () => console.info('complete') 
    })
  }
```
- проверьте ```localStorage``` на предмет объекта по ключу ```user```


# Регистрация

- создайте новый компонент ```sign```

- добавьте в ```authService``` метод ```register```:

```ts
  register(model:any){
    return this.http.post(this.baseUrl +"Users/", model)
  }
```

- теперь в компоненте ```sign``` создайте метод ```sign```, который вызывает метод сервиса:

```ts
  sign(){
    this.authService.register(this.model).subscribe({next: r => console.log(r),                                                 error: e => console.log(e.error)})
  }
```

- в шаблоне компонента ```sign``` в форме примените метод ```sign``` одноименного компонента:

```html

<div class="sign">
    <h1> Регистрация </h1>
    <form (ngSubmit)= "sign()" autocomplete="off">
        <p>
            <mat-form-field>
                <mat-label>Login</mat-label>
                <input matInput name="login" type="text" [(ngModel)] = "model.login" autocomplete="off" />
            </mat-form-field>
        </p>
        <p>
            <mat-form-field>
                <mat-label>Password</mat-label>
                <input matInput name="password" type="password" [(ngModel)] ="model.password" autocomplete="off" />
            </mat-form-field>
        </p>
        <button color="primary" mat-flat-button type="submit">
            Создать
        </button>
    </form>
</div>
```

- проверьте импорт модулей в компоненте ```sign```

```ts
imports: [CommonModule, FormsModule, MatInputModule, MatIconModule, MatButtonModule]
```

**Задание**: при успешной регистрации перейдите на компонент авторизации.

# Условный рендеринг и работа с состоянием

- создайте кнопку выхода в шаблоне компонента ```header```

```html
    <button mat-button type="button">
        <mat-icon>
          logout
        </mat-icon>
        Выход
    </button>
```

- если пользователь авторизован, то надо показывать кнопку выхода и кнопку списка всех пользоватлей,а если нет, то не показывать кнопки. Примените директиву для условного рендеринга к атрибуту ```button```:

```ts
*ngIf="authService.currentUser$ | async"
```

- в сервисе ```authService``` напишите логику метода для выхода:

```ts
  logout(){
    localStorage.removeItem("user")
    this.currentUser$.next(null!);
  }
```

- далее метод будет выполняться на событии ```click```:

```html
(click)="authService.logout()"
```

> **Замечание**: объект ```authService``` в конструкторе должен быть ```public```, чтобы можно было применять методы сервиса в шаблоне компонента.

**Задание**: после выполения метода ```logout()``` надо перейти на компонент авторизации с помощью роутера. Настройте логику маршрутизации из сервиса ```AuthService```.

**Задание**: добавьте кнопку "Авторизация" в компонент ```header```.