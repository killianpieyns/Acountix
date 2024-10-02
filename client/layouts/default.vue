<template>
    <Notification />
    <div class="layout">
        <Header />
        <main>
            <div class="container mx-auto">
                <slot />
            </div>
        </main>
        <Footer />
    </div>
</template>

<script setup>
</script>

<style scoped>
    main {
        margin: 20px;
        display: flex;
        flex-direction: column;
        flex-grow: 1;
    }
    .layout {
        display: flex;
        flex-direction: column;
        min-height: 100vh;
    }
</style>