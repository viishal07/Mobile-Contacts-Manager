import { Component, OnInit, OnDestroy, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { FormControl, ReactiveFormsModule } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatDialog } from '@angular/material/dialog';
import { Subject, debounceTime, distinctUntilChanged, takeUntil } from 'rxjs';

import { ContactService } from '../../services/contact.service';
import { Contact, PagedResult } from '../../models/contact.model';
import { ContactListComponent } from '../../components/contact-list/contact-list.component';
import { ContactDetailComponent } from '../../components/contact-detail/contact-detail.component';
import { ContactFormDialogComponent } from '../../components/contact-form/contact-form-dialog.component';
import { ConfirmDialogComponent } from '../../components/confirm-dialog/confirm-dialog.component';

import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTooltipModule } from '@angular/material/tooltip';

@Component({
  selector: 'app-contacts-page',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ContactListComponent,
    ContactDetailComponent,
    MatInputModule,
    MatFormFieldModule,
    MatIconModule,
    MatButtonModule,
    MatProgressBarModule,
    MatPaginatorModule,
    MatTooltipModule
  ],
  templateUrl: './contacts-page.component.html',
  styleUrls: ['./contacts-page.component.scss']
})
export class ContactsPageComponent implements OnInit, OnDestroy {
  private contactService = inject(ContactService);
  private snackBar = inject(MatSnackBar);
  private dialog = inject(MatDialog);
  private route = inject(ActivatedRoute);
  private destroy$ = new Subject<void>();

  searchControl = new FormControl('');

  contacts = signal<Contact[]>([]);
  selectedContact = signal<Contact | null>(null);
  loading = signal(false);
  totalCount = signal(0);
  page = signal(0);
  pageSize = signal(20);
  favoritesOnly = signal(false);

  ngOnInit(): void {
    // Check if favorites route
    const isFav = this.route.snapshot.data['favoritesOnly'] === true;
    this.favoritesOnly.set(isFav);

    this.loadContacts();

    // Reactive search
    this.searchControl.valueChanges.pipe(
      debounceTime(350),
      distinctUntilChanged(),
      takeUntil(this.destroy$)
    ).subscribe(() => {
      this.page.set(0);
      this.loadContacts();
    });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  loadContacts(): void {
    this.loading.set(true);
    const params = {
      search: this.searchControl.value || undefined,
      favorite: this.favoritesOnly() ? true : undefined,
      page: this.page() + 1,
      pageSize: this.pageSize()
    };

    this.contactService.getAll(params).subscribe({
      next: (result: PagedResult<Contact>) => {
        this.contacts.set(result.items);
        this.totalCount.set(result.totalCount);
        this.loading.set(false);

        // Keep selected contact in sync
        if (this.selectedContact()) {
          const updated = result.items.find(c => c.id === this.selectedContact()!.id);
          this.selectedContact.set(updated ?? null);
        }
      },
      error: () => {
        this.loading.set(false);
        this.showSnack('Failed to load contacts', 'error');
      }
    });
  }

  selectContact(contact: Contact): void {
    this.selectedContact.set(contact);
  }

  openAddDialog(): void {
    const ref = this.dialog.open(ContactFormDialogComponent, {
      width: '480px',
      data: { contact: null }
    });

    ref.afterClosed().subscribe(result => {
      if (result) {
        this.contactService.create(result).subscribe({
          next: (created) => {
            this.loadContacts();
            this.selectedContact.set(created);
            this.showSnack('Contact created successfully!', 'success');
          },
          error: () => this.showSnack('Failed to create contact', 'error')
        });
      }
    });
  }

  openEditDialog(contact: Contact): void {
    const ref = this.dialog.open(ContactFormDialogComponent, {
      width: '480px',
      data: { contact }
    });

    ref.afterClosed().subscribe(result => {
      if (result) {
        this.contactService.update(contact.id, result).subscribe({
          next: (updated) => {
            this.loadContacts();
            this.selectedContact.set(updated);
            this.showSnack('Contact updated successfully!', 'success');
          },
          error: () => this.showSnack('Failed to update contact', 'error')
        });
      }
    });
  }

  openDeleteDialog(contact: Contact): void {
    const ref = this.dialog.open(ConfirmDialogComponent, {
      width: '380px',
      data: {
        title: 'Delete Contact',
        message: `Are you sure you want to delete ${contact.fullName}? This action cannot be undone.`
      }
    });

    ref.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.contactService.delete(contact.id).subscribe({
          next: () => {
            if (this.selectedContact()?.id === contact.id) {
              this.selectedContact.set(null);
            }
            this.loadContacts();
            this.showSnack('Contact deleted', 'success');
          },
          error: () => this.showSnack('Failed to delete contact', 'error')
        });
      }
    });
  }

  onPageChange(event: PageEvent): void {
    this.page.set(event.pageIndex);
    this.pageSize.set(event.pageSize);
    this.loadContacts();
  }

  clearSearch(): void {
    this.searchControl.setValue('');
  }

  private showSnack(message: string, type: 'success' | 'error'): void {
    this.snackBar.open(message, '✕', {
      duration: 3500,
      panelClass: type === 'error' ? ['snack-error'] : ['snack-success'],
      horizontalPosition: 'end',
      verticalPosition: 'bottom'
    });
  }
}
