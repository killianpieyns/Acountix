<template>
    <div class="container mx-auto p-4">
        <UCard>
            <template #header>
                <h1>Register</h1>
            </template>
            <UForm @submit="register">
                <UFormGroup label="Email">
                    <UInput id="email" v-model="form.email" type="email" required />
                </UFormGroup>
                <UFormGroup label="Password">
                    <UInput id="password" v-model="form.password" type="password" required />
                </UFormGroup>
                <UFormGroup label="Confirm Password">
                    <UInput id="confirmPassword" v-model="form.confirmPassword" type="password" required />
                </UFormGroup>
                <UButton type="submit">Register</UButton>
            </UForm>
        </UCard>
    </div>
</template>

<script setup lang="ts">
    const form = ref({
        email: '',
        password: '',
        confirmPassword: '',
    });

    const router = useRouter();
    const { $api } = useNuxtApp();

    const register = async () => {
        try {
            const response = await $api('/api/auth/register', {
                method: 'POST',
                body: form.value,
            });
            if (response) {
                router.push('/login');
            }
        } catch (error) {
            console.error('Registration error:', error);
        }
    };
</script>