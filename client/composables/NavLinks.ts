export const useNavLinks = () => {
  return ref({
    core: [
      { name: 'Home', path: '/', authOnly: false },
      { name: 'Invoices', path: '/invoices', authOnly: true },
    ],
    auth: [
      { name: 'Profile', path: '/profile', authOnly: true },
    ],
  }
  )
}
