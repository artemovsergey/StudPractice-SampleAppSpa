import { Observable } from "rxjs";

export interface IRepository<T> {

    get(id: number): Observable<T>;

    create(object: T): Observable<T>;
    
    del(id: number): Observable<boolean>;
    
    update(t: T): Observable<T>;

    getAll(): Observable<T[]>

}