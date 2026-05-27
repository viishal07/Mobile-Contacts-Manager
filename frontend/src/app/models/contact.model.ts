export interface Contact {
  id: string;
  firstName: string;
  lastName?: string;
  email?: string;
  phoneNumber?: string;
  company?: string;
  address?: string;
  favorite: boolean;
  createdAt: string;
  updatedAt: string;
  fullName: string;
  initials: string;
}

export interface CreateContactDto {
  firstName: string;
  lastName?: string;
  email?: string;
  phoneNumber?: string;
  company?: string;
  address?: string;
  favorite: boolean;
}

export interface UpdateContactDto {
  firstName: string;
  lastName?: string;
  email?: string;
  phoneNumber?: string;
  company?: string;
  address?: string;
  favorite: boolean;
}

export interface ContactQueryParams {
  search?: string;
  favorite?: boolean;
  page?: number;
  pageSize?: number;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}
