import { Component, ViewChild, ElementRef, AfterViewChecked } from '@angular/core';
import { ChatService } from '../chat.service';

interface ChatMessage {
  sender: 'user' | 'bot';
  text: string;
  timestamp: Date;
}

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements AfterViewChecked {
  @ViewChild('messagesContainer') private messagesContainer!: ElementRef;

  userId: number = 1;
  message: string = '';
  messages: ChatMessage[] = [];
  isLoading: boolean = false;

  constructor(private chatService: ChatService) { }

  ngAfterViewChecked() {
    this.scrollToBottom();
  }

  private scrollToBottom(): void {
    try {
      this.messagesContainer.nativeElement.scrollTop = this.messagesContainer.nativeElement.scrollHeight;
    } catch (err) {
      console.error('Scroll error:', err);
    }
  }

  decodeUnicodeString(input: string): string {
    // Сначала заменим суррогатные пары (например, \uD83D\uDE0A — смайлики)
    const surrogateDecoded = input.replace(
      /\\uD[89AB][0-9A-F]{2}\\uD[CDEF][0-9A-F]{2}/gi,
      (match: string): string => {
        const high = parseInt(match.substr(2, 4), 16);
        const low = parseInt(match.substr(8, 4), 16);
        const codePoint = (high - 0xd800) * 0x400 + (low - 0xdc00) + 0x10000;
        return String.fromCodePoint(codePoint);
      }
    );

    // Затем заменим обычные \uXXXX
    const fullyDecoded = surrogateDecoded.replace(
      /\\u([0-9A-F]{4})/gi,
      (_: string, hex: string): string => {
        return String.fromCharCode(parseInt(hex, 16));
      }
    );

    return fullyDecoded;
  }

  cleanMessage(text: string): string {
    const newtext = this.decodeUnicodeString(text)
    return newtext
      .replace(/<[^>]*>/g, '')
      .replace(/\\n/g, '\n')
      .replace(/\n{2,}/g, '\n\n')
      .replace(/\\"/g, '"')
      .replace(/^[\s\n]+|[\s\n]+$/g, '');
  }

  send() {
    if (!this.message.trim() || this.isLoading) return;

    // Добавляем сообщение пользователя
    const userMessage: ChatMessage = {
      sender: 'user',
      text: this.message,
      timestamp: new Date()
    };
    this.messages.push(userMessage);
    this.isLoading = true;
    const currentMessage = this.message;
    this.message = '';

    // Отправляем сообщение на сервер
    this.chatService.sendMessage(this.userId, currentMessage).subscribe({
      next: (response: string) => {
        let cleanedResponse = response;
        if (response.startsWith('{"response":"') && response.endsWith('"}')) {
          cleanedResponse = response.slice(13, -2);
        }

        const botMessage: ChatMessage = {
          sender: 'bot',
          text: this.cleanMessage(cleanedResponse),
          timestamp: new Date()
        };
        this.messages.push(botMessage);
        this.isLoading = false;
      },
      error: (err) => {
        this.messages.push({
          sender: 'bot',
          text: 'Произошла ошибка при обработке запроса. Пожалуйста, попробуйте позже.',
          timestamp: new Date()
        });
        console.error('Chat error:', err);
        this.isLoading = false;
      }
    });
  }
}
