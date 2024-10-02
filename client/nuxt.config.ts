// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  runtimeConfig: {
    authSecret: 'your-super-super-secret-here-please-change-me',
    COGNITO_CLIENT_ID: 'your-cognito',
    COGNITO_CLIENT_SECRET: 'your-cognito',
    COGNITO_ISSUER: 'your-cognito'
  },

  devtools: { enabled: true },

  plugins: [
    '~/plugins/signalr.ts',
    '~/plugins/fetch.ts',
  ],

  modules: [
    '@nuxt/ui',
    '@sidebase/nuxt-auth'
  ],

  auth: {
    baseURL: 'https://localhost:3000',
    provider: {
      type: 'authjs',
      trustHost: false,
      defaultProvider: 'cognito',
      addDefaultCallbackUrl: true,
    },

  },

  devServer: {
    https: {
      key: "localhost-key.pem",
      cert: "localhost.pem",
    },
  },
  compatibilityDate: '2024-10-01'
})