<template>
    <div v-if="message" class="notification">
      {{ message }}
    </div>
  </template>
  
  <script setup lang="ts">
  const message = ref<string | null>(null);
  // const { $signalr } = useNuxtApp();
  
  // onMounted(() => {
  //   $signalr.on('ReceiveNotification', (msg: string) => {
  //     message.value = msg;
  //     setTimeout(() => message.value = null, 5000);
  //   });
  // });
  </script>
  
  <style>
  .notification {
    background-color: #4caf50;
    color: white;
    padding: 10px;
    position: fixed;
    top: 10px;
    right: 10px;
    z-index: 1000;
  }
  </style>
  