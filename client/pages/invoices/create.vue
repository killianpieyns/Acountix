<template>
    <div class="container mx-auto p-4">
        <h1 class="mb-4">Create New Invoice</h1>
        <UForm :state="state" @submit="createInvoice" class="space-y-4">
            <UFormGroup label="Invoice Number">
                <UInput v-model="state.invoiceNumber" required />
            </UFormGroup>
            <UFormGroup label="Date">
                <UInput type="date" v-model="state.date" required />
            </UFormGroup>
            <UFormGroup label="Customer">
                <USelect :options="customers" v-model="state.customerId" />
            </UFormGroup>
            <UFormGroup label="Products">
                <USelect multiple :options="products" v-model="state.productIds" />
            </UFormGroup>
            <UFormGroup label="Discount">
                <UInput type="number" v-model="state.discount" />
            </UFormGroup>
            <UFormGroup label="Down Payment">
                <UInput type="number" v-model="state.downPayment" />
            </UFormGroup>
            <UButton type="submit">Create Invoice</UButton>
        </UForm>
    </div>
</template>

<script setup>
    const { $api } = useNuxtApp();

    const state = reactive({
        invoiceNumber: 'INV-003',
        date: '2024-06-15',
        customerId: 1,
        productIds: [1],
        discount: 0,
        downPayment: 0,
    });

    const fetchCustomers = async () => {
        const { data } = await useAPI('/customers');
        customers.value = data.value.map(customer => ({
            label: customer.name,
            value: customer.id
        }));
    };

    const fetchProducts = async () => {
        const { data } = await useAPI('/products');
        products.value = data.value.map(product => ({
            label: product.name,
            value: product.id
        }));
    };

    const customers = ref()

    const products = ref()

    const router = useRouter();

    const createInvoice = async () => {
        try {
            const invoiceRequest = {
                invoiceNumber: state.invoiceNumber,
                date: state.date,
                customerId: state.customerId,
                productIds: state.productIds,
                discount: state.discount,
                downPayment: state.downPayment
            };
            const data = await $api.raw(`/invoices`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: invoiceRequest
            })

            if (data.status !== 201) {
                throw new Error('Failed to create invoice');
            }

            router.push('/invoices');
        } catch (error) {
            console.error('Error creating invoice:', error);
        }
    };

    fetchCustomers();
    fetchProducts();
</script>

<style scoped>
    form {
        display: flex;
        flex-direction: column;
    }

    div {
        margin-bottom: 1rem;
    }

    label {
        margin-bottom: 0.5rem;
        font-weight: bold;
    }

    input {
        padding: 0.5rem;
        font-size: 1rem;
    }

    button {
        padding: 0.5rem 1rem;
        font-size: 1rem;
        cursor: pointer;
    }
</style>