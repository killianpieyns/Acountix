export default defineNuxtPlugin(() => {
    const api = $fetch.create({
        baseURL: 'https://localhost:3000/api',
    })

    return {
        provide: {
            api
        }
    }
})
