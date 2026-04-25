import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Contact } from '../contact/contact';

type ContactModel = {
  name: string;
  avatarUrl?: string;
  fallbackEmoji: string;
  online: boolean;
};

@Component({
  selector: 'app-contact-list',
  imports: [CommonModule, Contact],
  templateUrl: './contact-list.html',
  styleUrl: './contact-list.scss',
})
export class ContactList {
  contacts: ContactModel[] = [
    {
      name: 'Maya Chen',
      fallbackEmoji: '👱🏻‍♀️',
      online: true,
    },
    {
      name: 'Jordan Park',
      fallbackEmoji: '👱',
      online: true,
    },
    {
      name: 'Theo Nakamura',
      fallbackEmoji: '👨🏽‍🦰',
      online: true,
    },
    {
      name: "Liam O'Connor",
      fallbackEmoji: '👨🏻‍💻',
      online: true,
    },
    {
      name: 'Eva Lindqvist',
      fallbackEmoji: '👩🏼‍💼',
      online: true,
    },
  ];
}
