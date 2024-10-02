<template>
    <div class="container mx-auto p-4">
        <UCard>
            <template #header>
                <h1>Real-Time Notifications</h1>
            </template>
            <ul>
                <li v-for="(notification, index) in notifications" :key="index">{{ notification }}</li>
            </ul>
        </UCard>
    </div>
</template>

<script setup lang="ts">
    const notifications = ref<string[]>([]);
    const { $signalr } = useNuxtApp();

    const addNotification = (message: string) => {
        notifications.value.push(message);
        console.log("Notification received:");
        console.log(message);
    };

    onMounted(() => {
        $signalr.on('ReceiveNotification', addNotification);
    });

    onBeforeUnmount(() => {
        $signalr.off('ReceiveNotification', addNotification);
    });
</script>

<style scoped>
    .container {
        max-width: 800px;
    }
</style>