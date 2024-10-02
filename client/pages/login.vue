<script setup lang="ts">
// Remember to disable the middleware protection from your page!
definePageMeta({
  auth: { unauthenticatedOnly: true, navigateAuthenticatedTo: '/' }
})

const { signIn } = useAuth()

/*
 * NOTE: Here we hard-coded username and password
 * On your own page this should probably be connected to two inputs
 */
const demoCredentials = { username: 'igod9800@gmail.com', password: '_Azerty12345' }
</script>

<template>
  <div class="flex flex-col gap-4">
    <p>Sign-In Options:</p>
    <div class="flex gap-2 items-start">
        <button @click="signIn('cognito')" class="bg-white text-black ring-1 ring-gray-200 rounded-sm py-2 px-4">
          Cognito
        </button>
    </div>
  </div>
</template>