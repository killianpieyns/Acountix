import type { UseFetchOptions } from 'nuxt/app'

export async function useAPI<T>(
  url: string | (() => string),
  options?: Omit<UseFetchOptions<T>, 'default'> & { default: () => T | Ref<T> },
) {
  return useFetch(url, {
    ...options,
    $fetch: useNuxtApp().$api,
  })
}