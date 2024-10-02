import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { defineNuxtPlugin } from '#app';

export default defineNuxtPlugin(() => {
  const signalr: HubConnection = new HubConnectionBuilder()
    .withUrl('http://localhost:8080/notificationHub')
    .build();

    signalr.start().catch(err => console.error(err.toString()));

  return {
    provide: {
      signalr
    }
  };
});
