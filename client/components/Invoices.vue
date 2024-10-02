<template>
    <div>
        <h1>Invoices</h1>
        <ul>
            <li v-for="invoice in invoices" :key="invoice.id">{{ invoice?.invoiceNumber }} - {{ invoice.totalAmount }}</li>
        </ul>
    </div>
</template>

<script setup lang="ts">
    defineProps({
        invoices: Array as PropType<any[]>
    });
</script>

<style scoped>
    h1 {
        margin-bottom: 20px;
    }
</style>