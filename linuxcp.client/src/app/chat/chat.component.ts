import { Component } from '@angular/core';
import { ChatService } from '../chat.service';

interface ChatMessage {
  sender: 'user' | 'bot';
  text: string;
}

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html'
})
export class ChatComponent {
  userId: number = 1; // можно задать из логики или хранить в localStorage
  message: string = '';
  messages: ChatMessage[] = [];

  constructor(private chatService: ChatService) { }

  getCurrentTime(): string {
    return new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
  }

  send() {
    if (!this.message.trim()) return;

    const userMessage: ChatMessage = { sender: 'user', text: this.message };
    this.messages.push(userMessage);

    this.chatService.sendMessage(this.userId, this.message).subscribe({
      next: (response: string) => {
        // Обрезаем первые 13 символов и последний 1 символ
        let cleanedResponse = response;
        if (response.startsWith('{"response":"') && response.endsWith('"}')) {
          cleanedResponse = response.slice(13, -2);
        }
        const botMessage: ChatMessage = { sender: 'bot', text: cleanedResponse };
        this.messages.push(botMessage);
      },
      error: (err) => {
        this.messages.push({ sender: 'bot', text: 'Ошибка отправки запроса.' });
        console.error(err);
      }
    });

    this.message = '';
  }
}
