<template>
    <div class="container mx-auto p-4">
        <UCard v-if="invoice">
            <template #header>
                <h1>Invoice Details</h1>
            </template>
            <!-- Invoice details go here -->
            <p><strong>Invoice Number:</strong> {{ invoice.invoiceNumber }}</p>
            <p><strong>Customer:</strong> {{ invoice.customer.name }}</p>
            <p><strong>Total Amount:</strong> {{ invoice.totalAmount }}</p>
            <!-- Other invoice details -->

            <!-- Buttons -->
            <div>
                <UButton @click="sendInvoiceEmail">Send Invoice via Email</UButton>
            </div>
        </UCard>
        <div v-else>Loading invoice details...</div>
    </div>
</template>

<script setup lang="ts">
    const route = useRoute();
    const invoice = ref({} as any);
    const { $api } = useNuxtApp();

    const fetchInvoiceDetails = async () => {
        const invoiceId = route.params.id;
        const { data } = await useAPI<any>(`/invoices/${invoiceId}`);
        invoice.value = data.value;
    };

    const sendInvoiceEmail = async () => {
        const invoiceId = route.params.id;
        try {
            await $api(`/invoices/${invoiceId}/email`, { method: 'POST' });
            alert('Invoice sent successfully!');
        } catch (error) {
            console.error('Error sending invoice:', error);
            alert('Failed to send the invoice. Please try again.');
        }
    };

    fetchInvoiceDetails();
</script>

<style scoped>
    .container {
        max-width: 800px;
    }
</style>