<template>
  <Button @click="() => signOut({ callbackUrl: '/' })">
    Signout with redirect
  </Button>
</template>

<script setup lang="ts">
  definePageMeta({
    middleware: 'auth',
  })
  const { signOut } = useAuth()
</script>