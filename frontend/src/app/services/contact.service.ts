import { Injectable } from "@angular/core";
import { HttpClient, HttpParams } from "@angular/common/http";
import { Observable } from "rxjs";

import { environment } from "../../environments/environment";

import {
  Contact,
  CreateContactDto,
  UpdateContactDto,
  ContactQueryParams,
  PagedResult,
} from "../models/contact.model";

@Injectable({
  providedIn: "root",
})
export class ContactService {
  private apiUrl = `${environment.apiUrl}/contacts`;

  constructor(private http: HttpClient) {}

  // Get all contacts
  getAll(params: ContactQueryParams = {}): Observable<PagedResult<Contact>> {
    let httpParams = new HttpParams();

    if (params.search) {
      httpParams = httpParams.set("search", params.search);
    }

    if (params.favorite !== undefined) {
      httpParams = httpParams.set("favorite", params.favorite.toString());
    }

    if (params.page) {
      httpParams = httpParams.set("page", params.page.toString());
    }

    if (params.pageSize) {
      httpParams = httpParams.set("pageSize", params.pageSize.toString());
    }

    return this.http.get<PagedResult<Contact>>(this.apiUrl, {
      params: httpParams,
    });
  }

  // Get single contact
  getById(id: string): Observable<Contact> {
    return this.http.get<Contact>(`${this.apiUrl}/${id}`);
  }

  // Create contact
  create(dto: CreateContactDto): Observable<Contact> {
    console.log("Creating Contact:", dto);

    return this.http.post<Contact>(this.apiUrl, dto);
  }

  // Update contact
  update(id: string, dto: UpdateContactDto): Observable<Contact> {
    return this.http.put<Contact>(`${this.apiUrl}/${id}`, dto);
  }

  // Delete contact
  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
