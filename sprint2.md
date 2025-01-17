# Sprint 2. Введение в Angular

- проверьте фиксацию изменений в master
- создайте ветку sprint2

# Создание проекта Angular

- установите инструмент командной строки Angular ```npm install -g @angular/cli@latest```

- создайте проект новый проект Angular с помощью команды ```ng new SampleApp.Angular --skip-tests```

Примечание: при создании проекта надо выбрать конфигурацию: 
- препроцессор scss - yes, 
- ssr - no
**Примечание**: по умолчанию выбор по большой букве


# Обзор архитектуры приложения Angular

- перейдите в проект с Angular и проверьте работоспособность сервера командой ```ng serve -o``` по адресу ```http://localhost:4200```

**Замечание**: при первом запуске не отправлять данные по статистике

- внесите в файл ```.gitignore``` (в Angular) файл ```.angular```

# Создание окружения

```ng g environments```

- добавьте базовый адрес бекенда в ```environment.development.ts```:

```ts
export const environment = {
    baseUrl: "http://localhost:5137/api"
};
```


# Создание модели данных

- в папке ```src``` создайте папку ```models```, в которой создайте файл ```user.ts```

- базовая модель
```ts
export interface IBase{
    id: number
}
```
- модель пользователя
```ts
import { IBase } from "./base";

export default interface User extends IBase {
    name: string;
}
```

# Интерфейсы

- в папке ```src``` создайте папку ```interfaces```, в которой создайте файл ```IRepository.ts```


- общий асинхронный интерфейс CRUD

```ts
import { Observable } from "rxjs";

export interface IRepository<T> {

    get(id: number): Observable<T>;

    create(object: T): Observable<T>;
    
    del(id: number): Observable<boolean>;
    
    update(t: T): Observable<T>;

    getAll(): Observable<T[]>

}
```

- интерфейс для пользователей
```ts
import { IRepository } from "./IRepository";
import User from "../models/user";

export interface IUserRepository extends IRepository<User>{

}
```


# Сервисы

- в папке ```src``` создайте папку ```services``` и выполните команду ```ng g s ../services/users```

В результате сгенерируется файл сервиса.

```ts
@Injectable({
  providedIn: 'root'
})
export class UsersService {
  
  constructor() { }

}
```

теперь надо реализовать в сервисе наши интерфейсы. Таким образом, сервис будет выглядеть так:


```ts
@Injectable({
  providedIn: 'root'
})
export class UsersService implements IUserRepository {

  http = inject(HttpClient)

  getAll(): Observable<User[]> {
      return this.http.get<User[]>(`${environment.baseUrl}/Users`)
  }

  get(id: number): Observable<User> {
    return this.http.get<User>(`${environment.baseUrl}/Users/${id}`)
  }

  create(u: User): Observable<User> {
    return this.http.post<User>(`${environment.baseUrl}/Users`, u, this.generateHeaders())
  }

  del(id: number): Observable<boolean> {
    return this.http.delete<boolean>(`${environment.baseUrl}/Users/${id}`)
  }

  update(u: User): Observable<User> {
    return this.http.put<User>(`${environment.baseUrl}/Users`, u, this.generateHeaders())
  }

  private generateHeaders = () => {
    return {
      headers: new HttpHeaders({'Content-Type': 'application/json'})
    }
  }

}
```


# Создание компонента

- создайте новый компонент ```home``` командой ```ng g c home```. 


# Рендеринг компонента в главном компоненте

В компоненте ```app``` импортируйте компонент ```home```

```ts
import { Component } from '@angular/core';
import { HomeComponent } from "../components/home/home.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [HomeComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  title = 'SampleApp.Angular';
}
```

Важно: при создании компонента через ng g c в зависимости от версии инструмента командной строки компоненты могут не содержать параметр автономности ```standalone: true,```. Убедитесь, что все компоненты содержат такой параметр.

В шаблоне компонента (app.component.html) ```app``` подключите новый компонент ```home```. Полностью удалите все, что там было.

**Примечание**. Удобно осуществлять навигацию по файлам в Visual Code не мышкой в обозревателе, а через Ctrl + P и поиском нужного файла.

app.component.html
```html
<app-home/>
```


# Настройка HttpClient

Найдите в проекте файл конфигурации главного компонента ```app.config.ts``` и замените в нем код. Здесь определяется провайдер для работы с http.

app.config.ts
```ts
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

export const appConfig: ApplicationConfig = {
  providers: [

            ...
            provideHttpClient(withInterceptorsFromDi())
            ...

            ]
};
```

# Observable. Подписка на запрос


Сделайте запрос к коллекции пользователей из API в компоненте ```home```;

home.component.ts
```ts
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import  User from '../../models/user'
import { UsersLocalService } from '../../services/userslocal.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})


export class HomeComponent implements OnInit {

  users: User[] = []
  usersService = inject(UsersService)

  ngOnInit() {
    this.usersService.getAll().subscribe({
      next: (v) => this.users = v,
      error: (e) => console.error(e),
      complete: () => console.info('complete') 
  })
  }
}
```

# Управляющие конструкции в шаблоне компонента

- в шаблоне компонента ```home``` поместите следующий код:

Директива ```*ngFor``` - это директива, которая является простым циклом for. Для включения этой функциональности надо импортировать в компонент ```home``` модуль ```CommonModule``` в секцию imports.

