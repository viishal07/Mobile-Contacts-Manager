import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatRippleModule } from '@angular/material/core';
import { Contact } from '../../models/contact.model';

@Component({
  selector: 'app-contact-list',
  standalone: true,
  imports: [CommonModule, MatIconModule, MatRippleModule],
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.scss']
})
export class ContactListComponent {
  @Input() contacts: Contact[] = [];
  @Input() selectedId: string | null = null;
  @Input() loading = false;
  @Output() contactSelected = new EventEmitter<Contact>();

  avatarColors = [
    '#6366f1', '#8b5cf6', '#ec4899', '#f97316',
    '#10b981', '#14b8a6', '#0ea5e9', '#f59e0b'
  ];

  getAvatarColor(name: string): string {
    let hash = 0;
    for (let i = 0; i < name.length; i++) {
      hash = name.charCodeAt(i) + ((hash << 5) - hash);
    }
    return this.avatarColors[Math.abs(hash) % this.avatarColors.length];
  }

  trackById(_: number, contact: Contact): string {
    return contact.id;
  }
}
