import { IBase } from "./base";
import User from "./user";

export default interface Micropost extends IBase {
    name: string;
    content: string;
    userid: number;
    user?: User;
}