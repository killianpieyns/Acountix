import { NuxtAuthHandler } from "#auth"
import CognitoProvider from "next-auth/providers/cognito"

export default NuxtAuthHandler({
  secret: useRuntimeConfig().authSecret,
  providers: [
    // @ts-expect-error
    CognitoProvider.default({
      clientId: useRuntimeConfig().COGNITO_CLIENT_ID!,
      clientSecret: useRuntimeConfig().COGNITO_CLIENT_SECRET!,
      issuer: useRuntimeConfig().COGNITO_ISSUER!,
    }),
  ],
  debug: true,
  // callbacks: {
  //   async session({ session, token }) {
  //     // Token we injected into the JWT callback above.
  //     const jwt = token.sessionToken

  //     // Fetch data OR add previous data from the JWT callback.
  //     const additionalUserData = await $fetch(`/api/session/${jwt}`)

  //     // Return the modified session
  //     return {
  //       ...session,
  //       user: {
  //         name: additionalUserData.name,
  //         avatar: additionalUserData.avatar,
  //         role: additionalUserData.role
  //       }
  //     }
  //   },
  // }
})