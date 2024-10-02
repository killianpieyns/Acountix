<template>
  <div>
    <div class="flex justify-between">
      <h1>Invoices</h1>
      <Button :click="() => navigateTo('/invoices/create')">Create New Invoice</Button>
    </div>
    <Button :click="fetchInvoices">Fetch Invoices</Button>
    <div class="flex flex-col gap-4 mt-8">
      <UCard v-for="invoice in invoices" :key="invoice.id">
        {{ invoice.invoiceNumber }}
        <UButton @click="downloadPdf(invoice.id)">Download PDF</UButton>
        <UButton @click="downloadPeppol(invoice.id)">Download Peppol</UButton>
        <NuxtLink :to="`/invoices/${invoice.id}`">View Details</NuxtLink>
      </UCard>
    </div>
  </div>
</template>

<script setup>
  const invoices = ref([]);

  const fetchInvoices = async () => {
    const { data } = await useAPI('/bff/invoices')
    console.log(data.value);
    invoices.value = data.value;
  };

  const downloadPdf = async (id) => {
    const response = await fetch(`http://localhosts/api/invoices/${id}/pdf`);
    const blob = await response.blob();
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `Invoice_${id}.pdf`;
    a.click();
    window.URL.revokeObjectURL(url);
  };

  const downloadPeppol = async (id) => {
    const response = await fetch(`https://localhost/api/invoices/${id}/peppol`);
    const blob = await response.blob();
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `Invoice_${id}.xml`;
    a.click();
    window.URL.revokeObjectURL(url);
  };

  fetchInvoices();
</script>