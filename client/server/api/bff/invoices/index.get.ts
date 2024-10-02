import { getServerSession, getToken } from "#auth"
import { useCookie } from "nuxt/app"

export default defineEventHandler(async (event) => {
  const session = await getServerSession(event)
  if (!session) {
    throw createError({ statusMessage: 'Unauthenticated', statusCode: 401 })
  }

  const token = useCookie('__Secure-next-auth.session-token')
  console.log('__Secure-next-auth.session-token', token.value)

  const data = $fetch("https://localhost/api/invoices")

  return data
})