<template>
    <header>
        <nav class="flex justify-between items-center container mx-auto">
            <ul>
                <template v-for="link in links.core">
                    <li v-if="link.authOnly === authenticated">
                        <NuxtLink :to=link.path>{{ link.name }}</NuxtLink>
                    </li>
                </template>
            </ul>
            <ul class="flex items-center">
                <template v-for="link in links.auth">
                    <li v-if="link.authOnly === authenticated">
                        <NuxtLink :to=link.path>{{ link.name }}</NuxtLink>
                    </li>
                </template>
                <li>
                    <Button v-if="!authenticated" @click="() => signIn('cognito', { callbackUrl: '/' })">
                        Signin
                    </Button>
                </li>
                <li>
                    <Button v-if="authenticated" @click="() => signOut({ callbackUrl: '/' })">
                        Signout
                    </Button>
                </li>
            </ul>
        </nav>
    </header>
</template>

<script setup>
    const links = useNavLinks();
    const { status, signOut, signIn } = useAuth();
    const authenticated = computed(() => status.value === 'authenticated');
</script>

<style scoped>
    header {
        background-color: #333;
        color: white;
        padding: 10px 0;
        text-align: center;
    }

    nav ul {
        list-style-type: none;
        padding: 0;
    }

    nav ul li {
        display: inline;
        margin-right: 10px;
    }

    nav ul li a {
        color: white;
        text-decoration: none;
    }

    nav ul li a:hover {
        text-decoration: underline;
    }
</style>