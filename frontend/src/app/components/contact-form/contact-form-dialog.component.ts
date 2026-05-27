import { Component, Inject, OnInit, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { Contact } from '../../models/contact.model';

export interface ContactFormData {
  contact: Contact | null;
}

@Component({
  selector: 'app-contact-form-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatSlideToggleModule
  ],
  templateUrl: './contact-form-dialog.component.html',
  styleUrls: ['./contact-form-dialog.component.scss']
})
export class ContactFormDialogComponent implements OnInit {
  form!: FormGroup;
  isEdit = false;

  avatarColors = [
    '#6366f1', '#8b5cf6', '#ec4899', '#f97316',
    '#10b981', '#14b8a6', '#0ea5e9', '#f59e0b'
  ];

  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<ContactFormDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ContactFormData
  ) {}

  ngOnInit(): void {
    this.isEdit = !!this.data.contact;

    this.form = this.fb.group({
      firstName: [this.data.contact?.firstName ?? '', [Validators.required, Validators.maxLength(100)]],
      lastName: [this.data.contact?.lastName ?? '', [Validators.maxLength(100)]],
      email: [this.data.contact?.email ?? '', [Validators.email, Validators.maxLength(200)]],
      phoneNumber: [this.data.contact?.phoneNumber ?? '', [Validators.maxLength(20)]],
      company: [this.data.contact?.company ?? '', [Validators.maxLength(200)]],
      address: [this.data.contact?.address ?? '', [Validators.maxLength(500)]],
      favorite: [this.data.contact?.favorite ?? false]
    });
  }

  get initials(): string {
    const f = this.form.get('firstName')?.value || '';
    const l = this.form.get('lastName')?.value || '';
    return `${f[0] || ''}${l[0] || ''}`.toUpperCase() || '?';
  }

  get avatarColor(): string {
    const name = `${this.form.get('firstName')?.value}${this.form.get('lastName')?.value}`;
    let hash = 0;
    for (let i = 0; i < name.length; i++) {
      hash = name.charCodeAt(i) + ((hash << 5) - hash);
    }
    return this.avatarColors[Math.abs(hash) % this.avatarColors.length];
  }

  submit(): void {
    if (this.form.valid) {
      this.dialogRef.close(this.form.value);
    } else {
      this.form.markAllAsTouched();
    }
  }

  cancel(): void {
    this.dialogRef.close();
  }
}
