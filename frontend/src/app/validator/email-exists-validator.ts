import { Injectable } from "@angular/core";
import { UserService } from "../service/user/user-service";
import { AbstractControl, AsyncValidatorFn, ValidationErrors } from "@angular/forms";
import { catchError, debounceTime, distinctUntilChanged, map, Observable, of } from "rxjs";

@Injectable({providedIn: 'root'})
export class EmailExistsValidator {

    constructor(private userService: UserService) { }

    checkEmailExists(): AsyncValidatorFn {
        return (control: AbstractControl): Observable<ValidationErrors | null> => {
            if (!control.value) {
                return of(null);
            }

            return this.userService.findByEmail(control.value).pipe(
                debounceTime(500),
                distinctUntilChanged(),
                map(() => {
                    
                    return { emailExists: true };
                }),
                catchError((err) => {
                    if (err.status === 404) {
                        return of(null);
                    }

                    return of(null);
                })
            )
        };
    }
}
