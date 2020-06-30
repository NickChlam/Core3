import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PaginatedResult, Pagination } from '../_models/pagination';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  users: User[];
  pagination: Pagination;
  userParams: any = {};
  likee = true;
  liker = false;

  
  constructor(private route: ActivatedRoute, private userService: UserService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.route.data.subscribe(data => {

      this.users = data.users.result;
      this.pagination = data.users.pagination
    });
  }

  loadUsers(page, pageSize, params, likee, liker){
    this.userService.getUsers(page, pageSize, params, likee, liker).subscribe( data => {
      this.users = data.result;
      this.pagination = data.pagination;
    }, err => {
      this.alertify.error(err)
    })
  }

  
  pageChanged(event: any){
    this.pagination.currentPage = event.page;
    this.loadUsers(this.pagination.currentPage, this.pagination.itemsPerPage, null, this.likee, this.liker);

  }

  ToggleLikee() {
    this.likee = true;
    this.liker = false;

    this.loadUsers(this.pagination.currentPage, this.pagination.itemsPerPage, undefined, this.likee, this.liker)
    
  }

  ToggleLiker() {
    this.likee = false;
    this.liker = true;
    this.loadUsers(this.pagination.currentPage, this.pagination.itemsPerPage, undefined, this.likee, this.liker)
    
  }

}