```html
<h3> Пользователи </h3>

<ul *ngFor="let user of users">
    <li> {{user.name}}</li>
</ul>
```

**Замечание**: в новых версиях Angular управление потоком выполняется с помощью функций

```
@for (u of users; track $index) {
    <ul>
        <li> {{u.name}}</li>
    </ul>
}
```

Но данные не будут выведены на экран. Посмотрите в консоль браузера.


# Cors. Подключение и конфигурация

По умолчанию политика браузера такова, что он не разрешает совершать запросы к ресурсу, который находится в другом домене, если принимающая сторона явно не разрешает это. То есть наше приложение Angular не сможет получить ответ от API, пока API не разрешит это.  Эта политика назыаветяс CORS (Cross-Origin Resource Sharing).

Теперь вернитесь в проект API и разрешите обращение к нему от всех источников и заголовков.

Program.cs
```Csharp
builder.Services.AddCors();
...
app.UseCors(o => o.AllowAnyOrigin().AllowAnyHeader());
```

# Установка пакета Angular Material

Выполните установку пакета командой ```ng add @angular/material```. При установке вас попросят выбрать тему, типографию и анимацию. На все вопросы ответьте утвердительно.


# Вывод пользователей в виде таблицы 

В шаблоне компонента ```home``` применяются элементы из библиотеки пользовательского интерфейса ```Angular Material```. Для отображения таблицы нужно импортировать модель ```MatTableModule```,

```ts
import {MatTableModule} from '@angular/material/table';
```

а также объявить новое свойство ```displayedColumns``` в компоненте ```home```, которое нужно для отображение таблицы.

```ts
displayedColumns: string[] = ['id', 'name'];
```

home.component.html
```html
<h1> {{title}}</h1>

<table mat-table [dataSource]="users" class="mat-elevation-z8">
  <ng-container matColumnDef="id">
    <th mat-header-cell *matHeaderCellDef> Id </th>
    <td mat-cell *matCellDef="let element"> {{element.id}} </td>
  </ng-container>

  <ng-container matColumnDef="name">
    <th mat-header-cell *matHeaderCellDef> Name </th>
    <td mat-cell *matCellDef="let element"> {{element.name}} </td>
  </ng-container>

  <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
  <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
</table>
```

- перезагрузите сервер


# Настройка https сертификата для localhost (опция)

## Создание центра сертификации

- создайте файл ```openssl.conf```

```
[req]
distinguished_name = req_distinguished_name
prompt = no
default_bits = 2048
default_md = sha256
[req_distinguished_name]
CN = localhost
emailAddress = test@git.scc
```

- выполните команду только в ```Git Bash``` в директории, где находится файл ```openssl.conf```:

```
openssl req -x509 -newkey rsa:4096 -days 365 -nodes -keyout root_ca.key -out root_ca.crt -config openssl.conf
```
На выходе генерируются два файла: ```root_ca.key``` и ```root_ca.crt```


# Генерация приватного ключа

```
openssl genrsa -out localhost.key 2048
```
На выходе генерируются ```localhost.key```


# Создание запроc на подписывание ключа к центру сертификации

```
openssl req -new -key localhost.key -out localhost.csr -config openssl.conf
```
На выходе генерируются ```localhost.csr```

# Подпись запроса нашим центром сертификации 

- создайте файл ```localhost.ext```

```
authorityKeyIdentifier=keyid,issuer
basicConstraints=CA:FALSE
subjectAltName=@alt_names
[alt_names]
DNS.1=localhost
DNS.2=backend
IP.1=127.0.0.1
IP.2= {id}
```
**Замечание**: вместо {ip} подставьте ip вашего компьютера


- выполните команду

openssl x509 -req \
-CA root_ca.crt \
-CAkey root_ca.key \
-in localhost.csr \
-out localhost.crt \
-days 365 \
-CAcreateserial \
-extfile localhost.ext

На выходе генерируются ```localhost.crt```

# Установить центр сертификации в Chrome

- Настройки -> Конфиденциальность и безопасность -> Безопасность -> Настроить сертификаты

- найдите доверенные центры сертификации и импортируйте файл ```localhost.crt```

![](http://192.168.4.90/asv/StudPractice-SampleAppSpa/raw/master/images/crt.png)


# Настройка ssl для localhost в приложении Angular:

- в файле ```angular.json``` вставьте в ```options``` в секцию ```serve``` c указанием пути к ssl сертификату и ключу.

angular.json
```json
   "options": {
            "sslCert": "./src/crt/localhost.crt",
            "sslKey": "./src/crt/localhost.key",
            "ssl": true
```

- запустите приложение ```npm run start``` или ```ng serve```. Приложение должно работать на https

**Примечание**:
В этой статье описывается подробно процесс создания самоподписанного сертфиката для localhost.
https://dzen.ru/a/ZQ4nAsQZ6GkuFw7_

Также есть путь более быстрого создания сертификата для localhost с приминением mkcert



Задание: настройте работу API по протоколу https. Добавьте адрес в Properties/launchSettings.json

```
  "applicationUrl": "http://localhost:5137;https://localhost:5138"
```

Всвязи с новым адресом API перенастройте фронтенд.


# Rebase sprint2 в master

- зафиксируйте sprint2
- перейдите в master (возможно, добавьте в gitignore .angular в проекте Angular)
- выполните команду git rebase sprint1