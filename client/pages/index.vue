<template>
    <div>
        <h1>{{ authenticated }}</h1>
        <Invoices :invoices="invoices" />
        <Button :click="() => navigateTo('/invoices/create')">Create Invoice</Button>
        <Button :click="fetchInvoices">Fetch Invoices</Button>
    </div>
</template>

<script setup lang="ts">
    const { status } = useAuth();
    const authenticated = computed(() => status.value === 'authenticated');
    const invoices = ref<any[]>([]);
    const fetchInvoices = async () => {
        try {
            const { data } = await useFetch<any[]>('/api/bff/invoices')
            invoices.value = data.value!;
            console.log(data.value)
        } catch (error) {
            console.error(error)
        }
    }
</script>