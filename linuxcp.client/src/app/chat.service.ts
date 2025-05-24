// chat.service.ts
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

interface ChatRequest {
  prompt: string;
}

@Injectable({ providedIn: 'root' })
export class ChatService {
  private baseUrl = 'https://localhost:7053/api/chat';

  constructor(private http: HttpClient) { }

  sendMessage(userId: number, prompt: string): Observable<string> {
    const params = new HttpParams().set('UserId', userId.toString());
    const body: ChatRequest = { prompt };
    return this.http.post(this.baseUrl + '/Send', body, {
      params,
      responseType: 'text' // предполагаем, что ответ — обычный текст
    });
  }
}
