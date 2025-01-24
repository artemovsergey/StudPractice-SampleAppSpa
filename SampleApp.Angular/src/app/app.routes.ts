import { Routes } from "@angular/router";
import { ProfileComponent } from "./profile/profile.component";
import { EditUserComponent } from "./edit-user/edit-user.component";
import { UsersComponent } from "./users/users.component";
import { HomeComponent } from "./home/home.component";
import { AuthComponent } from "./auth/auth.component";
import { NotFountComponent } from "./errors/not-fount/not-fount.component";
import { ErrorServerComponent } from "./errors/error-server/error-server.component";
import { SignComponent } from "./sign/sign.component";
import { authGuard } from "../guards/auth.guard";
import { preventUnsavedChangesGuard } from "../guards/prevent-unsaved-changes.guard";


export const routes: Routes = [
    {
        path:'',
        runGuardsAndResolvers: "always",
        canActivate:[authGuard],
        children:[
            { path: 'profile', component: ProfileComponent},
            { path: 'user/edit', component: EditUserComponent, canDeactivate:[preventUnsavedChangesGuard]},
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
