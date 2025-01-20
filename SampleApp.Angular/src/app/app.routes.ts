import { Routes } from "@angular/router";
import { HeaderComponent } from "./header/header.component";
import { UsersComponent } from "./users/users.component";
import { HomeComponent } from "./home/home.component";
import { AuthComponent } from "./auth/auth.component";
import { SignComponent } from "./sign/sign.component";

export const routes: Routes = [
    { path: 'sign', component: SignComponent },
    { path: 'auth', component: AuthComponent },
    { path: 'header', component: HeaderComponent },
    { path: 'users', component: UsersComponent },
    { path: 'home', component: HomeComponent },
    { path: '', component: HomeComponent },
];