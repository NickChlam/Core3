import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { catchError } from 'rxjs/operators';
import { of, Observable } from 'rxjs';
import { AuthService } from '../_services/auth.service';

@Injectable()
export class MemberLikeeResolver implements Resolve<User> {
    constructor(private userService: UserService, private router: Router, private alertify: AlertifyService, private authService: AuthService) {}
    pageNumber = 1;
    pageSize = 10;

    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        // get the currently logged in user by using decoded token nameid
        return this.userService.getUsers(this.pageNumber, this.pageSize, undefined, true)
            .pipe(
                catchError(err => {
                    this.alertify.error('Problem getting the your data');
                    this.router.navigate(['/members']);
                    return of(null);
                })
            )
    }

 
}
