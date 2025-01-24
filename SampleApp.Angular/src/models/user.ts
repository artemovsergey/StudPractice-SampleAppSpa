import { IBase } from "./base";

export default interface User extends IBase {
    name: string;
    login: string;
    password: string;
    photo: string;
}