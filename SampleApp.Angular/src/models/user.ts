import { IBase } from "./base";
import Micropost from "./micropsots";

export default interface User extends IBase {
    name: string;
    login: string;
    password: string;
    photo: string;
    microposts: Micropost[]
}